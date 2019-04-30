using AutoMapper;
using FluentValidation;
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Infrastructure;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Service.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Service.Products.ProductService;

namespace UnitTests.Catalog.Service.Products.ProductService
{
    [TestClass]
    public class DeleteAsyncTests
    {
        private Mock<IDomainScope> UnitOfWorkStub;
        private Mock<ISqlProductRepository> SqlProductRepositoryStub;
        private Mock<IMongoProductRepository> MongoProductRepositoryStub;
        private Mock<ICommentRepository> CommentRepositoryStub;
        private Target Service;

        public DeleteAsyncTests()
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
        public void ValidId_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            string mongoId = "TestMongoId";
            bool expectedResult = true;

            SqlProductRepositoryStub
                .Setup(products => products.DeleteAsync(id))
                .Returns(Task.FromResult(expectedResult));
            SqlProductRepositoryStub
                .Setup(stub => stub.GetMongoIdAsync(id))
                .Returns(Task.FromResult(mongoId));
            MongoProductRepositoryStub
                .Setup(products => products.DeleteAsync(mongoId))
                .Returns(Task.FromResult(expectedResult));
            CommentRepositoryStub
                .Setup(stub => stub.DeleteAllParentRelatedAsync(id))
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
            string mongoId = "TestMongoId";
            bool expectedResult = false;

            SqlProductRepositoryStub
                .Setup(products => products.DeleteAsync(id))
                .Returns(Task.FromResult(expectedResult));
            SqlProductRepositoryStub
                .Setup(stub => stub.GetMongoIdAsync(id))
                .Returns(Task.FromResult(mongoId));
            MongoProductRepositoryStub
                .Setup(products => products.DeleteAsync(mongoId))
                .Returns(Task.FromResult(expectedResult));

            // Act
            Task<bool> result = Service.DeleteAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);

        }
    }
}
