﻿using Target = Web.Bff.Shopping.Services.Catalog.CategoriesService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Web.Bff.Shopping.Services.Catalog.Consumers;
using Common.Models.Categories;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace UnitTests.WebBffShopping.Services.Catalog.CategoriesService
{
    [TestClass]
    public class GetCategoryTests
    {        
        private Mock<CategoriesConsumer> CategoriesAccessorStub;
        private Target CategoriesService;

        public GetCategoryTests()
        {
            CategoriesAccessorStub = new Mock<CategoriesConsumer>();
            CategoriesService = new Target(CategoriesAccessorStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedType()
        {
            // Arrange
            int id = 1;

            CategoriesAccessorStub
                .Setup(categories => categories.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            Task<Category> result = CategoriesService.GetCategory(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Category>));
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedCategory()
        {
            // Arrange
            int id = 1;

            CategoriesAccessorStub
                .Setup(categories => categories.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            Task<Category> result = CategoriesService.GetCategory(id);
            Category actualCategory = result.GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(actualCategory.Id, ExpectedValidCategory.Id);
            Assert.AreEqual(actualCategory.Name, ExpectedValidCategory.Name);
            Assert.AreEqual(actualCategory.ParentCategoryId, ExpectedValidCategory.ParentCategoryId);
        }

        [TestMethod]
        public void InvalidId_ReturnsNull()
        {
            // Arrange
            int id = 99999;

            CategoriesAccessorStub
                .Setup(categories => categories.GetAsync(id))
                .Returns(Task.FromResult(ExpectedInvalidCategory));

            // Act
            Task<Category> result = CategoriesService.GetCategory(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Category>));
            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;

            // Act
            Task<Category> result = CategoriesService.GetCategory(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        private Category ExpectedValidCategory
        {
            get
            {
                int id = 1;
                string name = "TestCategory";
                int parentId = 2;

                Category category = new Category
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
