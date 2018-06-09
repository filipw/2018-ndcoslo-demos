using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

namespace FileProvider
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            var blobOptions = Configuration.GetSection("AzureBlobOptions").Get<AzureBlobOptions>();
            var azureBlobFileProvider = new AzureBlobFileProvider(blobOptions);
            services.AddSingleton(azureBlobFileProvider);

            services.AddMvc().ConfigureApplicationPartManager(a =>
            {
                var binDirectory = azureBlobFileProvider.GetDirectoryContents("bin");

                foreach (var item in binDirectory)
                {
                    using (var assemblyStream = item.CreateReadStream())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            assemblyStream.CopyTo(ms);
                            var assembly = Assembly.Load(ms.ToArray());
                            a.ApplicationParts.Add(new AssemblyPart(assembly));
                        }
                    }
                }
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();

            var blobFileProvider = app.ApplicationServices.GetRequiredService<AzureBlobFileProvider>();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = blobFileProvider,
                RequestPath = "/files"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = blobFileProvider,
                RequestPath = "/files"
            });

            app.UseMvc();
        }
    }
}
