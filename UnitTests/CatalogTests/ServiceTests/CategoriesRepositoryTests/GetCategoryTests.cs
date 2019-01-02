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
    public class GetCategoryTests
    {
        private Mock<ISqlDataAccessor<Category>> CategoriesAccessorMock;
        private CategoriesRepository Service;

        public GetCategoryTests()
        {
            CategoriesAccessorMock = new Mock<ISqlDataAccessor<Category>>();
            Service = new CategoriesRepository(CategoriesAccessorMock.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedType()
        {
            // Arrange
            var id = 1;

            CategoriesAccessorMock
                .Setup(categories => categories.Get(id))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            var result = Service.GetCategory(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Category>));
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedCategory()
        {
            // Arrange
            var id = 1;

            CategoriesAccessorMock
                .Setup(categories => categories.Get(id))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            var result = Service.GetCategory(id);

            // Assert
            Assert.AreEqual(result.Result, ExpectedValidCategory);
        }

        [TestMethod]
        public void InvalidId_ReturnsNull()
        {
            // Arrange
            var id = 99999;

            CategoriesAccessorMock
                .Setup(categories => categories.Get(id))
                .Returns(Task.FromResult(ExpectedInvalidCategory));

            // Act
            var result = Service.GetCategory(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Category>));
            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            var id = -1;

            // Act
            var result = Service.GetCategory(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        private Category ExpectedValidCategory
        {
            get
            {
                var id = 1;
                var name = "TestCategory";
                var parentId = 2;

                var category = new Category
                {
                    Id = id,
                    Name = name,
                    ParentCategoryId = parentId
                };

                return category;
            }
        }

        private Category ExpectedInvalidCategory
        {
            get
            {
                return null;
            }
        }
    }
}
