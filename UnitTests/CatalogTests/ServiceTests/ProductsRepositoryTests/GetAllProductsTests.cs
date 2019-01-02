using Catalog.Services.Products;
using Catalog.Services.Products.Interfaces;
using Common.Interfaces;
using Common.Models.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.CatalogTests.ServiceTests.ProductsRepositoryTests
{
    [TestClass]
    public class GetAllProductsTests
    {
        private Mock<ISqlDataAccessor<Product>> ProductsSqlAccessorMock;
        private Mock<IMongoDataAccessor<Product>> ProductsMongoAccessorMock;
        private Mock<IProductMerger> ProductMergerMock;
        private ProductsRepository Service;

        public GetAllProductsTests()
        {
            ProductsSqlAccessorMock = new Mock<ISqlDataAccessor<Product>>();
            ProductsMongoAccessorMock = new Mock<IMongoDataAccessor<Product>>();
            ProductMergerMock = new Mock<IProductMerger>();
            Service = new ProductsRepository(ProductsSqlAccessorMock.Object, ProductsMongoAccessorMock.Object, ProductMergerMock.Object);
        }

        [TestMethod]
        public void ReturnsExpectedType()
        {
            // Arrange
            ProductsSqlAccessorMock
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
            ProductsSqlAccessorMock
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
