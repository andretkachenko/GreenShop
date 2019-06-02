using GreenShop.Catalog.Api;
using GreenShop.Catalog.Api.Service.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GreenShop.Catalog.IntegrationTests.ProductsController
{
    [TestClass]
    public class GetProductAsyncTests
    {
        private HttpClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _client = new CustomWebApplicationFactory<Startup>().CreateClient();
            _client.DefaultRequestHeaders.Add("api-version", "1");
        }

        [TestMethod]
        public async Task ValidId_CanGetProduct()
        {
            // Assert
            int id = 3;

            // Act
            HttpResponseMessage httpResponse = await _client.GetAsync("/api/products/" + id);
            httpResponse.EnsureSuccessStatusCode();
            string stringResponse = await httpResponse.Content.ReadAsStringAsync();
            ProductDto product = JsonConvert.DeserializeObject<ProductDto>(stringResponse);

            // Assign
            Assert.IsTrue(product != null);
            Assert.IsTrue(product.Name == "First Integration Product Name");
            Assert.IsTrue(product.Description == "First Integration Product Description");
        }

        [TestMethod]
        public async Task InvalidId_ReturnsNotFound()
        {
            // Assert
            int id = 1;

            // Act
            HttpResponseMessage httpResponse = await _client.GetAsync("/api/products/" + id);

            // Assign
            Assert.IsTrue(httpResponse.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
