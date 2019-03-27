﻿using Common.Models.Categories;
using Common.Models.Products;
using Common.Models.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenShop.Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = GreenShop.Web.Bff.Shopping.Services.CatalogService;

namespace UnitTests.WebBffShopping.Services.CatalogService
{
    [TestClass]
    public class GetAllProductsAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public GetAllProductsAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void ReturnsExpectedType()
        {
            // Arrange
            ProductsConsumerStub
                .Setup(products => products.GetAllAsync())
                .Returns(Task.FromResult(ExpectedProductList));

            // Act
            Task<IEnumerable<Product>> result = CatalogService.GetAllProductsAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<IEnumerable<Product>>));
        }

        [TestMethod]
        public void ReturnsExpectedProduct()
        {
            // Arrange
            Product expectedProduct = ExpectedProductList.First();
            ProductsConsumerStub
                .Setup(products => products.GetAllAsync())
                .Returns(Task.FromResult(ExpectedProductList));

            // Act
            Task<IEnumerable<Product>> result = CatalogService.GetAllProductsAsync();
            Product actualProduct = result.Result.First();

            // Assert
            Assert.AreEqual(result.Result.Count(), ExpectedProductList.Count());
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

        private IEnumerable<Product> ExpectedProductList
        {
            get
            {
                int id = 1;
                string name = "TestProduct";
                string mongoId = "TestMongoId";
                int parentId = 3;
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
                            CategoryId = parentId,
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
    }
}