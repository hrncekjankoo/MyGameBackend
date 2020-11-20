using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Net.NetworkInformation;
using System.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MyGame.Domain;
using MyGame.Data;
using MyGame.Services;
using MyGame.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace MyGame
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MyGame",
                    Description = "API documentation for MyGame"
                });
            });

            services.AddAuthentication(options => 
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.LoginPath = "/account/google-login"; 
                })
                .AddGoogle(options => 
                {
                    options.ClientId = "827482706191-mpb50d7hb47o4kfbj729l21dm7ggssrl.apps.googleusercontent.com";
                    options.ClientSecret = "Ow95oXATApVNqAsL_63Dq72U";
                });

            services.RegisterData();
            services.RegisterDomains();

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.KeepAliveInterval = TimeSpan.FromMinutes(2);
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            /*// Login
            app.UseWhen(context => context.Request.Path.StartsWithSegments(new PathString("/login")),
                b => b.Use(async (context, next) =>
                {
                    if (context.Request.Cookies["my_token"] != null)
                    {
                        await next.Invoke();
                    }
                    else
                    {
                        string errorMsg = "User not authenticated. Missing authentication token. Please go to login page.";

                        // Token doesn't exist and user is not authenticated
                        throw new UnauthorizedAccessException(errorMsg);
                            
                    }
                }));

            // API calls
            app.UseWhen(context => context.Request.Path.StartsWithSegments(new PathString("/api")),
                b => b.Use(async (context, next) =>
                {
                    if (context.Request.Cookies["my_token"] != null)
                    {
                        await next.Invoke();
                    }
                    else
                    {
                        string errorMsg = "User not authenticated. Missing authentication token. Please go to login page.";

                        // Token doesn't exist and user is not authenticated
                        throw new UnauthorizedAccessException(errorMsg);
                            
                    }
                }));*/

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MatchHub>("/matchHub");
            });
        }
    }
}
