namespace GreenShop.Web.Mvc.App.Config
{
    public partial class UrlsConfig
    {
        public partial class WebShoppingApiOperations
        {
            public class CategoryApiOperations
            {
                public static string GetAllCategories = $"api/catalog/categories";
                public static string GetCategory(int id) => $"api/catalog/categories/{id}";
                public static string GetCategoryWithRelatedProducts(int id) => $"api/catalog/categories/{id}/products";
                public static string AddCategory = $"api/catalog/categories";
                public static string DeleteCategory(int id) => $"api/catalog/categories/{id}";
                public static string EditCategory = $"api/catalog/categories";
            }
        }
    }
}