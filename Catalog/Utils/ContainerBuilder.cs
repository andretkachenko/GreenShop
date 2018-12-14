using Catalog.DataAccessor;
using Catalog.DataAccessors;
using Catalog.Services.Categories;
using Catalog.Services.Categories.Interfaces;
using Catalog.Services.Products;
using Catalog.Services.Products.Interfaces;
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
        internal void InjectDependencies(IServiceCollection services)
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
            services.AddSingleton<IDataAccessor<Category>, Categories>();

            services.AddSingleton<IDataAccessor<Product>, Products>();
        }


        /// <summary>
        /// Method that registers all Scoped-type dependencies
        /// <para>Scoped objects are the same within a request, but different across different requests.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private void RegisterScoped(IServiceCollection services)
        {

        }


        /// <summary>
        /// Method that registers all Transient-type dependencies.
        /// <para>Transient objects are provided as a new instance to every controller and every service.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private void RegisterTransient(IServiceCollection services)
        {
            // Categories
            services.AddTransient<ICategoriesService, CategoriesService>();

            // Products
            services.AddTransient<IProductsService, ProductsService>();
        }
    }
}
