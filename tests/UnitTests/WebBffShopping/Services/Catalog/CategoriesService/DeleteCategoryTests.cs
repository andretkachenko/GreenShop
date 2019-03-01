using Target = Web.Bff.Shopping.Services.Catalog.CategoriesService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Common.Models.Categories;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using FluentValidation;

namespace UnitTests.WebBffShopping.Services.Catalog.CategoriesService
{
    [TestClass]
    public class DeleteCategoryTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Target CategoriesRepository;

        public DeleteCategoryTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            CategoriesRepository = new Target(CategoriesConsumerStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsTrue()
        {
            // Arrange
            var id = 1;
            var expectedResult = true;

            CategoriesConsumerStub
                .Setup(categories => categories.DeleteAsync(id))
                .Returns(Task.FromResult(true));

            // Act
            var result = CategoriesRepository.DeleteCategory(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            var id = -1;

            // Act
            var result = CategoriesRepository.GetCategory(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidId_ReturnsFalse()
        {
            // Arrange
            var id = 99999;
            var expectedResult = false;

            CategoriesConsumerStub
                .Setup(categories => categories.DeleteAsync(id))
                .Returns(Task.FromResult(false));

            // Act
            var result = CategoriesRepository.DeleteCategory(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);

        }
    }
}
