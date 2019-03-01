namespace Web.Bff.Shopping.Config
{
    public partial class UrlsConfig
    {
        public class ProductApiOperations
        {
            public static string GetAllProducts => $"api/products";
            public static string GetProduct(int id) => $"api/products/{id}";
            public static string AddProduct => $"api/products";
            public static string DeleteProduct(int id) => $"api/products/{id}";
            public static string EditProduct => $"api/products";
        }
        
    }
}
