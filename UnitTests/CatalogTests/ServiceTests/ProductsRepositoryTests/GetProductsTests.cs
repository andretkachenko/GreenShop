using Catalog.Services.Products;
using Catalog.Services.Products.Interfaces;
using Common.Interfaces;
using Common.Models.Products;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTests.CatalogTests.ServiceTests.ProductsRepositoryTests
{
    [TestClass]
    public class GetProductsTests
    {
        private Mock<ISqlDataAccessor<Product>> ProductsSqlAccessorMock;
        private Mock<IMongoDataAccessor<Product>> ProductsMongoAccessorMock;
        private Mock<IProductMerger> ProductMergerMock;
        private ProductsRepository Service;

        public GetProductsTests()
        {
            ProductsSqlAccessorMock = new Mock<ISqlDataAccessor<Product>>();
            ProductsMongoAccessorMock = new Mock<IMongoDataAccessor<Product>>();
            ProductMergerMock = new Mock<IProductMerger>();
            Service = new ProductsRepository(ProductsSqlAccessorMock.Object, ProductsMongoAccessorMock.Object, ProductMergerMock.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedType()
        {
            // Arrange
            var id = 1;

            ProductsSqlAccessorMock
                .Setup(products => products.Get(id))
                .Returns(Task.FromResult(ExpectedValidProduct));

            // Act
            var result = Service.GetProduct(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Product>));
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedProduct()
        {
            // Arrange
            var id = 1;

            ProductsSqlAccessorMock
                .Setup(products => products.Get(id))
                .Returns(Task.FromResult(ExpectedValidProduct));

            // Act
            var result = Service.GetProduct(id);

            // Assert
            Assert.AreEqual(result.Result, ExpectedValidProduct);
        }

        [TestMethod]
        public void InvalidId_ReturnsNull()
        {
            // Arrange
            var id = 99999;

            ProductsSqlAccessorMock
                .Setup(products => products.Get(id))
                .Returns(Task.FromResult(ExpectedInvalidProduct));

            // Act
            var result = Service.GetProduct(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Product>));
            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            var id = -1;

            // Act
            var result = Service.GetProduct(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        private Product ExpectedValidProduct
        {
            get
            {
                var id = 1;
                var name = "TestProduct";
                var parentId = 3;
                var description = "TestDescription";
                var basePrice = 12m;
                var rating = 4.5f;

                var product = new Product
                {
                    Id = id,
                    Name = name,
                    CategoryId = parentId,
                    Description = description,
                    BasePrice = basePrice,
                    Rating = rating
                };

                return product;
            }
        }

        private Product ExpectedInvalidProduct
        {
            get
            {
                return null;
            }
        }
    }
}
