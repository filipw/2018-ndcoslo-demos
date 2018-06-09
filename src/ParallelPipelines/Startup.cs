using AdminApi;
using CustomerApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ParallelPipelines
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.IsolatedMap<StartupAdmin>("/admin");
            app.IsolatedMap<StartupCustomer>("/customer");
        }
    }
}
