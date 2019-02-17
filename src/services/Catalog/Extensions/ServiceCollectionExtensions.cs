using Catalog.DataAccessor;
using Catalog.DataAccessors;
using Catalog.Services.Categories;
using Catalog.Services.Categories.Interfaces;
using Catalog.Services.Products;
using Catalog.Services.Products.Interfaces;
using Catalog.Utils;
using Common.Configuration.MongoDB;
using Common.Configuration.SQL;
using Common.Interfaces;
using Common.Models.Categories;
using Common.Models.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Distinct Dependency Injection Block.
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        internal static void InjectDependencies(this IServiceCollection services)
        {
            services.RegisterSingletones();
            services.RegisterScoped();
            services.RegisterTransient();
        }

        /// <summary>
        /// Method that registers all Singleton-type dependencies
        /// <para>Singleton objects are the same for every object and every request.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private static void RegisterSingletones(this IServiceCollection services)
        {
            services.AddSingleton<IMongoContext, MongoContext>();
            services.AddSingleton<ISqlContext, SqlContext>();

            services.AddSingleton<ISqlDataAccessor<Category>, Categories>();
            services.AddSingleton<ISqlDataAccessor<Product>, SqlProducts>();
            services.AddSingleton<IProductMerger, ProductMerger>();
        }


        /// <summary>
        /// Method that registers all Scoped-type dependencies
        /// <para>Scoped objects are the same within a request, but different across different requests.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private static void RegisterScoped(this IServiceCollection services)
        {
            services.AddScoped<IMongoDataAccessor<Product>, MongoProducts>();
        }


        /// <summary>
        /// Method that registers all Transient-type dependencies.
        /// <para>Transient objects are provided as a new instance to every controller and every service.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private static void RegisterTransient(this IServiceCollection services)
        {
            services.AddTransient<ICategoriesRepository, CategoriesRepository>();
            services.AddTransient<IProductsRepository, ProductsRepository>();
        }
    }
}
