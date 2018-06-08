using AdminApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using ParallelPipelines;

namespace AdminApi
{
    public class StartupAdmin
    {
        public void ConfigureServices(IServiceCollection s)
        {
            s.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            s.AddAuthorization(o => o.AddPolicy("admin", b => b.RequireAuthenticatedUser()));

            s.AddTransient<IOrderService, AdminOrderService>();
            s.AddMvc(o =>
            {
                o.Filters.Add(new AuthorizeFilter("admin"));
            }).ConfigureApplicationPartManager(manager =>
            {
                manager.ApplicationParts.Clear();
                manager.ApplicationParts.Add(new AssemblyPart(GetType().Assembly));
            });
        }

        public void Configure(IApplicationBuilder a)
        {
            a.UseAuthentication();
            a.UseMvc();
        }
    }
}
