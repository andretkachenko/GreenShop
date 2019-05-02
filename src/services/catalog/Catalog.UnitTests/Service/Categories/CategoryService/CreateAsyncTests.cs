using AutoMapper;
using FluentValidation;
using GreenShop.Catalog.Api.Domain.Categories;
using GreenShop.Catalog.Api.Infrastructure;
using GreenShop.Catalog.Api.Service.Categories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Api.Service.Categories.CategoryService;

namespace GreenShop.Catalog.UnitTests.Service.Categories.CategoryService
{
    [TestClass]
    public class CreateAsyncTests
    {
        private Mock<IDomainScope> UnitOfWorkStub;
        private Mock<IRepository<Category, CategoryDto>> CategoriesAccessorStub;
        private Target Service;

        public CreateAsyncTests()
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
        public void ValidCategory_ReturnsExpectedId()
        {
            // Arrange
            int id = 1;
            string name = "TestCategory";
            int parentId = 3;

            CategoryDto categoryDto = new CategoryDto
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            CategoriesAccessorStub
                .Setup(categories => categories.CreateAsync(It.IsAny<Category>()))
                .Returns(Task.FromResult(id));

            // Act
            Task<int> result = Service.CreateAsync(categoryDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<int>));
            Assert.AreEqual(id, result.Result);
        }

        [TestMethod]
        public void CategoryWithEmptyName_ThrowsValidationException()
        {
            // Arrange
            string emptyName = string.Empty;
            CategoryDto invalidCategory = new CategoryDto()
            {
                Name = emptyName
            };

            // Act
            Task<int> result = Service.CreateAsync(invalidCategory);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void CategoryWithoutName_ThrowsArgumentNullException()
        {
            // Arrange
            CategoryDto invalidCategory = new CategoryDto { };

            // Act
            Task<int> result = Service.CreateAsync(invalidCategory);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ArgumentNullException));
        }
    }
}
