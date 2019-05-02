using AutoMapper;
using FluentValidation;
using GreenShop.Catalog.Api.Domain.Products;
using GreenShop.Catalog.Api.Infrastructure;
using GreenShop.Catalog.Api.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Api.Service.Products;
using GreenShop.Catalog.UnitTests.Wrappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Api.Service.Products.ProductService;

namespace GreenShop.Catalog.UnitTests.Service.Products.ProductService
{
    [TestClass]
    public class GetAsyncTests
    {
        private Mock<IDomainScope> UnitOfWorkStub;
        private Mock<ISqlProductRepository> SqlProductRepositoryStub;
        private Mock<IMongoProductRepository> MongoProductRepositoryStub;
        private Mock<ICommentRepository> CommentRepositoryStub;
        private Target Service;

        public GetAsyncTests()
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
        public void ValidId_ReturnsExpectedType()
        {
            // Arrange
            int id = 1;
            string mongoId = "TestMongoId";

            SqlProductRepositoryStub
                .Setup(products => products.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidSqlProduct));
            MongoProductRepositoryStub
                .Setup(products => products.GetAsync(mongoId))
                .Returns(Task.FromResult(ExpectedValidMongoProduct));
            CommentRepositoryStub
                .Setup(comments => comments.GetAllParentRelatedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(ExpectedEmptyCommentList));

            // Act
            Task<ProductDto> result = Service.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<ProductDto>));
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedProduct()
        {
            // Arrange
            int id = 1;
            string mongoId = "TestMongoId";

            SqlProductRepositoryStub
                .Setup(products => products.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidSqlProduct));
            SqlProductRepositoryStub
                .Setup(products => products.GetMongoIdAsync(id))
                .Returns(Task.FromResult(mongoId));
            MongoProductRepositoryStub
                .Setup(products => products.GetAsync(mongoId))
                .Returns(Task.FromResult(ExpectedValidMongoProduct));
            CommentRepositoryStub
                .Setup(comments => comments.GetAllParentRelatedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(ExpectedEmptyCommentList));

            // Act
            ProductDto result = Service.GetAsync(id).Result;

            // Assert
            Assert.AreEqual(ExpectedMergedProduct.Id, result.Id);
            Assert.AreEqual(ExpectedMergedProduct.MongoId, result.MongoId);
            Assert.AreEqual(ExpectedMergedProduct.Name, result.Name);
            Assert.AreEqual(ExpectedMergedProduct.CategoryId, result.CategoryId);
            Assert.AreEqual(ExpectedMergedProduct.Description, result.Description);
            Assert.AreEqual(ExpectedMergedProduct.BasePrice, result.BasePrice);
            Assert.AreEqual(ExpectedMergedProduct.Rating, result.Rating);
            Assert.AreEqual(ExpectedMergedProduct.Specifications.First().Name, result.Specifications.First().Name);
            Assert.AreEqual(ExpectedMergedProduct.Specifications.First().MaxSelectionAvailable, result.Specifications.First().MaxSelectionAvailable);
            Assert.AreEqual(ExpectedMergedProduct.Specifications.First().Options.Count(), result.Specifications.First().Options.Count());
            Assert.AreEqual(ExpectedMergedProduct.Specifications.First().Options.First(), result.Specifications.First().Options.First());
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void InvalidId_ReturnsNull()
        {
            // Arrange
            int id = 99999;
            string mongoId = "TestMongoId";

            SqlProductRepositoryStub
                .Setup(products => products.GetAsync(id))
                .Returns(Task.FromResult(ExpectedInvalidProduct));
            MongoProductRepositoryStub
                .Setup(products => products.GetAsync(mongoId))
                .Returns(Task.FromResult(ExpectedValidMongoProduct));
            CommentRepositoryStub
                .Setup(comments => comments.GetAllParentRelatedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(ExpectedEmptyCommentList));

            // Act
            ProductDto result = Service.GetAsync(id).GetAwaiter().GetResult();

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;

            // Act
            Task<ProductDto> result = Service.GetAsync(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        private Product ExpectedValidSqlProduct
        {
            get
            {
                int id = 1;
                string mongoId = "TestMongoId";
                string name = "TestProduct";
                int categoryId = 3;
                string description = "TestDescription";
                decimal basePrice = 12m;
                float rating = 4.5f;

                ProductWrapper product = new ProductWrapper
                {
                    WrapId = id,
                    WrapMongoId = mongoId,
                    WrapBasePrice = basePrice,
                    WrapRating = rating,
                    WrapName = name,
                    WrapCategoryId = categoryId,
                    WrapDescription = description
                };

                return product;
            }
        }

        private Product ExpectedValidMongoProduct
        {
            get
            {
                string mongoId = "TestMongoId";
                string specName = "sampleSpecification";
                int maxSelectionAvailable = 1;
                List<string> specOptions = new List<string> { "opt1" };
                string name = "TestProduct";
                int categoryId = 3;
                string description = "TestDescription";

                ProductWrapper product = new ProductWrapper
                {
                    WrapMongoId = mongoId,
                    WrapSpecifications = new List<Specification> { new Specification(specName, maxSelectionAvailable, specOptions) },
                    WrapName = name,
                    WrapCategoryId = categoryId,
                    WrapDescription = description
                };

                return product;
            }
        }

        private Product ExpectedMergedProduct
        {
            get
            {
                Product product = ExpectedValidSqlProduct;
                product.Specifications = ExpectedValidMongoProduct.Specifications;
                return product;
            }
        }

        private Product ExpectedInvalidProduct
        {
            get
            {
                return null;
            }
        }

        private IEnumerable<Comment> ExpectedEmptyCommentList
        {
            get
            {
                return new List<Comment>();
            }
        }
    }
}
