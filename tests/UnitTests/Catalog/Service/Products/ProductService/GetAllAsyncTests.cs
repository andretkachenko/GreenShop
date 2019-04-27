using AutoMapper;
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
    public class GetAllAsyncTests
    {
        private Mock<IDomainScope> UnitOfWorkStub;
        private Mock<ISqlProductRepository> SqlProductRepositoryStub;
        private Mock<IMongoProductRepository> MongoProductRepositoryStub;
        private Mock<ICommentRepository> CommentRepositoryStub;
        private Target Service;

        public GetAllAsyncTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<Comment, CommentDto>();
                cfg.CreateMap<Specification, SpecificationDto>();
            });

            var mapper = new Mapper(config);
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
        public void ReturnsExpectedType()
        {
            // Arrange
            SqlProductRepositoryStub
                .Setup(products => products.GetAllAsync())
                .Returns(Task.FromResult(ExpectedProductSqlList));
            MongoProductRepositoryStub
                .Setup(products => products.GetAllAsync())
                .Returns(Task.FromResult(ExpectedProductMongoList));

            // Act
            var result = Service.GetAllAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<IEnumerable<ProductDto>>));
        }

        [TestMethod]
        public void ReturnsExpectedProduct()
        {
            // Arrange
            var expectedProduct = ExpectedMergedList.First();
            SqlProductRepositoryStub
                .Setup(products => products.GetAllAsync())
                .Returns(Task.FromResult(ExpectedProductSqlList));
            MongoProductRepositoryStub
                .Setup(products => products.GetAllAsync())
                .Returns(Task.FromResult(ExpectedProductMongoList));
            CommentRepositoryStub
                .Setup(comments => comments.GetAllParentRelatedAsync(It.IsAny<IEnumerable<int>>()))
                .Returns(Task.FromResult(EmptyCommentsList));

            // Act
            var result = Service.GetAllAsync();
            var actualProduct = result.Result.First();

            // Assert
            Assert.AreEqual(result.Result.Count(), ExpectedMergedList.Count());
            Assert.AreEqual(actualProduct.Id, expectedProduct.Id);
            Assert.AreEqual(actualProduct.MongoId, expectedProduct.MongoId);
            Assert.AreEqual(actualProduct.Name, expectedProduct.Name);
            Assert.AreEqual(actualProduct.CategoryId, expectedProduct.CategoryId);
            Assert.AreEqual(actualProduct.Description, expectedProduct.Description);
            Assert.AreEqual(actualProduct.BasePrice, expectedProduct.BasePrice);
            Assert.AreEqual(actualProduct.Rating, expectedProduct.Rating);
            Assert.AreEqual(actualProduct.Specifications.First().Name, expectedProduct.Specifications.First().Name);
            Assert.AreEqual(actualProduct.Specifications.First().MaxSelectionAvailable, expectedProduct.Specifications.First().MaxSelectionAvailable);
            Assert.AreEqual(actualProduct.Specifications.First().Options.Count(), expectedProduct.Specifications.First().Options.Count());
            Assert.AreEqual(actualProduct.Specifications.First().Options.First(), expectedProduct.Specifications.First().Options.First());
        }

        private IEnumerable<Product> ExpectedProductSqlList
        {
            get
            {
                var id = 1;
                var name = "TestProduct";
                var mongoId = "TestMongoId";
                var parentId = 3;
                var description = "TestDescription";
                var basePrice = 12m;
                var rating = 4.5f;

                var productsList = new List<Product>()
                    {
                        new ProductWrapper(id, mongoId, basePrice, rating, name, parentId, description)
                    };

                return productsList;
            }
        }

        private IEnumerable<Product> ExpectedProductMongoList
        {
            get
            {
                var name = "TestProduct";
                var parentId = 3;
                var description = "TestDescription";
                var mongoId = "TestMongoId";
                var specName = "sampleSpecification";
                var maxSelectionAvailable = 1;
                var specOptions = new List<string> { "opt1" };

                var productsList = new List<Product>()
                {
                    new ProductWrapper(mongoId, 
                                       new List<Specification> { new Specification(specName, maxSelectionAvailable, specOptions) }, 
                                       name, 
                                       parentId, 
                                       description)
                };

                return productsList;
            }
        }

        private IEnumerable<Product> ExpectedMergedList
        {
            get
            {
                var expectedProduct = ExpectedProductSqlList.First();
                expectedProduct.Specifications = ExpectedProductMongoList.First().Specifications;

                return new List<Product> { expectedProduct };
            }
        }

        private Dictionary<int, IEnumerable<Comment>> EmptyCommentsList
        {
            get
            {
                return new Dictionary<int, IEnumerable<Comment>>();
            }
        }
    }
}
