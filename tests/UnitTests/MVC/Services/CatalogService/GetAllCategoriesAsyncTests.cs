using GreenShop.MVC.Models.Categories;
using GreenShop.MVC.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Target = GreenShop.MVC.Services.CatalogService;

namespace UnitTests.MVC.Services.CatalogService
{
    [TestClass]
    public class GetAllCategoriesAsyncTests
    {
        private Mock<ICatalogConsumer> CatalogConsumerStub;
        private Target CatalogService;

        public GetAllCategoriesAsyncTests()
        {
            CatalogConsumerStub = new Mock<ICatalogConsumer>();
            CatalogService = new Target(CatalogConsumerStub.Object);
        }

        [TestMethod]
        public void ReturnsExpectedType()
        {
            // Arrange
            CatalogConsumerStub
                .Setup(catalog => catalog.GetAllCategoriesAsync())
                .Returns(Task.FromResult(ExpectedCategoryList));

            // Act
            Task<IEnumerable<Category>> result = CatalogService.GetAllCategoriesAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<IEnumerable<Category>>));
        }

        [TestMethod]
        public void ReturnsExpectedCategory()
        {
            // Arrange
            CatalogConsumerStub
                .Setup(catalog => catalog.GetAllCategoriesAsync())
                .Returns(Task.FromResult(ExpectedCategoryList));

            // Act
            Task<IEnumerable<Category>> result = CatalogService.GetAllCategoriesAsync();
            Category actualCategory = result.GetAwaiter().GetResult().First();
            Category expectedCategory = ExpectedCategoryList.First();

            // Assert
            Assert.AreEqual(result.Result.Count(), ExpectedCategoryList.Count());
            Assert.AreEqual(actualCategory.Id, expectedCategory.Id);
            Assert.AreEqual(actualCategory.Name, expectedCategory.Name);
            Assert.AreEqual(actualCategory.ParentCategoryId, expectedCategory.ParentCategoryId);
        }

        private IEnumerable<Category> ExpectedCategoryList
        {
            get
            {
                int id = 1;
                string name = "TestCategory";
                int parentId = 2;

                List<Category> categoriesList = new List<Category>()
                    {
                        new Category
                        {
                            Id = id,
                            Name = name,
                            ParentCategoryId = parentId
                        }
                    };

                return categoriesList;
            }
        }
    }
}
