using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GreenShop.Web.Bff.Shopping.Api.Config;
using GreenShop.Web.Bff.Shopping.Api.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using GreenShop.Web.Bff.Shopping.Api.HealthChecks;
using Microsoft.AspNetCore.Http;
using Polly;
using System.Net.Http;
using Polly.Extensions.Http;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using GreenShop.Web.Bff.Shopping.Api.Models.Categories;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Consumers;
using GreenShop.Web.Bff.Shopping.Api.Models.Products;
using GreenShop.Web.Bff.Shopping.Api.Services;
using System;

namespace GreenShop.Web.Bff.Shopping
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddApiVersioning(o => o.ApiVersionReader = new HeaderApiVersionReader("api-version"));
            services.AddSingleton(Configuration);
            services.Configure<UrlsConfig>(options => Configuration.GetSection("urls").Bind(options));

            services
                    // Registers required services for health checks
                    .AddHealthChecksUI()
                    // Register required services for health checks
                    .AddHealthChecks()
                    // Add a health check for a SQL database
                    .AddCheck("Catalog API", new CatalogHealthCheck(Configuration.GetSection("urls").GetSection("catalog").Value));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Web.Bff.Shopping.API", Version = "v1" });
            });

            RegisterHttpServices(services);
            InjectDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                })
                .UseHealthChecksUI(config =>
                {
                    config.ApiPath = "/health-app";
                    config.UIPath = "/health-ui";
                });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.Bff.Shopping.API V1");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(
                    "Navigate to /health to see the health status.\n" +
                    "Navigate to /swagger to see API documentation");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private IServiceCollection RegisterHttpServices(IServiceCollection services)
        {
            services.AddHttpClient<ICatalogService, CatalogService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }

        /// <summary>
        /// Distinct Dependency Injection Block
        /// <para>Singleton objects are the same for every object and every request.</para>
        /// <para>Scoped objects are the same within a request, but different across different requests.</para>
        /// <para>Transient objects are provided as a new instance to every controller and every service.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IConsumer<Category>, CategoriesConsumer>();
            services.AddTransient<IConsumer<Product>, ProductsConsumer>();
            services.AddTransient<ICommentsConsumer, CommentsConsumer>();
            services.AddTransient<ICatalogService, CatalogService>();
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        }

        private IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
