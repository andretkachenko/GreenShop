using Target = Catalog.Services.Categories.CategoriesRepository;
using Common.Interfaces;
using Common.Models.Categories;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTests.Catalog.Services.CategoriesRepository
{
    [TestClass]
    public class DeleteCategoryTests
    {
        private Mock<ISqlDataAccessor<Category>> CategoriesAccessorStub;
        private Target CategoriesRepository;

        public DeleteCategoryTests()
        {
            CategoriesAccessorStub = new Mock<ISqlDataAccessor<Category>>();
            CategoriesRepository = new Target(CategoriesAccessorStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsTrue()
        {
            // Arrange
            var id = 1;
            var expectedResult = true;

            CategoriesAccessorStub
                .Setup(categories => categories.Delete(id))
                .Returns(Task.FromResult(1));

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

            CategoriesAccessorStub
                .Setup(categories => categories.Delete(id))
                .Returns(Task.FromResult(0));

            // Act
            var result = CategoriesRepository.DeleteCategory(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);

        }
    }
}
