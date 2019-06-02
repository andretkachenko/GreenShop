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
    public class DeleteAsyncTests
    {
        private Mock<IDomainScope> UnitOfWorkStub;
        private Mock<IRepository<Category, CategoryDto>> CategoriesAccessorStub;
        private Target Service;

        public DeleteAsyncTests()
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
        public void ValidId_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            bool expectedResult = true;

            CategoriesAccessorStub
                .Setup(categories => categories.DeleteAsync(id))
                .Returns(Task.FromResult(expectedResult));

            // Act
            Task<bool> result = Service.DeleteAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;

            // Act
            Task<bool> result = Service.DeleteAsync(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidId_ReturnsFalse()
        {
            // Arrange
            int id = 99999;
            bool expectedResult = false;

            CategoriesAccessorStub
                .Setup(categories => categories.DeleteAsync(id))
                .Returns(Task.FromResult(expectedResult));

            // Act
            Task<bool> result = Service.DeleteAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);

        }
    }
}
