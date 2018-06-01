using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Logging;

namespace RuntimeControllers
{
    public class ApplicationPartWatcher
    {
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
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
            };

            watcher.Changed += (s, e) => _logger.LogInformation("Changed: " + e.FullPath);
            watcher.Renamed += (s, e) => _logger.LogInformation("Renamed: " + e.OldFullPath + " to " + e.FullPath);
            watcher.Created += (s, e) =>
            {
                _logger.LogInformation("Created: " + e.FullPath);
                if (string.Equals(Path.GetExtension(e.FullPath), ".dll", StringComparison.OrdinalIgnoreCase))
                {
                    using (var fs = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, (int)fs.Length);
                        var assembly = Assembly.Load(buffer);

                        if (e.FullPath.EndsWith("Views.dll", StringComparison.OrdinalIgnoreCase))
                        {
                            _applicationPartManager.ApplicationParts.Add(new FileSystemCompiledRazorAssemblyPart(e.FullPath, assembly));
                        }
                        else
                        {
                            _applicationPartManager.ApplicationParts.Add(new FileSystemAssemblyPart(e.FullPath, assembly));
                        }

                        _onDemandActionDescriptorChangeProvider.TokenSource.Cancel();
                    }
                }
            };
            watcher.Deleted += (s, e) =>
            {
                _logger.LogInformation("Deleted: " + e.FullPath);
                var existingfileSystemAssemblyPart = _applicationPartManager.ApplicationParts.FirstOrDefault(x => x is FileSystemAssemblyPart && ((FileSystemAssemblyPart)x).AbsolutePath == e.FullPath);
                if (existingfileSystemAssemblyPart != null)
                {
                    _applicationPartManager.ApplicationParts.Remove(existingfileSystemAssemblyPart);
                    _onDemandActionDescriptorChangeProvider.TokenSource.Cancel();
                    return;
                }

                var existingRazorAssemblyPart = _applicationPartManager.ApplicationParts.FirstOrDefault(x => x is FileSystemCompiledRazorAssemblyPart && ((FileSystemCompiledRazorAssemblyPart)x).AbsolutePath == e.FullPath);
                if (existingRazorAssemblyPart != null)
                {
                    _applicationPartManager.ApplicationParts.Remove(existingRazorAssemblyPart);
                    _onDemandActionDescriptorChangeProvider.TokenSource.Cancel();
                }
            };
            watcher.EnableRaisingEvents = true;
        }
    }
}
