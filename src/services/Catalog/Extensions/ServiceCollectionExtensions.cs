using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.DataAccessor;
using GreenShop.Catalog.DataAccessors;
using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Models.Categories;
using GreenShop.Catalog.Models.Comments;
using GreenShop.Catalog.Models.Products;
using GreenShop.Catalog.Services.Categories;
using GreenShop.Catalog.Services.Categories.Interfaces;
using GreenShop.Catalog.Services.Comments;
using GreenShop.Catalog.Services.Comments.Interfaces;
using GreenShop.Catalog.Services.Products;
using GreenShop.Catalog.Services.Products.Interfaces;
using GreenShop.Catalog.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace GreenShop.Catalog.Extensions
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
            services.AddSingleton<ISqlChildDataAccessor<Comment>, Comments>();
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
            services.AddTransient<ICommentsRepository, CommentsRepository>();
        }
    }
}
