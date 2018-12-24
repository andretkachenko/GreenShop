using Catalog.Services.Products;
using Common.Interfaces;
using Common.Models.Products;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTests.CatalogTests.ServiceTests.ProductsServiceTests
{
    [TestClass]
    public class EditProductTests
    {
        private Mock<IDataAccessor<Product>> ProductsAccessorMock;
        private ProductsService Service;

        public EditProductTests()
        {
            ProductsAccessorMock = new Mock<IDataAccessor<Product>>();
            Service = new ProductsService(ProductsAccessorMock.Object);
        }

        [TestMethod]
        public void ValidProduct_ReturnsTrue()
        {
            // Arrange
            var id = 1;
            var name = "RenamedTestProduct";
            var parentId = 3;
            var description = "TestDescription";
            var basePrice = 12m;
            var rating = 4.5f;
            var expectedResult = true;

            var product = new Product
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            ProductsAccessorMock
                .Setup(products => products.Edit(product))
                .Returns(Task.FromResult(1));

            // Act
            var result = Service.EditProduct(product);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeProductId_ThrowsValidationException()
        {
            // Arrange
            var id = -1;
            var name = "RenamedTestProduct";
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

            // Act
            var result = Service.EditProduct(product);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidProductId_ReturnsFalse()
        {
            // Arrange
            var id = 99999;
            var name = "NonExistingProduct";
            var parentId = 3;
            var description = "TestDescription";
            var basePrice = 12m;
            var rating = 4.5f;
            var expectedResult = false;

            var product = new Product
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            ProductsAccessorMock
                .Setup(products => products.Edit(product))
                .Returns(Task.FromResult(0));

            // Act
            var result = Service.EditProduct(product);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
