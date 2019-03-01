using Common.Interfaces;
using Common.Models.Categories;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using Target = Catalog.Services.Categories.CategoriesRepository;

namespace UnitTests.Catalog.Services.CategoriesRepository
{
    [TestClass]
    public class AddCategoryTests
    {
        private Mock<ISqlDataAccessor<Category>> CategoriesAccessorStub;
        private Target CategoriesRepository;

        public AddCategoryTests()
        {
            CategoriesAccessorStub = new Mock<ISqlDataAccessor<Category>>();
            CategoriesRepository = new Target(CategoriesAccessorStub.Object);
        }

        [TestMethod]
        public void ValidCategory_ReturnsExpectedId()
        {
            // Arrange
            int id = 1;
            string name = "TestCategory";
            int parentId = 3;

            Category category = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            CategoriesAccessorStub
                .Setup(categories => categories.Add(category))
                .Returns(Task.FromResult(id));

            // Act
            Task<int> result = CategoriesRepository.AddCategory(category);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<int>));
            Assert.AreEqual(id, result.Result);
        }

        [TestMethod]
        public void CategoryWithEmptyName_ThrowsValidationException()
        {
            // Arrange
            string emptyName = string.Empty;
            Category invalidCategory = new Category
            {
                Name = emptyName
            };

            // Act
            Task<int> result = CategoriesRepository.AddCategory(invalidCategory);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void CategoryWithoutName_ThrowsArgumentNullException()
        {
            // Arrange
            Category invalidCategory = new Category {  };

            // Act
            Task<int> result = CategoriesRepository.AddCategory(invalidCategory);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ArgumentNullException));
        }
    }
}
