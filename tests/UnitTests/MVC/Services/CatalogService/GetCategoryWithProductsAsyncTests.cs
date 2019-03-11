using Common.Models.Categories;
using Common.Models.DTO;
using Common.Models.Products;
using Common.Models.Specifications;
using FluentValidation;
using GreenShop.MVC.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Target = GreenShop.MVC.Services.CatalogService;

namespace UnitTests.MVC.Services.CatalogService
{
    [TestClass]
    public class GetCategoryWithProductsAsyncTests
    {
        private Mock<ICatalogConsumer> CatalogConsumerStub;
        private Target CatalogService;

        public GetCategoryWithProductsAsyncTests()
        {
            CatalogConsumerStub = new Mock<ICatalogConsumer>();
            CatalogService = new Target(CatalogConsumerStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedType()
        {
            // Arrange
            int id = 1;

            CatalogConsumerStub
                .Setup(catalog => catalog.GetCategoryWithProductsAsync(id))
                .Returns(Task.FromResult(ExpectedValidDto));

            // Act
            Task<CategoryProductsDTO> result = CatalogService.GetCategoryWithProductsAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<CategoryProductsDTO>));
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedCategory()
        {
            // Arrange
            int id = 1;

            CatalogConsumerStub
                .Setup(catalog => catalog.GetCategoryWithProductsAsync(id))
                .Returns(Task.FromResult(ExpectedValidDto));

            // Act
            Task<CategoryProductsDTO> result = CatalogService.GetCategoryWithProductsAsync(id);
            CategoryProductsDTO actualCategory = result.GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(actualCategory.Category.Id, ExpectedValidCategory.Id);
            Assert.AreEqual(actualCategory.Category.Name, ExpectedValidCategory.Name);
            Assert.AreEqual(actualCategory.Category.ParentCategoryId, ExpectedValidCategory.ParentCategoryId);
        }

        [TestMethod]
        public void ValidIdForCategoryWithRelatedProducts_ReturnsExpectedProductList()
        {
            // Arrange
            Product expectedProduct = ExpectedProductList.First();
            int id = 1;

            CatalogConsumerStub
                .Setup(catalog => catalog.GetCategoryWithProductsAsync(id))
                .Returns(Task.FromResult(ExpectedValidDto));

            // Act
            Task<CategoryProductsDTO> result = CatalogService.GetCategoryWithProductsAsync(id);
            CategoryProductsDTO dto = result.GetAwaiter().GetResult();
            Category actualCategory = dto.Category;
            List<Product> actualProducts = dto.Products;
            Product actualProduct = actualProducts.First();

            // Assert
            Assert.AreEqual(actualProducts.Count(), ExpectedProductList.Count());
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

        [TestMethod]
        public void ValidIdForCategoryWithoutRelatedProducts_ReturnsNullProductList()
        {
            // Arrange
            Product expectedProduct = ExpectedProductList.First();
            int id = 1;

            CatalogConsumerStub
                .Setup(catalog => catalog.GetCategoryWithProductsAsync(id))
                .Returns(Task.FromResult(new CategoryProductsDTO { Category = ExpectedValidCategory, Products = ExpectedInvalidProductList }));

            // Act
            Task<CategoryProductsDTO> result = CatalogService.GetCategoryWithProductsAsync(id);
            CategoryProductsDTO dto = result.GetAwaiter().GetResult();
            Category actualCategory = dto.Category;
            List<Product> actualProducts = dto.Products;

            // Assert
            Assert.IsFalse(actualProducts.Any());
        }

        [TestMethod]
        public void InvalidCategoryId_ReturnsNull()
        {
            // Arrange
            int id = 99999;

            CatalogConsumerStub
                .Setup(catalog => catalog.GetCategoryWithProductsAsync(id))
                .Returns(Task.FromResult(new CategoryProductsDTO { Category = ExpectedInvalidCategory, Products = ExpectedInvalidProductList.ToList() }));

            // Act
            Task<CategoryProductsDTO> result = CatalogService.GetCategoryWithProductsAsync(id);

            // Assert
            Assert.IsNull(result.Result.Category);
            Assert.IsFalse(result.Result.Products.Any());
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;

            // Act
            Task<CategoryProductsDTO> result = CatalogService.GetCategoryWithProductsAsync(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        private CategoryProductsDTO ExpectedValidDto => new CategoryProductsDTO
        {
            Category = ExpectedValidCategory,
            Products = ExpectedProductList.ToList()
        };

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

        private IEnumerable<Product> ExpectedProductList
        {
            get
            {
                int id = 1;
                string name = "TestProduct";
                string mongoId = "TestMongoId";
                int categoryId = 1;
                string description = "TestDescription";
                decimal basePrice = 12m;
                float rating = 4.5f;
                string specName = "sampleSpecification";
                int maxSelectionAvailable = 1;
                List<string> specOptions = new List<string> { "opt1" };

                List<Product> productsList = new List<Product>()
                    {
                        new Product
                        {
                            Id = id,
                            MongoId = mongoId,
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
                        }
                    };

                return productsList;
            }
        }

        private List<Product> ExpectedInvalidProductList => new List<Product>();
    }
}
