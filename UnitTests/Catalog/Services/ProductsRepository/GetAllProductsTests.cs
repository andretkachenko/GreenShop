using Target = Catalog.Services.Products.ProductsRepository;
using Catalog.Services.Products.Interfaces;
using Common.Interfaces;
using Common.Models.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                .Returns(Task.FromResult(ExpectedProductList));

            // Act
            var result = ProductsRepository.GetAllProducts();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<IEnumerable<Product>>));
        }

        [TestMethod]
        public void ReturnsExpectedProduct()
        {
            // Arrange
            ProductsSqlAccessorStub
                .Setup(products => products.GetAll())
                .Returns(Task.FromResult(ExpectedProductList));

            // Act
            var result = ProductsRepository.GetAllProducts();

            // Assert
            Assert.AreEqual(result.Result.First(), ExpectedProductList.First());
        }

        private IEnumerable<Product> ExpectedProductList
        {
            get
            {
                var id = 1;
                var name = "TestProduct";
                var parentId = 3;
                var description = "TestDescription";
                var basePrice = 12m;
                var rating = 4.5f;

                var productsList = new List<Product>()
                    {
                        new Product
                        {
                            Id = id,
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
    }
}
