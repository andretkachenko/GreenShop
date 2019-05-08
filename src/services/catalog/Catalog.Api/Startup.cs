using AutoMapper;
using GreenShop.Catalog.Api.Config.Interfaces;
using GreenShop.Catalog.Api.DataAccessor;
using GreenShop.Catalog.Api.Domain.Categories;
using GreenShop.Catalog.Api.Infrastructure;
using GreenShop.Catalog.Api.Infrastructure.Products;
using GreenShop.Catalog.Api.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Api.Service.Categories;
using GreenShop.Catalog.Api.Service.Products;
using GreenShop.Catalog.Api.Utils;
using GreenShop.Catalog.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace GreenShop.Catalog.Api
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
            services.AddAutoMapper(typeof(Startup))
                    .AddApiVersioning(o => o.ApiVersionReader = new HeaderApiVersionReader("api-version"))
                    .AddSingleton(Configuration)
                    // Register the Swagger generator, defining 1 or more Swagger documents
                    .AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new Info { Title = "Catalog.API", Version = "v1" });
                    })
                    // Registers required services for health checks
                    .AddHealthChecksUI()
                    // Register required services for health checks
                    .AddHealthChecks()
                    // Add a health check for a SQL database
                    .AddCheck("SQL Database", new SqlConnectionHealthCheck(new SqlContext(Configuration).ConnectionString))
                    .Services
                    .AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Dependency injection block
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
                })
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                .UseSwagger()
                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API V1");
                })
                .UseHttpsRedirection()
                .UseMvc();
        }

        /// <summary>
        /// Distinct Dependency Injection Block.
        /// <para>Singleton objects are the same for every object and every request.</para>
        /// <para>Scoped objects are the same within a request, but different across different requests.</para>
        /// <para>Transient objects are provided as a new instance to every controller and every service.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<ISqlContext, SqlContext>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IMongoProductRepository, MongoProductRepository>();
            services.AddScoped<ISqlProductRepository, SqlProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRepository<Category, CategoryDto>, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddTransient<IDomainScope, DomainScope>();
        }
    }
}
