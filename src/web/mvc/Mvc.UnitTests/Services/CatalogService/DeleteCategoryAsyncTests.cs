using FluentValidation;
using GreenShop.Web.Mvc.App.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Mvc.App.Services.CatalogService;

namespace GreenShop.Web.Mvc.UnitTests.Services.CatalogService
{
    [TestClass]
    public class DeleteCategoryAsyncTests
    {
        private Mock<ICatalogConsumer> CatalogConsumerStub;
        private Target CatalogService;

        public DeleteCategoryAsyncTests()
        {
            CatalogConsumerStub = new Mock<ICatalogConsumer>();
            CatalogService = new Target(CatalogConsumerStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            bool expectedResult = true;

            CatalogConsumerStub
                .Setup(catalog => catalog.DeleteCategoryAsync(id))
                .Returns(Task.FromResult(true));

            // Act
            Task<bool> result = CatalogService.DeleteCategoryAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;

            // Act
            Task<bool> result = CatalogService.DeleteCategoryAsync(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidId_ReturnsFalse()
        {
            // Arrange
            int id = 99999;
            bool expectedResult = false;

            CatalogConsumerStub
                .Setup(catalog => catalog.DeleteCategoryAsync(id))
                .Returns(Task.FromResult(false));

            // Act
            Task<bool> result = CatalogService.DeleteCategoryAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);

        }
    }
}
