using Catalog.DataAccessor;
using Catalog.DataAccessors;
using Catalog.Services.Categories;
using Catalog.Services.Categories.Interfaces;
using Catalog.Services.Products;
using Catalog.Services.Products.Interfaces;
using Common.Configuration.MongoDB;
using Common.Configuration.SQL;
using Common.Interfaces;
using Common.Models.Categories;
using Common.Models.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Utils
{
    internal sealed class ContainerBuilder
    {
        /// <summary>
        /// Distinct Dependency Injection Block.
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        internal static void InjectDependencies(IServiceCollection services)
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
        private static void RegisterSingletones(IServiceCollection services)
        {
            services.AddSingleton<IMongoContext, MongoContext>();
            services.AddSingleton<ISqlContext, SqlContext>();

            services.AddSingleton<IDataAccessor<Category>, Categories>();
        }


        /// <summary>
        /// Method that registers all Scoped-type dependencies
        /// <para>Scoped objects are the same within a request, but different across different requests.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private static void RegisterScoped(IServiceCollection services)
        {
            services.AddScoped<IDataAccessor<Product>, Products>();
        }


        /// <summary>
        /// Method that registers all Transient-type dependencies.
        /// <para>Transient objects are provided as a new instance to every controller and every service.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private static void RegisterTransient(IServiceCollection services)
        {
            services.AddTransient<ICategoriesRepository, CategoriesRepository>();
            services.AddTransient<IProductsRepository, ProductsRepository>();
        }
    }
}
