using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Models.Products;
using GreenShop.Catalog.Models.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Target = GreenShop.Catalog.Services.Products.ProductMerger;

namespace UnitTests.Catalog.Services.ProductMerger
{
    [TestClass]
    public class MergeProductTests
    {
        private Mock<ISqlContext> SqlContextStub;
        private Target ProductMerger;

        public MergeProductTests()
        {
            SqlContextStub = new Mock<ISqlContext>();
            ProductMerger = new Target(SqlContextStub.Object);
        }

        [TestMethod]
        public void ValidSqlProduct_ValidMongoProduct_ReturnValidMergedProduct()
        {
            // Assign
            int expectedId = 1;
            string expectedMongoId = "sampleMongoId";
            string expectedDescription = "sampleDesription";
            string expectedSpecName = "sampleSpecification";
            int expectedMaxSelectionAvailable = 1;
            List<string> expectedSpecOptions = new List<string> { "opt1", "opt2", "opt3" };
            Product validSqlProduct = new Product
            {
                Id = expectedId,
                MongoId = expectedMongoId,
                Description = expectedDescription
            };
            Product validMongoProduct = new Product
            {
                MongoId = expectedMongoId,
                Specifications = new List<Specification>
                {
                    new Specification
                    {
                        Name = expectedSpecName,
                        MaxSelectionAvailable = expectedMaxSelectionAvailable,
                        Options = expectedSpecOptions
                    }
                }
            };

            // Act
            Product mergedProduct = ProductMerger.MergeProduct(validSqlProduct, validMongoProduct);

            // Assert
            Assert.AreEqual(expectedId, mergedProduct.Id);
            Assert.AreEqual(expectedMongoId, mergedProduct.MongoId);
            Assert.AreEqual(expectedDescription, mergedProduct.Description);
            Assert.IsNotNull(mergedProduct.Specifications);
            Assert.IsTrue(mergedProduct.Specifications.Count() == 1);
            Assert.AreEqual(expectedSpecName, mergedProduct.Specifications.First().Name);
            Assert.AreEqual(expectedMaxSelectionAvailable, mergedProduct.Specifications.First().MaxSelectionAvailable);
            Assert.IsTrue(expectedSpecOptions.SequenceEqual(mergedProduct.Specifications.First().Options));
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void NullSqlProduct_ValidMongoProduct_ThrowArgumentNullException()
        {
            // Assign
            Product invalidSqlProduct = null;
            string expectedMongoId = "sampleMongoId";
            string expectedSpecName = "sampleSpecification";
            int expectedMaxSelectionAvailable = 1;
            List<string> expectedSpecOptions = new List<string> { "opt1", "opt2", "opt3" };
            Product validMongoProduct = new Product
            {
                MongoId = expectedMongoId,
                Specifications = new List<Specification>
                {
                    new Specification
                    {
                        Name = expectedSpecName,
                        MaxSelectionAvailable = expectedMaxSelectionAvailable,
                        Options = expectedSpecOptions
                    }
                }
            };

            // Act
            ProductMerger.MergeProduct(invalidSqlProduct, validMongoProduct);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ValidProductsWithDifferentMongoIds_ThrowArgumentException()
        {
            // Assign
            int expectedId = 1;
            string firstMongoId = "firstMongoId";
            string secondMongoId = "secondMongoId";
            string expectedDescription = "sampleDesription";
            string expectedSpecName = "sampleSpecification";
            int expectedMaxSelectionAvailable = 1;
            List<string> expectedSpecOptions = new List<string> { "opt1", "opt2", "opt3" };
            Product validSqlProduct = new Product
            {
                Id = expectedId,
                MongoId = firstMongoId,
                Description = expectedDescription
            };
            Product validMongoProduct = new Product
            {
                MongoId = secondMongoId,
                Specifications = new List<Specification>
                {
                    new Specification
                    {
                        Name = expectedSpecName,
                        MaxSelectionAvailable = expectedMaxSelectionAvailable,
                        Options = expectedSpecOptions
                    }
                }
            };

            // Act
            ProductMerger.MergeProduct(validSqlProduct, validMongoProduct);
        }

        [TestMethod]
        public void ValidSqlProduct_NullMongoProduct_ReturnSameSqlProduct()
        {
            // Assign
            int expectedId = 1;
            string expectedMongoId = "sampleMongoId";
            string expectedDescription = "sampleDesription";
            List<string> expectedSpecOptions = new List<string> { "opt1", "opt2", "opt3" };
            Product validSqlProduct = new Product
            {
                Id = expectedId,
                MongoId = expectedMongoId,
                Description = expectedDescription
            };
            Product invalidMongoProduct = null;

            // Act
            Product actualProduct = ProductMerger.MergeProduct(validSqlProduct, invalidMongoProduct);

            // Assert
            Assert.AreEqual(validSqlProduct, actualProduct);
        }
    }
}
