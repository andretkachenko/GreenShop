﻿namespace ApiGateway.Config
{
    public class UrlsConfig
    {
        public class CatalogOperations
        {
            public static string GetAllCategories() => $"/api/categories";
            public static string GetCategory(int id) => $"/api/categories/{id}";
            public static string AddCategory() => $"/api/categories";
            public static string DeleteCategory(int id) => $"/api/categories/{id}";
            public static string EditCategory() => $"/api/categories";

            public static string GetAllProducts() => $"/api/products";
            public static string GetProduct(int id) => $"/api/products/{id}";
            public static string AddProduct() => $"/api/products";
            public static string DeleteProduct(int id) => $"/api/products/{id}";
            public static string EditProduct() => $"/api/products";

            public static string GetAllPruductComments(int productId) => $"/api/comments/product/{productId}";
            public static string GetComment(int id) => $"/api/comments/{id}";
            public static string EditComment(int id) => $"/api/comments/{id}";
            public static string DeleteComment(int id) => $"/api/comments/{id}";
        }

        public string Catalog { get; set; }
    }
}
