using Catalog;
using Common.Models.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Catalog.ProductsController
{
    [TestClass]
    public class GetAllProductsAsyncTests
    {
        private HttpClient _client;

        [TestInitialize]
        public void Initialize()
        {
            _client = new CustomWebApplicationFactory<Startup>().CreateClient();
        }

        [TestMethod]
        public async Task CanGetProducts()
        {
            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync("/api/products");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(stringResponse);
            Assert.IsTrue(products.First() != null);
            Assert.IsTrue(products.First().Name == "First Integration Product Name");
            Assert.IsTrue(products.First().Description == "First Integration Product Description");
        }
    }
}
