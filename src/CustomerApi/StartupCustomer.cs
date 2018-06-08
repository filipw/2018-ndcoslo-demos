using CustomerApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using ParallelPipelines;

namespace CustomerApi
{
    public class StartupCustomer
    {
        public void ConfigureServices(IServiceCollection s)
        {
            s.AddTransient<IOrderService, CustomerOrderService>();
            s.AddMvc().ConfigureApplicationPartManager(manager =>
            {
                manager.ApplicationParts.Clear();
                manager.ApplicationParts.Add(new AssemblyPart(GetType().Assembly));
            });
        }

        public void Configure(IApplicationBuilder a)
        {
            a.UseMvc();
        }
    }
}
