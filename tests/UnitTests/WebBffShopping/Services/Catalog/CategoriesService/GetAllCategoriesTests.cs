using Target = Web.Bff.Shopping.Services.Catalog.CategoriesService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Common.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Web.Bff.Shopping.Services.Catalog.Interfaces;

namespace UnitTests.WebBffShopping.Services.Catalog.CategoriesService
{
    [TestClass]
    public class GetAllCategoriesTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Target CategoriesService;

        public GetAllCategoriesTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            CategoriesService = new Target(CategoriesConsumerStub.Object);
        }

        [TestMethod]
        public void ReturnsExpectedType()
        {
            // Arrange
            CategoriesConsumerStub
                .Setup(categories => categories.GetAllAsync())
                .Returns(Task.FromResult(ExpectedCategoryList));

            // Act
            Task<IEnumerable<Category>> result = CategoriesService.GetAllCategories();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<IEnumerable<Category>>));
        }

        [TestMethod]
        public void ReturnsExpectedCategory()
        {
            // Arrange
            CategoriesConsumerStub
                .Setup(categories => categories.GetAllAsync())
                .Returns(Task.FromResult(ExpectedCategoryList));

            // Act
            Task<IEnumerable<Category>> result = CategoriesService.GetAllCategories();
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
                var id = 1;
                var name = "TestCategory";
                var parentId = 2;

                var categoriesList = new List<Category>()
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
