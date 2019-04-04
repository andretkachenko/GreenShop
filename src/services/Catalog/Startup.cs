using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.DataAccessor;
using GreenShop.Catalog.DataAccessors;
using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Infrastructure;
using GreenShop.Catalog.Infrastructure.Products;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Models.Categories;
using GreenShop.Catalog.Models.Comments;
using GreenShop.Catalog.Models.Products;
using GreenShop.Catalog.Services.Categories;
using GreenShop.Catalog.Services.Categories.Interfaces;
using GreenShop.Catalog.Services.Comments;
using GreenShop.Catalog.Services.Comments.Interfaces;
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
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private void InjectDependencies(IServiceCollection services)
        {
            RegisterSingletones(services);
            RegisterScoped(services);
            RegisterTransient(services);
        }

        /// <summary>
        /// Method that registers all Singleton-type dependencies
        /// <para>Singleton objects are the same for every object and every request.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private void RegisterSingletones(IServiceCollection services)
        {
            services.AddSingleton<IMongoContext, MongoContext>();
            services.AddSingleton<ISqlContext, SqlContext>();

            services.AddSingleton<ISqlDataAccessor<Category>, Categories>();
            services.AddSingleton<ISqlChildDataAccessor<Comment>, Comments>();
        }


        /// <summary>
        /// Method that registers all Scoped-type dependencies
        /// <para>Scoped objects are the same within a request, but different across different requests.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private void RegisterScoped(IServiceCollection services)
        {
            services.AddScoped<IMongoProducts, MongoProducts>();
            services.AddScoped<ISqlProducts, SqlProducts>();
            services.AddScoped<IUnitOfWork, DomainScope>();
        }


        /// <summary>
        /// Method that registers all Transient-type dependencies.
        /// <para>Transient objects are provided as a new instance to every controller and every service.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private void RegisterTransient(IServiceCollection services)
        {
            services.AddTransient<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddTransient<ICommentsRepository, CommentsRepository>();
        }
    }
}
