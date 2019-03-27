using GreenShop.Catalog.Extensions;
using Common.Models.Products;
using Common.Models.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTests.Catalog.Extensions.ProductExtensions
{
    [TestClass]
    public class HasSqlPropertiesTests
    {
        [TestMethod]
        public void ProductWithCategoryId_ReturnTrue()
        {
            // Assign
            var validProduct = new Product
            {
                CategoryId = 1
            };

            // Act
            var result = validProduct.HasSqlProperties();

            // Assert
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void ProductWithDescription_ReturnTrue()
        {
            // Assign
            var validProduct = new Product
            {
                Description = "Description"
            };

            // Act
            var result = validProduct.HasSqlProperties();

            // Assert
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void ProductWithBasePrice_ReturnTrue()
        {
            // Assign
            var validProduct = new Product
            {
                BasePrice = 1
            };

            // Act
            var result = validProduct.HasSqlProperties();

            // Assert
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void ProductWithRating_ReturnTrue()
        {
            // Assign
            var validProduct = new Product
            {
                Rating = 2
            };

            // Act
            var result = validProduct.HasSqlProperties();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProductWithoutSqlProperties_ReturnFalse()
        {
            // Assign
            var invalidProduct = new Product
            {
                Specifications = new List<Specification>
                {
                    new Specification
                    {
                        Name = "sampleSpecName",
                        MaxSelectionAvailable = 1,
                        Options = new List<string>{ "opt1", "opt2", "opt3"}
                    }
                }
            };

            // Act
            var result = invalidProduct.HasSqlProperties();

            // Assert
            Assert.IsFalse(result);
        }
    }
}
