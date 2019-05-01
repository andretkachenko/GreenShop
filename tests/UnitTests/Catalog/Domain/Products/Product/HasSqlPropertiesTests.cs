using GreenShop.Catalog.Domain.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UnitTests.Wrappers;

namespace UnitTests.Catalog.Domain.Products.Product
{
    [TestClass]
    public class HasSqlPropertiesTests
    {
        [TestMethod]
        public void ProductWithCategoryId_ReturnTrue()
        {
            // Assign
            var validProduct = new ProductWrapper
            {
                WrapCategoryId = 1
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
            var validProduct = new ProductWrapper
            {
                WrapDescription = "Description"
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
            var validProduct = new ProductWrapper
            {
                WrapBasePrice = 1
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
            var validProduct = new ProductWrapper
            {
                WrapRating = 2
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
            var invalidProduct = new ProductWrapper
            {
                WrapSpecifications = new List<Specification>
                {
                    new Specification("sampleSpecName", 1, new List<string>{ "opt1", "opt2", "opt3" })
                }
            };

            // Act
            var result = invalidProduct.HasSqlProperties();

            // Assert
            Assert.IsFalse(result);
        }
    }
}
