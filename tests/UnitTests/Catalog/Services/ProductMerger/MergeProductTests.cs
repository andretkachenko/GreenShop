using Target = Catalog.Services.Products.ProductMerger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Common.Configuration.SQL;
using System;
using Common.Models.Products;
using System.Collections.Generic;
using Common.Models.Specifications;
using System.Linq;

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
            var expectedId = 1;
            var expectedMongoId = "sampleMongoId";
            var expectedDescription = "sampleDesription";
            var expectedSpecName = "sampleSpecification";
            var expectedMaxSelectionAvailable = 1;
            var expectedSpecOptions = new List<string> { "opt1", "opt2", "opt3" };
            var validSqlProduct = new Product
            {
                Id = expectedId,
                MongoId = expectedMongoId,
                Description = expectedDescription
            };
            var validMongoProduct = new Product
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
            var mergedProduct = ProductMerger.MergeProduct(validSqlProduct, validMongoProduct);

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
            var expectedMongoId = "sampleMongoId";
            var expectedSpecName = "sampleSpecification";
            var expectedMaxSelectionAvailable = 1;
            var expectedSpecOptions = new List<string> { "opt1", "opt2", "opt3" };
            var validMongoProduct = new Product
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
            var expectedId = 1;
            var firstMongoId = "firstMongoId";
            var secondMongoId = "secondMongoId";
            var expectedDescription = "sampleDesription";
            var expectedSpecName = "sampleSpecification";
            var expectedMaxSelectionAvailable = 1;
            var expectedSpecOptions = new List<string> { "opt1", "opt2", "opt3" };
            var validSqlProduct = new Product
            {
                Id = expectedId,
                MongoId = firstMongoId,
                Description = expectedDescription
            };
            var validMongoProduct = new Product
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
            var expectedId = 1;
            var expectedMongoId = "sampleMongoId";
            var expectedDescription = "sampleDesription";
            var expectedSpecOptions = new List<string> { "opt1", "opt2", "opt3" };
            var validSqlProduct = new Product
            {
                Id = expectedId,
                MongoId = expectedMongoId,
                Description = expectedDescription
            };
            Product invalidMongoProduct = null;

            // Act
            var actualProduct = ProductMerger.MergeProduct(validSqlProduct, invalidMongoProduct);

            // Assert
            Assert.AreEqual(validSqlProduct, actualProduct);
        }
    }
}
