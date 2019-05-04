﻿namespace GreenShop.Web.Bff.Shopping.Api.Config
{
    public partial class UrlsConfig
    {
        public class CommentApiOperations
        {
            public static string AddComment => $"api/comments/";
            public static string GetAllProductComments(int productId) => $"api/comments/product/{productId}";
            public static string GetComment(int id) => $"api/comments/{id}";
            public static string EditComment(int id) => $"api/comments/{id}";
            public static string DeleteComment(int id) => $"api/comments/{id}";
        }
    }
}