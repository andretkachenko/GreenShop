using Catalog.Services.Categories;
using Common.Interfaces;
using Common.Models.Categories;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTests.CatalogTests.ServiceTests.CategoriesRepositoryTests
{
    [TestClass]
    public class AddCategoryTests
    {
        private Mock<ISqlDataAccessor<Category>> CategoriesAccessorMock;
        private CategoriesRepository Service;

        public AddCategoryTests()
        {
            CategoriesAccessorMock = new Mock<ISqlDataAccessor<Category>>();
            Service = new CategoriesRepository(CategoriesAccessorMock.Object);
        }

        [TestMethod]
        public void ValidCategory_ReturnsTrue()
        {
            // Arrange
            var id = 1;
            var expectedResult = true;

            CategoriesAccessorMock
                .Setup(categories => categories.Delete(id))
                .Returns(Task.FromResult(1));

            // Act
            var result = Service.DeleteCategory(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCategoryId_ThrowsValidationException()
        {
            // Arrange
            var id = -1;

            // Act
            var result = Service.DeleteCategory(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidCategoryId_ReturnsFalse()
        {
            // Arrange
            var id = 99999;
            var expectedResult = false;

            CategoriesAccessorMock
                .Setup(categories => categories.Delete(id))
                .Returns(Task.FromResult(0));

            // Act
            var result = Service.DeleteCategory(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
