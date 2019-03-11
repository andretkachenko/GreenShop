using Common.Models.Categories;
using FluentValidation;
using GreenShop.MVC.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.MVC.Services.CatalogService;

namespace UnitTests.MVC.Services.CatalogService
{
    [TestClass]
    public class EditCategoryAsyncTests
    {
        private Mock<ICatalogConsumer> CatalogConsumerStub;
        private Target CatalogService;

        public EditCategoryAsyncTests()
        {
            CatalogConsumerStub = new Mock<ICatalogConsumer>();
            CatalogService = new Target(CatalogConsumerStub.Object);
        }

        [TestMethod]
        public void ValidCategory_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            string name = "RenamedTestCategory";
            int parentId = 3;
            bool expectedResult = true;

            Category category = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            CatalogConsumerStub
                .Setup(catalog => catalog.EditCategoryAsync(category))
                .Returns(Task.FromResult(true));

            // Act
            Task<bool> result = CatalogService.EditCategoryAsync(category);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCategoryId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;
            string name = "RenamedTestCategory";
            int parentId = 3;

            Category category = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            // Act
            Task<bool> result = CatalogService.EditCategoryAsync(category);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidCategoryId_ReturnsFalse()
        {
            // Arrange
            int id = 99999;
            string name = "NonExistingCategory";
            int parentId = 3;
            bool expectedResult = false;

            Category category = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            CatalogConsumerStub
                .Setup(catalog => catalog.EditCategoryAsync(category))
                .Returns(Task.FromResult(false));

            // Act
            Task<bool> result = CatalogService.EditCategoryAsync(category);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
