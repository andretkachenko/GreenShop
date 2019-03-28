using FluentValidation;
using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Models.Categories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Services.Categories.CategoriesRepository;

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
            int id = 1;
            bool expectedResult = true;

            CategoriesAccessorStub
                .Setup(categories => categories.Delete(id))
                .Returns(Task.FromResult(1));

            // Act
            Task<bool> result = CategoriesRepository.DeleteCategory(id);

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
            Task<Category> result = CategoriesRepository.GetCategory(id);

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

            CategoriesAccessorStub
                .Setup(categories => categories.Delete(id))
                .Returns(Task.FromResult(0));

            // Act
            Task<bool> result = CategoriesRepository.DeleteCategory(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);

        }
    }
}
