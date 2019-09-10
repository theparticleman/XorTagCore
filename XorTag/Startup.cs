using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XorTag.Domain;

namespace XorTag
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Scan(ts =>
            {
                ts.FromCallingAssembly().AddClasses().AsMatchingInterface().AsSelf().WithTransientLifetime();
            });
            services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
            services.AddMemoryCache();
            services.AddResponseCaching();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.Use(async (ctx, next) =>
            {
                ctx.Request.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(60)
                };
                await next();
            });

            app.UseResponseCaching();
            app.UseStaticFiles();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMvc();
        }
    }
}
