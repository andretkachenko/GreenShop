namespace Web.Bff.Shopping.Config
{
    public partial class UrlsConfig
    {
        public class CommentApiOperations
        {
            public static string GetAllPruductComments(int productId) => $"/api/comments/product/{productId}";
            public static string GetComment(int id) => $"/api/comments/{id}";
            public static string EditComment(int id) => $"/api/comments/{id}";
            public static string DeleteComment(int id) => $"/api/comments/{id}";
        }
    }
}
