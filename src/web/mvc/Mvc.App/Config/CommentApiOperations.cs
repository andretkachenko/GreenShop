﻿namespace GreenShop.Web.Mvc.App.Config
{
    public partial class UrlsConfig
    {
        public partial class WebShoppingApiOperations
        {
            public class CommentApiOperations
            {
                public static string AddComment => $"api/catalog/comments/";
                public static string GetAllProductComments(int productId) => $"api/catalog/comments/products/{productId}";
                public static string GetComment(int id) => $"api/catalog/comments/{id}";
                public static string EditComment(int id) => $"api/catalog/comments/{id}";
                public static string DeleteComment(int id) => $"api/catalog/comments/{id}";
            }
        }
    }
}
