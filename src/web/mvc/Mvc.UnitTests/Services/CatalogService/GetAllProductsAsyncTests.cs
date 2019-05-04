using GreenShop.Web.Mvc.App.Models.Products;
using GreenShop.Web.Mvc.App.Models.Specifications;
using GreenShop.Web.Mvc.App.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Mvc.App.Services.CatalogService;

namespace GreenShop.Web.Mvc.UnitTests.Services.CatalogService
{
    [TestClass]
    public class GetAllProductsAsyncTests
    {
        private Mock<ICatalogConsumer> CatalogConsumerStub;
        private Target CatalogService;

        public GetAllProductsAsyncTests()
        {
            CatalogConsumerStub = new Mock<ICatalogConsumer>();
            CatalogService = new Target(CatalogConsumerStub.Object);
        }

        [TestMethod]
        public void ReturnsExpectedType()
        {
            // Arrange
            CatalogConsumerStub
                .Setup(catalog => catalog.GetAllProductsAsync())
                .Returns(Task.FromResult(ExpectedProductList));

            // Act
            Task<IEnumerable<Product>> result = CatalogService.GetAllProductsAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<IEnumerable<Product>>));
        }

        [TestMethod]
        public void ReturnsExpectedProduct()
        {
            // Arrange
            Product expectedProduct = ExpectedProductList.First();
            CatalogConsumerStub
                .Setup(catalog => catalog.GetAllProductsAsync())
                .Returns(Task.FromResult(ExpectedProductList));

            // Act
            Task<IEnumerable<Product>> result = CatalogService.GetAllProductsAsync();
            Product actualProduct = result.Result.First();

            // Assert
            Assert.AreEqual(result.Result.Count(), ExpectedProductList.Count());
            Assert.AreEqual(actualProduct.Id, expectedProduct.Id);
            Assert.AreEqual(actualProduct.Name, expectedProduct.Name);
            Assert.AreEqual(actualProduct.CategoryId, expectedProduct.CategoryId);
            Assert.AreEqual(actualProduct.Description, expectedProduct.Description);
            Assert.AreEqual(actualProduct.BasePrice, expectedProduct.BasePrice);
            Assert.AreEqual(actualProduct.Rating, expectedProduct.Rating);
            Assert.AreEqual(actualProduct.Specifications.First().Name, expectedProduct.Specifications.First().Name);
            Assert.AreEqual(actualProduct.Specifications.First().MaxSelectionAvailable, expectedProduct.Specifications.First().MaxSelectionAvailable);
            Assert.AreEqual(actualProduct.Specifications.First().Options.Count(), expectedProduct.Specifications.First().Options.Count());
            Assert.AreEqual(actualProduct.Specifications.First().Options.First(), expectedProduct.Specifications.First().Options.First());
        }

        private IEnumerable<Product> ExpectedProductList
        {
            get
            {
                int id = 1;
                string name = "TestProduct";
                int parentId = 3;
                string description = "TestDescription";
                decimal basePrice = 12m;
                float rating = 4.5f;
                string specName = "sampleSpecification";
                int maxSelectionAvailable = 1;
                List<string> specOptions = new List<string> { "opt1" };

                List<Product> productsList = new List<Product>()
                    {
                        new Product
                        {
                            Id = id,
                            Name = name,
                            CategoryId = parentId,
                            Description = description,
                            BasePrice = basePrice,
                            Rating = rating,
                            Specifications = new List<Specification>
                        {
                            new Specification
                            {
                                Name = specName,
                                MaxSelectionAvailable = maxSelectionAvailable,
                                Options = specOptions
                            }
                        }
                        }
                    };

                return productsList;
            }
        }
    }
}
