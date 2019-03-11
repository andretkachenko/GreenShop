using Common.Models.Categories;
using Common.Models.Products;
using Common.Models.Specifications;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = Web.Bff.Shopping.Services.CatalogService;

namespace UnitTests.WebBffShopping.Services.CatalogService
{
    [TestClass]
    public class GetProductWithCategoryAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public GetProductWithCategoryAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedType()
        {
            // Arrange
            int productId = 1;

            ProductsConsumerStub
                .Setup(products => products.GetAsync(productId))
                .Returns(Task.FromResult(ExpectedValidProduct));
            CategoriesConsumerStub
                .Setup(categories => categories.GetAsync(ExpectedValidProduct.CategoryId))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            Task<Product> result = CatalogService.GetProductWithCategoryAsync(productId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Product>));
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedProduct()
        {
            // Arrange
            int id = 1;

            ProductsConsumerStub
                .Setup(products => products.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidProduct));
            CategoriesConsumerStub
                .Setup(categories => categories.GetAsync(ExpectedValidProduct.CategoryId))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            Product result = CatalogService.GetProductWithCategoryAsync(id).Result;

            // Assert
            Assert.AreEqual(ExpectedValidProduct.Id, result.Id);
            Assert.AreEqual(ExpectedValidProduct.MongoId, result.MongoId);
            Assert.AreEqual(ExpectedValidProduct.Name, result.Name);
            Assert.AreEqual(ExpectedValidProduct.CategoryId, result.CategoryId);
            Assert.AreEqual(ExpectedValidProduct.Description, result.Description);
            Assert.AreEqual(ExpectedValidProduct.BasePrice, result.BasePrice);
            Assert.AreEqual(ExpectedValidProduct.Rating, result.Rating);
            Assert.AreEqual(ExpectedValidProduct.Specifications.First().Name, result.Specifications.First().Name);
            Assert.AreEqual(ExpectedValidProduct.Specifications.First().MaxSelectionAvailable, result.Specifications.First().MaxSelectionAvailable);
            Assert.AreEqual(ExpectedValidProduct.Specifications.First().Options.Count(), result.Specifications.First().Options.Count());
            Assert.AreEqual(ExpectedValidProduct.Specifications.First().Options.First(), result.Specifications.First().Options.First());
        }

        [TestMethod]
        public void ValidIdOfProductWithValidCategory_ReturnsProductWithValidCategory()
        {
            // Arrange
            int id = 1;

            ProductsConsumerStub
                .Setup(products => products.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidProduct));
            CategoriesConsumerStub
                .Setup(categories => categories.GetAsync(ExpectedValidProduct.CategoryId))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            Category result = CatalogService.GetProductWithCategoryAsync(id).Result.Category;

            // Assert
            Assert.AreEqual(result.Id, ExpectedValidCategory.Id);
            Assert.AreEqual(result.Name, ExpectedValidCategory.Name);
            Assert.AreEqual(result.ParentCategoryId, ExpectedValidCategory.ParentCategoryId);
        }

        [TestMethod]
        public void ValidIdOfProductWithInvalidCategory_ReturnsProductWithoutCategory()
        {
            // Arrange
            int id = 1;

            ProductsConsumerStub
                .Setup(products => products.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidProduct));
            CategoriesConsumerStub
                .Setup(categories => categories.GetAsync(ExpectedValidProduct.CategoryId))
                .Returns(Task.FromResult(ExpectedInvalidCategory));

            // Act
            Category result = CatalogService.GetProductWithCategoryAsync(id).Result.Category;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void InvalidId_ReturnsNull()
        {
            // Arrange
            int id = 99999;

            ProductsConsumerStub
                .Setup(products => products.GetAsync(id))
                .Returns(Task.FromResult(ExpectedInvalidProduct));

            // Act
            Product result = CatalogService.GetProductWithCategoryAsync(id).GetAwaiter().GetResult();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;

            // Act
            Task<Product> result = CatalogService.GetProductWithCategoryAsync(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        private Product ExpectedValidProduct
        {
            get
            {
                int id = 1;
                string name = "TestProduct";
                int categoryId = 3;
                string description = "TestDescription";
                decimal basePrice = 12m;
                float rating = 4.5f;
                string specName = "sampleSpecification";
                int maxSelectionAvailable = 1;
                List<string> specOptions = new List<string> { "opt1" };

                Product product = new Product
                {
                    Id = id,
                    Name = name,
                    CategoryId = categoryId,
                    Description = description,
                    BasePrice = basePrice,
                    Rating = rating,
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

        private Category ExpectedValidCategory
        {
            get
            {
                int id = 1;
                string name = "TestCategory";
                int parentId = 2;

                Category category = new Category
                {
                    Id = id,
                    Name = name,
                    ParentCategoryId = parentId
                };

                return category;
            }
        }

        private Category ExpectedInvalidCategory => null;

        private Product ExpectedInvalidProduct => null;
    }
}
