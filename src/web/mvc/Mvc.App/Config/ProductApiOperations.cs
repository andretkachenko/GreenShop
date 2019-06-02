namespace GreenShop.Web.Mvc.App.Config
{
    public partial class UrlsConfig
    {
        public partial class WebShoppingApiOperations
        {
            public class ProductApiOperations
            {
                public static string GetAllProducts = $"api/catalog/products";
                public static string GetProduct(int id) => $"api/catalog/products/{id}";
                public static string GetProductWithCategory(int id) => $"api/catalog/products/{id}/category";
                public static string AddProduct = $"api/catalog/products";
                public static string DeleteProduct(int id) => $"api/catalog/products/{id}";
                public static string EditProduct = $"api/catalog/products";
            }
        }
    }
}
