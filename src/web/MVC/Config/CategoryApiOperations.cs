namespace MvcWebApp.Config
{
    public partial class UrlsConfig
    {
        public partial class WebShoppingApiOperations
        {
            public class CategoryApiOperations
            {
                public static string GetAllCategories = $"api/catalog/categories";
                public static string GetCategory(int id) => $"api/catalog/categories/{id}";
                public static string AddCategory = $"api/catalog/categories";
                public static string DeleteCategory(int id) => $"api/catalog/categories/{id}";
                public static string EditCategory = $"api/catalog/categories";
            }
        }
    }
}
