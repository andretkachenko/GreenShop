using Dapper;
using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Models.Products;
using GreenShop.Catalog.Properties;
using GreenShop.Catalog.Services.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenShop.Catalog.Services.Products
{
    public class ProductMerger : IProductMerger
    {
        private readonly ISqlContext _sql;

        public ProductMerger(ISqlContext sql)
        {
            _sql = sql;
        }

        /// <summary>
        /// Get MongoId field for a Product with the specified Id
        /// </summary>
        /// <param name="id">Id of a Product</param>
        /// <returns>MongoId</returns>
        public string GetMongoId(int id)
        {
            using (System.Data.SqlClient.SqlConnection context = _sql.Context)
            {
                string mongoId = context.Query<string>(@"
                    SELECT [MongoId]
                    FROM [Products]
                    WHERE [Id] = @id
                ", new
                {
                    id
                }).FirstOrDefault();

                return mongoId;
            }
        }

        /// <summary>
        /// Merge Lists of Products from SQL and MongoDB
        /// </summary>
        /// <param name="sqlProducts">List of Products from SQL DB</param>
        /// <param name="mongoProducts">List of Products from MongoDB</param>
        /// <returns>List of merged Products</returns>
        public IEnumerable<Product> MergeProducts(IEnumerable<Product> sqlProducts, IEnumerable<Product> mongoProducts)
        {
            List<Product> products = new List<Product>();

            foreach (Product sqlProduct in sqlProducts)
            {
                products.Add(MergeProduct(sqlProduct, mongoProducts.FirstOrDefault(x => x.MongoId == sqlProduct.MongoId)));
            }

            return products;
        }

        /// <summary>
        /// Merge Products from SQL and MongoDB
        /// </summary>
        /// <param name="sqlProduct">Product from SQL DB</param>
        /// <param name="mongoProduct">Product from MongoDB</param>
        /// <returns>Merged Product</returns>
        public Product MergeProduct(Product sqlProduct, Product mongoProduct)
        {
            if (sqlProduct == null)
            {
                throw new ArgumentNullException(Resources.NullSqlProductException);
            }
            if (mongoProduct == null)
            {
                return sqlProduct;
            }
            if (sqlProduct.MongoId != mongoProduct.MongoId)
            {
                throw new ArgumentException(Resources.ProductsMismatchException);
            }

            sqlProduct.Specifications = mongoProduct.Specifications;

            return sqlProduct;
        }

    }
}
