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
    public class AddCategoryTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Target CategoriesService;

        public AddCategoryTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            CategoriesService = new Target(CategoriesConsumerStub.Object);
        }

        [TestMethod]
        public void ValidCategory_ReturnsExpectedId()
        {
            // Arrange
            int id = 1;
            string name = "TestName";
            int parentCategoryId = 2;

            Category expectedCategory = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentCategoryId
            };

            CategoriesConsumerStub
                .Setup(categories => categories.AddAsync(expectedCategory))
                .Returns(Task.FromResult(id));

            // Act
            Task<int> result = CategoriesService.AddCategory(expectedCategory);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<int>));
            Assert.AreEqual(id, result.Result);
        }

        [TestMethod]
        public void EmptyName_ThrowsValidationException()
        {
            // Arrange
            string invalidName = string.Empty;
            Category invalidCategory = new Category
            {
                Name = invalidName
            };

            // Act
            Task<int> result = CategoriesService.AddCategory(invalidCategory);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
