using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Composite;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace RuntimeControllers
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<OnDemandActionDescriptorChangeProvider>();
            services.AddSingleton<IActionDescriptorChangeProvider>(f => f.GetService<OnDemandActionDescriptorChangeProvider>());
            services.AddSingleton<ApplicationPartWatcher>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationPartWatcher applicationPartWatcher)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseMvcWithDefaultRoute();

            Task.Run(() => applicationPartWatcher.Watch("plugins"));
        }
    }
}
