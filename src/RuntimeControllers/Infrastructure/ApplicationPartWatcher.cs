using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Logging;

namespace RuntimeControllers
{
    public class ApplicationPartWatcher
    {
        private ConcurrentDictionary<string, List<Assembly>> _loadedAssemblies = new ConcurrentDictionary<string, List<Assembly>>();
        private readonly OnDemandActionDescriptorChangeProvider _onDemandActionDescriptorChangeProvider;
        private readonly ApplicationPartManager _applicationPartManager;
        private readonly ILogger<ApplicationPartWatcher> _logger;

        public ApplicationPartWatcher(OnDemandActionDescriptorChangeProvider onDemandActionDescriptorChangeProvider, 
                ApplicationPartManager applicationPartManager, ILoggerFactory loggerFactory)
        {
            _onDemandActionDescriptorChangeProvider = onDemandActionDescriptorChangeProvider;
            _applicationPartManager = applicationPartManager;
            _logger = loggerFactory.CreateLogger<ApplicationPartWatcher>();
        }

        public void Watch(string path)
        {
            var configFolderPath = Path.Combine(Directory.GetCurrentDirectory(), path);

            if (!Directory.Exists(configFolderPath))
                Directory.CreateDirectory(configFolderPath);

            var watcher = new FileSystemWatcher()
            {
                Path = configFolderPath,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.DirectoryName
            };

            watcher.Changed += (s, e) => _logger.LogInformation("Changed: " + e.FullPath);
            watcher.Renamed += (s, e) => _logger.LogInformation("Renamed: " + e.OldFullPath + " to " + e.FullPath);
            watcher.Created += (s, e) =>
            {
                _logger.LogInformation("Created: " + e.FullPath);
                //hack to let the file complete the creation...
                Thread.Sleep(1000);

                var loadedFolderAssemblies = new List<Assembly>();
                foreach (var file in Directory.EnumerateFiles(e.FullPath, "*.dll"))
                {
                    using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var assembly = AssemblyLoadContext.Default.LoadFromStream(fs);
                        loadedFolderAssemblies.Add(assembly);

                        var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
                        foreach (var part in partFactory.GetApplicationParts(assembly))
                        {
                            _applicationPartManager.ApplicationParts.Add(part);
                        }

                    }
                }

                if (loadedFolderAssemblies.Any())
                {
                    _loadedAssemblies[e.FullPath] = loadedFolderAssemblies;
                    _onDemandActionDescriptorChangeProvider.TokenSource.Cancel();
                }
            };
            watcher.Deleted += (s, e) =>
            {
                _logger.LogInformation("Deleted: " + e.FullPath);

                if (_loadedAssemblies.TryGetValue(e.FullPath, out var loadedFolderAssemblies))
                {
                    foreach (var assembly in loadedFolderAssemblies)
                    {
                        var existingAssemblyPart = _applicationPartManager.ApplicationParts.FirstOrDefault(x => x.Name == assembly.GetName().Name);
                        if (existingAssemblyPart != null)
                        {
                            _applicationPartManager.ApplicationParts.Remove(existingAssemblyPart);
                        }
                    }

                    _onDemandActionDescriptorChangeProvider.TokenSource.Cancel();
                }

            };
            watcher.EnableRaisingEvents = true;
        }
    }
}
