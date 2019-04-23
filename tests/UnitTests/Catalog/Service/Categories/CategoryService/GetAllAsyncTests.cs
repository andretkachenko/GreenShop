using AutoMapper;
using GreenShop.Catalog.Domain.Categories;
using GreenShop.Catalog.Infrastructure;
using GreenShop.Catalog.Service.Categories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.Wrappers;
using Target = GreenShop.Catalog.Service.Categories.CategoryService;

namespace UnitTests.Catalog.Service.Categories.CategoryService
{
    [TestClass]
    public class GetAllAsyncTests
    {
        private Mock<IDomainScope> UnitOfWorkStub;
        private Mock<IRepository<Category, CategoryDto>> CategoriesAccessorStub;
        private Target Service;

        public GetAllAsyncTests()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Category, CategoryDto>());
            var mapper = new Mapper(config);
            UnitOfWorkStub = new Mock<IDomainScope>();
            CategoriesAccessorStub = new Mock<IRepository<Category, CategoryDto>>();
            Service = new Target(mapper, UnitOfWorkStub.Object);
        }

        [TestInitialize]
        public void Setup()
        {
            UnitOfWorkStub.Setup(x => x.CategoryRepository)
                    .Returns(CategoriesAccessorStub.Object);
        }

        [TestMethod]
        public void ReturnsExpectedType()
        {
            // Arrange
            CategoriesAccessorStub
                .Setup(categories => categories.GetAllAsync())
                .Returns(Task.FromResult(ExpectedCategoryList));

            // Act
            Task<IEnumerable<CategoryDto>> result = Service.GetAllAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<IEnumerable<CategoryDto>>));
        }

        [TestMethod]
        public void ReturnsExpectedCategory()
        {
            // Arrange
            CategoriesAccessorStub
                .Setup(categories => categories.GetAllAsync())
                .Returns(Task.FromResult(ExpectedCategoryList));

            // Act
            Task<IEnumerable<CategoryDto>> result = Service.GetAllAsync();
            //CategoryDto actualCategory = result.GetAwaiter().GetResult().First();
            var q = result.GetAwaiter();
            var w = q.GetResult();
            var e = w.First();

            // Assert
            Assert.AreEqual(result.Result.Count(), ExpectedCategoryList.Count());
            Assert.AreEqual(e.Id, ExpectedCategory.Id);
            Assert.AreEqual(e.Name, ExpectedCategory.Name);
            Assert.AreEqual(e.ParentCategoryId, ExpectedCategory.ParentCategoryId);
        }

        private IEnumerable<Category> ExpectedCategoryList
        {
            get
            {
                List<Category> categoriesList = new List<Category>()
                {
                    ExpectedCategory
                };

                return categoriesList;
            }
        }

        private Category ExpectedCategory
        {
            get
            {
                int id = 1;
                string name = "TestCategory";
                int parentId = 2;
                CategoryWrapper category = new CategoryWrapper(id, name, parentId);

                return category;
            }
        }
    }
}
