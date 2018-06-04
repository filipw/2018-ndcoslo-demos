using AdminApi;
using CustomerApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WebApiContrib.Core;

namespace ParallelPipelines
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseBranchWithServices("/admin",
                s =>
                {
                    s.AddAuthentication(o =>
                    {
                        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    }).AddJwtBearer();

                    s.AddAuthorization(o => o.AddPolicy("admin", b => b.RequireAuthenticatedUser()));

                    s.AddTransient<IHiService, AdminService>();
                    s.AddMvc(o => 
                    {
                        o.Filters.Add(new AuthorizeFilter("admin"));
                    }).ConfigureApplicationPartManager(manager =>
                    {
                        manager.ApplicationParts.Clear();
                        manager.ApplicationParts.Add(new AssemblyPart(typeof(AdminService).Assembly));
                    });
                },
                a =>
                {
                    a.UseAuthentication();
                    a.UseMvc();
                });

            app.UseBranchWithServices("/customer",
                s =>
                {
                    s.AddTransient<IHiService, PublicService>();
                    s.AddMvc().ConfigureApplicationPartManager(manager =>
                    {
                        manager.ApplicationParts.Clear();
                        manager.ApplicationParts.Add(new AssemblyPart(typeof(PublicService).Assembly));
                    });
                },
                a =>
                {
                    a.UseMvc();
                });

            app.Run(async c =>
            {
                await c.Response.WriteAsync("Nothing here!");
            });
        }
    }
}
