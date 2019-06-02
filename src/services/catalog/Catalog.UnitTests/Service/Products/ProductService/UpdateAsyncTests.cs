using AutoMapper;
using FluentValidation;
using GreenShop.Catalog.Api.Domain.Products;
using GreenShop.Catalog.Api.Infrastructure;
using GreenShop.Catalog.Api.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Api.Service.Products;
using GreenShop.Catalog.UnitTests.Wrappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Api.Service.Products.ProductService;

namespace GreenShop.Catalog.UnitTests.Service.Products.ProductService
{
    [TestClass]
    public class UpdateAsyncTests
    {
        private Mock<IDomainScope> UnitOfWorkStub;
        private Mock<ISqlProductRepository> SqlProductRepositoryStub;
        private Mock<IMongoProductRepository> MongoProductRepositoryStub;
        private Mock<ICommentRepository> CommentRepositoryStub;
        private Target Service;

        public UpdateAsyncTests()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<Comment, CommentDto>();
                cfg.CreateMap<Specification, SpecificationDto>();
            });

            Mapper mapper = new Mapper(config);
            UnitOfWorkStub = new Mock<IDomainScope>();
            SqlProductRepositoryStub = new Mock<ISqlProductRepository>();
            MongoProductRepositoryStub = new Mock<IMongoProductRepository>();
            CommentRepositoryStub = new Mock<ICommentRepository>();
            Service = new Target(mapper, UnitOfWorkStub.Object);
        }

        [TestInitialize]
        public void Setup()
        {
            UnitOfWorkStub.Setup(x => x.SqlProductRepository)
                    .Returns(SqlProductRepositoryStub.Object);
            UnitOfWorkStub.Setup(x => x.MongoProductRepository)
                    .Returns(MongoProductRepositoryStub.Object);
            UnitOfWorkStub.Setup(x => x.Comments)
                    .Returns(CommentRepositoryStub.Object);
        }

        [TestMethod]
        public void ValidProduct_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            string name = "RenamedTestProduct";
            int parentId = 3;
            string description = "TestDescription";
            decimal basePrice = 12m;
            float rating = 4.5f;
            string mongoId = "TestMongoId";
            bool expectedResult = true;

            ProductDto productDto = new ProductDto
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            ProductWrapper productWrapper = new ProductWrapper
            {
                WrapMongoId = mongoId,
                WrapSpecifications = new List<Specification>(),
                WrapName = name,
                WrapCategoryId = parentId,
                WrapDescription = description
            };

            SqlProductRepositoryStub
                .Setup(products => products.UpdateAsync(productDto))
                .Returns(Task.FromResult(expectedResult));
            MongoProductRepositoryStub
                .Setup(products => products.UpdateAsync(productWrapper))
                .Returns(Task.FromResult(expectedResult));

            // Act
            Task<bool> result = Service.UpdateAsync(productDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeProductId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;
            string name = "RenamedTestProduct";
            int parentId = 3;
            string description = "TestDescription";
            decimal basePrice = 12m;
            float rating = 4.5f;

            ProductDto productDto = new ProductDto
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            // Act
            Task<bool> result = Service.UpdateAsync(productDto);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidProductId_ReturnsFalse()
        {
            // Arrange
            int id = 99999;
            string name = "NonExistingProduct";
            int parentId = 3;
            string description = "TestDescription";
            decimal basePrice = 12m;
            float rating = 4.5f;
            string mongoId = "TestMongoId";
            bool expectedResult = false;

            ProductDto productDto = new ProductDto
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            ProductWrapper productWrapper = new ProductWrapper
            {
                WrapMongoId = mongoId,
                WrapSpecifications = new List<Specification>(),
                WrapName = name,
                WrapCategoryId = parentId,
                WrapDescription = description
            };

            SqlProductRepositoryStub
                .Setup(products => products.UpdateAsync(productDto))
                .Returns(Task.FromResult(expectedResult));
            MongoProductRepositoryStub
                .Setup(products => products.UpdateAsync(productWrapper))
                .Returns(Task.FromResult(expectedResult));

            // Act
            Task<bool> result = Service.UpdateAsync(productDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
