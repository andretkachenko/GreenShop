namespace MvcWebApp.Config
{
    public partial class UrlsConfig
    {
        public partial class WebShoppingApiOperations
        {
            public class CategoryApiOperations
            {
                public static string GetAllCategories() => $"/api/categories";
                public static string GetCategory(int id) => $"/api/categories/{id}";
                public static string AddCategory() => $"/api/categories";
                public static string DeleteCategory(int id) => $"/api/categories/{id}";
                public static string EditCategory() => $"/api/categories";
            }
        }
    }
}
