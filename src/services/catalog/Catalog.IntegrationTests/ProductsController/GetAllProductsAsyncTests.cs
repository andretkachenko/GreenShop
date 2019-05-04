using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GreenShop.Catalog.IntegrationTests.ProductsController
{
    [TestClass]
    public class GetAllProductsAsyncTests
    {
        private HttpClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _client = new CustomWebApplicationFactory<Startup>().CreateClient();
            _client.DefaultRequestHeaders.Add("api-version", "1");
        }

        [TestMethod]
        public async Task CanGetProducts()
        {
            // The endpoint or route of the controller action.
            HttpResponseMessage httpResponse = await _client.GetAsync("/api/products");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            string stringResponse = await httpResponse.Content.ReadAsStringAsync();
            IEnumerable<ProductDto> products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(stringResponse);
            Assert.IsTrue(products.First() != null);
            Assert.IsTrue(products.First().Name == "First Integration Product Name");
            Assert.IsTrue(products.First().Description == "First Integration Product Description");
        }
    }
}
