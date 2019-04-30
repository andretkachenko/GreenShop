using AutoMapper;
using FluentValidation;
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Infrastructure;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Service.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.Wrappers;
using Target = GreenShop.Catalog.Service.Products.ProductService;

namespace UnitTests.Catalog.Service.Products.ProductService
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
            var id = 1;
            var name = "RenamedTestProduct";
            var parentId = 3;
            var description = "TestDescription";
            var basePrice = 12m;
            var rating = 4.5f;
            string mongoId = "TestMongoId";
            var expectedResult = true;

            var productDto = new ProductDto
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            var productWrapper = new ProductWrapper(mongoId, new List<Specification>(), name, parentId, description);

            SqlProductRepositoryStub
                .Setup(products => products.UpdateAsync(productDto))
                .Returns(Task.FromResult(expectedResult));
            MongoProductRepositoryStub
                .Setup(products => products.UpdateAsync(productWrapper))
                .Returns(Task.FromResult(expectedResult));

            // Act
            var result = Service.UpdateAsync(productDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeProductId_ThrowsValidationException()
        {
            // Arrange
            var id = -1;
            var name = "RenamedTestProduct";
            var parentId = 3;
            var description = "TestDescription";
            var basePrice = 12m;
            var rating = 4.5f;

            var productDto = new ProductDto
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            // Act
            var result = Service.UpdateAsync(productDto);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidProductId_ReturnsFalse()
        {
            // Arrange
            var id = 99999;
            var name = "NonExistingProduct";
            var parentId = 3;
            var description = "TestDescription";
            var basePrice = 12m;
            var rating = 4.5f;
            string mongoId = "TestMongoId";
            var expectedResult = false;

            var productDto = new ProductDto
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            var productWrapper = new ProductWrapper(mongoId, new List<Specification>(), name, parentId, description);

            SqlProductRepositoryStub
                .Setup(products => products.UpdateAsync(productDto))
                .Returns(Task.FromResult(expectedResult));
            MongoProductRepositoryStub
                .Setup(products => products.UpdateAsync(productWrapper))
                .Returns(Task.FromResult(expectedResult));

            // Act
            var result = Service.UpdateAsync(productDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
