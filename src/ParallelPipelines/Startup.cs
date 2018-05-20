using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using ParallelPipelines.CustomerApi;
using ParallelPipelines.DashboardApi;
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
            app.UseBranchWithServices("/dashboard",
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
                    }).AddSpecificControllersOnly<DashboardBaseController>();
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
                    s.AddMvc().AddSpecificControllersOnly<CustomerBaseController>();
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
