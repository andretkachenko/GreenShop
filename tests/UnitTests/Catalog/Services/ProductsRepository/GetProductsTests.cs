using Target = GreenShop.Catalog.Services.Products.ProductsRepository;
using GreenShop.Catalog.Services.Products.Interfaces;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using GreenShop.Catalog.Models.Products;
using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Models.Specifications;

namespace UnitTests.Catalog.Services.ProductsRepository
{
    [TestClass]
    public class GetProductsTests
    {
        private Mock<ISqlDataAccessor<Product>> ProductsSqlAccessorStub;
        private Mock<IMongoDataAccessor<Product>> ProductsMongoAccessorStub;
        private Mock<IProductMerger> ProductMergerStub;
        private Target ProductsRepository;

        public GetProductsTests()
        {
            ProductsSqlAccessorStub = new Mock<ISqlDataAccessor<Product>>();
            ProductsMongoAccessorStub = new Mock<IMongoDataAccessor<Product>>();
            ProductMergerStub = new Mock<IProductMerger>();
            ProductsRepository = new Target(ProductsSqlAccessorStub.Object, ProductsMongoAccessorStub.Object, ProductMergerStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedType()
        {
            // Arrange
            var id = 1;
            var mongoId = "TestMongoId";

            ProductsSqlAccessorStub
                .Setup(products => products.Get(id))
                .Returns(Task.FromResult(ExpectedValidSqlProduct));
            ProductsMongoAccessorStub
                .Setup(products => products.Get(mongoId))
                .Returns(Task.FromResult(ExpectedValidMongoProduct));
            ProductMergerStub
                .Setup(stub => stub.MergeProduct(It.IsAny<Product>(), It.IsAny<Product>()))
                .Returns(ExpectedMergedProduct);

            // Act
            var result = ProductsRepository.GetProduct(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Product>));
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedProduct()
        {
            // Arrange
            var id = 1;
            var mongoId = "TestMongoId";

            ProductsSqlAccessorStub
                .Setup(products => products.Get(id))
                .Returns(Task.FromResult(ExpectedValidMongoProduct));
            ProductsMongoAccessorStub
                .Setup(products => products.Get(mongoId))
                .Returns(Task.FromResult(ExpectedValidMongoProduct));
            ProductMergerStub
                .Setup(stub => stub.MergeProduct(It.IsAny<Product>(), It.IsAny<Product>()))
                .Returns(ExpectedMergedProduct);

            // Act
            var result = ProductsRepository.GetProduct(id).Result;

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
            var id = 99999;
            var mongoId = "TestMongoId";

            ProductsSqlAccessorStub
                .Setup(products => products.Get(id))
                .Returns(Task.FromResult(ExpectedInvalidProduct));
            ProductsMongoAccessorStub
                .Setup(products => products.Get(mongoId))
                .Returns(Task.FromResult(ExpectedValidMongoProduct));
            ProductMergerStub
                .Setup(stub => stub.MergeProduct(It.IsAny<Product>(), It.IsAny<Product>()))
                .Throws(new ArgumentNullException());

            // Act
            var result = ProductsRepository.GetProduct(id).GetAwaiter().GetResult();

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            var id = -1;

            // Act
            var result = ProductsRepository.GetProduct(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        private Product ExpectedValidSqlProduct
        {
            get
            {
                var id = 1;
                var name = "TestProduct";
                var parentId = 3;
                var description = "TestDescription";
                var basePrice = 12m;
                var rating = 4.5f;

                var product = new Product
                {
                    Id = id,
                    Name = name,
                    CategoryId = parentId,
                    Description = description,
                    BasePrice = basePrice,
                    Rating = rating
                };

                return product;
            }
        }

        private Product ExpectedValidMongoProduct
        {
            get
            {
                var mongoId = "TestMongoId";
                var specName = "sampleSpecification";
                var maxSelectionAvailable = 1;
                var specOptions = new List<string> { "opt1" };

                var product = new Product
                {
                    MongoId = mongoId,
                    Specifications = new List<Specification>
                        {
                            new Specification
                            {
                                Name = specName,
                                MaxSelectionAvailable = maxSelectionAvailable,
                                Options = specOptions
                            }
                        }
                };

                return product;
            }
        }

        private Product ExpectedMergedProduct
        {
            get
            {
                var product = ExpectedValidSqlProduct;
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
    }
}
