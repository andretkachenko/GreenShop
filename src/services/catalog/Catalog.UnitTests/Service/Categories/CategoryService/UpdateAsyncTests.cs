using AutoMapper;
using FluentValidation;
using GreenShop.Catalog.Api.Domain.Categories;
using GreenShop.Catalog.Api.Infrastructure;
using GreenShop.Catalog.Api.Service.Categories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Api.Service.Categories.CategoryService;

namespace GreenShop.Catalog.UnitTests.Service.Categories.CategoryService
{
    [TestClass]
    public class UpdateAsyncTests
    {
        private Mock<IDomainScope> UnitOfWorkStub;
        private Mock<IRepository<Category, CategoryDto>> CategoriesAccessorStub;
        private Target Service;

        public UpdateAsyncTests()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Category, CategoryDto>());
            Mapper mapper = new Mapper(config);
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
        public void ValidCategory_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            string name = "RenamedTestCategory";
            int parentId = 3;
            bool expectedResult = true;

            CategoryDto categoryDto = new CategoryDto
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            CategoriesAccessorStub
                .Setup(categories => categories.UpdateAsync(categoryDto))
                .Returns(Task.FromResult(expectedResult));

            // Act
            Task<bool> result = Service.UpdateAsync(categoryDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCategoryId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;
            string name = "RenamedTestCategory";
            int parentId = 3;

            CategoryDto categoryDto = new CategoryDto
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            // Act
            Task<bool> result = Service.UpdateAsync(categoryDto);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidCategoryId_ReturnsFalse()
        {
            // Arrange
            int id = 99999;
            string name = "NonExistingCategory";
            int parentId = 3;
            bool expectedResult = false;

            CategoryDto categoryDto = new CategoryDto
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            CategoriesAccessorStub
                .Setup(categories => categories.UpdateAsync(categoryDto))
                .Returns(Task.FromResult(expectedResult));

            // Act
            Task<bool> result = Service.UpdateAsync(categoryDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
