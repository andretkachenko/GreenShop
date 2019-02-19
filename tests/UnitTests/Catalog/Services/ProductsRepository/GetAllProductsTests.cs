using Target = Catalog.Services.Products.ProductsRepository;
using Catalog.Services.Products.Interfaces;
using Common.Interfaces;
using Common.Models.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.Specifications;

namespace UnitTests.Catalog.Services.ProductsRepository
{
    [TestClass]
    public class GetAllProductsTests
    {
        private Mock<ISqlDataAccessor<Product>> ProductsSqlAccessorStub;
        private Mock<IMongoDataAccessor<Product>> ProductsMongoAccessorStub;
        private Mock<IProductMerger> ProductMergerStub;
        private Target ProductsRepository;

        public GetAllProductsTests()
        {
            ProductsSqlAccessorStub = new Mock<ISqlDataAccessor<Product>>();
            ProductsMongoAccessorStub = new Mock<IMongoDataAccessor<Product>>();
            ProductMergerStub = new Mock<IProductMerger>();
            ProductsRepository = new Target(ProductsSqlAccessorStub.Object, ProductsMongoAccessorStub.Object, ProductMergerStub.Object);
        }

        [TestMethod]
        public void ReturnsExpectedType()
        {
            // Arrange
            ProductsSqlAccessorStub
                .Setup(products => products.GetAll())
                .Returns(Task.FromResult(ExpectedProductSqlList));
            ProductsMongoAccessorStub
                .Setup(products => products.GetAll())
                .Returns(Task.FromResult(ExpectedProductMongoList));

            // Act
            var result = ProductsRepository.GetAllProducts();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<IEnumerable<Product>>));
        }

        [TestMethod]
        public void ReturnsExpectedProduct()
        {
            // Arrange
            var expectedProduct = ExpectedMergedList.First();
            ProductsSqlAccessorStub
                .Setup(products => products.GetAll())
                .Returns(Task.FromResult(ExpectedProductSqlList));
            ProductsMongoAccessorStub
                .Setup(products => products.GetAll())
                .Returns(Task.FromResult(ExpectedProductMongoList));
            ProductMergerStub
                .Setup(stub => stub.MergeProducts(It.IsAny<IEnumerable<Product>>(), It.IsAny<IEnumerable<Product>>()))
                .Returns(ExpectedMergedList);

            // Act
            var result = ProductsRepository.GetAllProducts();
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
                        new Product
                        {
                            Id = id,
                            MongoId = mongoId,
                            Name = name,
                            CategoryId = parentId,
                            Description = description,
                            BasePrice = basePrice,
                            Rating = rating
                        }
                    };

                return productsList;
            }
        }

        private IEnumerable<Product> ExpectedProductMongoList
        {
            get
            {
                var mongoId = "TestMongoId";
                var specName = "sampleSpecification";
                var maxSelectionAvailable = 1;
                var specOptions = new List<string> { "opt1" };

                var productsList = new List<Product>()
                {
                    new Product
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
                    }
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
    }
}
