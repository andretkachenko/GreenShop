using Catalog.Services.Products;
using Common.Interfaces;
using Common.Models.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.CatalogTests.ServiceTests.ProductsServiceTests
{
    [TestClass]
    public class GetAllProductsTests
    {
        private Mock<IDataAccessor<Product>> ProductsAccessorMock;
        private ProductsService Service;

        public GetAllProductsTests()
        {
            ProductsAccessorMock = new Mock<IDataAccessor<Product>>();
            Service = new ProductsService(ProductsAccessorMock.Object);
        }

        [TestMethod]
        public void ReturnsExpectedType()
        {
            // Arrange
            ProductsAccessorMock
                .Setup(products => products.GetAll())
                .Returns(Task.FromResult(ExpectedProductList));

            // Act
            var result = Service.GetAllProducts();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<IEnumerable<Product>>));
        }

        [TestMethod]
        public void ReturnsExpectedProduct()
        {
            // Arrange
            ProductsAccessorMock
                .Setup(products => products.GetAll())
                .Returns(Task.FromResult(ExpectedProductList));

            // Act
            var result = Service.GetAllProducts();

            // Assert
            Assert.AreEqual(result.Result.First(), ExpectedProductList.First());
        }

        private IEnumerable<Product> ExpectedProductList
        {
            get
            {
                var id = 1;
                var name = "TestProduct";
                var parentId = 3;
                var description = "TestDescription";
                var basePrice = 12m;
                var rating = 4.5f;

                var productsList = new List<Product>()
                    {
                        new Product
                        {
                            Id = id,
                            Name = name,
                            CategoryId = parentId,
                            Description = description,
                            BasePrice = basePrice,
                            Rating = rating
                        }
                    };

                return productsList;
            }
        }
    }
}
