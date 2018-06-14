using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApplicationModelProvider
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISadService, SadService>();
            services.AddSingleton<IHappyService, HappyService>();

            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IApplicationModelProvider, ActionDependencyModelProvider>());

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }

    public class HappyService : IHappyService
    {
        public string BeHappy(string name) => $"😂🦄{name}✨🌈🎉";
    }

    public interface IHappyService
    {
        string BeHappy(string name);
    }

    public class SadService : ISadService
    {
        public string BeSad(string name) => $"😢🚬{name}💀💩🇸🇪";
    }

    public interface ISadService
    {
        string BeSad(string name);
    }
}
