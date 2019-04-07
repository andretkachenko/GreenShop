using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.DataAccessor;
using GreenShop.Catalog.Domain.Categories;
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Infrastructure;
using GreenShop.Catalog.Infrastructure.Products;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GreenShop.Catalog
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

            app.UseHttpsRedirection();
            app.UseMvc();
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
            services.AddScoped<IComments, Comments>();
            services.AddScoped<IMongoProducts, MongoProducts>();
            services.AddScoped<ISqlProducts, SqlProducts>();
            services.AddScoped<IUnitOfWork, DomainScope>();
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();
        }
    }
}
