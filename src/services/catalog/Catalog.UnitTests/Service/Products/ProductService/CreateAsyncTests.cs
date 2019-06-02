using AutoMapper;
using FluentValidation;
using GreenShop.Catalog.Api.Domain.Products;
using GreenShop.Catalog.Api.Infrastructure;
using GreenShop.Catalog.Api.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Api.Service.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Api.Service.Products.ProductService;

namespace GreenShop.Catalog.UnitTests.Service.Products.ProductService
{
    [TestClass]
    public class CreateAsyncTests
    {
        private Mock<IDomainScope> UnitOfWorkStub;
        private Mock<ISqlProductRepository> SqlProductRepositoryStub;
        private Mock<IMongoProductRepository> MongoProductRepositoryStub;
        private Mock<ICommentRepository> CommentRepositoryStub;
        private Target Service;

        public CreateAsyncTests()
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
            int expectedId = 1;
            string name = "RenamedTestProduct";
            int parentId = 3;
            string description = "TestDescription";
            decimal basePrice = 12m;
            float rating = 4.5f;
            bool expectedResult = true;

            ProductDto product = new ProductDto
            {
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            SqlProductRepositoryStub
                .Setup(products => products.CreateAsync(It.IsAny<Product>()))
                .Returns(Task.FromResult(expectedId));
            MongoProductRepositoryStub
                .Setup(products => products.CreateAsync(It.IsAny<Product>()))
                .Returns(Task.FromResult(expectedResult));

            // Act
            Task<int> result = Service.CreateAsync(product);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<int>));
            Assert.AreEqual(expectedId, result.Result);
        }

        [TestMethod]
        public void EmptyName_ThrowsValidationException()
        {
            // Arrange
            string name = "";

            ProductDto product = new ProductDto
            {
                Name = name
            };

            // Act
            Task<int> result = Service.CreateAsync(product);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
