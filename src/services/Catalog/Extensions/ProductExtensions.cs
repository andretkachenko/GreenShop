using Common.Models.Products;
using System.Linq;

namespace Catalog.Extensions
{
    public static class ProductExtensions
    {
        /// <summary>
        /// Check if specified Product has any properties filled, that are stored in MongoDB
        /// </summary>
        /// <param name="product">Product to investigate</param>
        public static bool HasMongoProperties(this Product product)
        {
            return product.Specifications != null && product.Specifications.Any();
        }

        /// <summary>
        /// Check if specified Product has any properties filled, that are stored in SQL DB
        /// </summary>
        /// <param name="product">Product to investigate</param>
        public static bool HasSqlProperties(this Product product)
        {
            return !string.IsNullOrWhiteSpace(product.Name)
                    || product.CategoryId != 0
                    || !string.IsNullOrWhiteSpace(product.Description)
                    || product.BasePrice != 0
                    || product.Rating != 0;
        }
    }
}
