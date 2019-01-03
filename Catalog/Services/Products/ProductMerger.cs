﻿using Catalog.Services.Products.Interfaces;
using Common.Configuration.SQL;
using Common.Models.Products;
using Dapper;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Services.Products
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
            using (var context = _sql.Context)
            {
                var mongoId = context.Query<string>(@"
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
            var products = new List<Product>();

            foreach (var sqlProduct in sqlProducts)
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
            if (mongoProduct != null)
            {
                sqlProduct.Specifications = mongoProduct.Specifications;
            }

            return sqlProduct;
        }

    }
}