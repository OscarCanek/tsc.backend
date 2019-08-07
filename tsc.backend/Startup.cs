using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using tsc.backend.lib.Countries;
using tsc.backend.lib.Models;
using tsc.backend.lib.Subdivisions;

namespace tsc.backend
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
            // add CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowWorld",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(2520)));
            });

            // database configuration
            services.AddDbContext<TscContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("tsc"))
                );

            // configures the country handler
            services.AddScoped<ICountryHandler, CountryHandler>(x =>
            {
                var tscContext = x.GetService<TscContext>() as TscContext;
                return new CountryHandler(tscContext);
            });

            // configures the subdivision handler
            services.AddScoped<ISubdivisionHandler, SubdivisionHandler>(x =>
            {
                var tscContext = x.GetService<TscContext>() as TscContext;
                return new SubdivisionHandler(tscContext);
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TSC Countries API", Version = "v1" });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TSC Countries V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();

            // UseCors with CorsPolicyBuilder.
            app.UseCors("AllowWorld");
        }
    }
}
