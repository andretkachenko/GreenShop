using GreenShop.Catalog.Api.Domain.Products;
using GreenShop.Catalog.UnitTests.Wrappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GreenShop.Catalog.UnitTests.Domain.Products.Product
{
    [TestClass]
    public class HasSqlPropertiesTests
    {
        [TestMethod]
        public void ProductWithCategoryId_ReturnTrue()
        {
            // Assign
            ProductWrapper validProduct = new ProductWrapper
            {
                WrapCategoryId = 1
            };

            // Act
            bool result = validProduct.HasSqlProperties();

            // Assert
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void ProductWithDescription_ReturnTrue()
        {
            // Assign
            ProductWrapper validProduct = new ProductWrapper
            {
                WrapDescription = "Description"
            };

            // Act
            bool result = validProduct.HasSqlProperties();

            // Assert
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void ProductWithBasePrice_ReturnTrue()
        {
            // Assign
            ProductWrapper validProduct = new ProductWrapper
            {
                WrapBasePrice = 1
            };

            // Act
            bool result = validProduct.HasSqlProperties();

            // Assert
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void ProductWithRating_ReturnTrue()
        {
            // Assign
            ProductWrapper validProduct = new ProductWrapper
            {
                WrapRating = 2
            };

            // Act
            bool result = validProduct.HasSqlProperties();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProductWithoutSqlProperties_ReturnFalse()
        {
            // Assign
            ProductWrapper invalidProduct = new ProductWrapper
            {
                WrapSpecifications = new List<Specification>
                {
                    new Specification("sampleSpecName", 1, new List<string>{ "opt1", "opt2", "opt3" })
                }
            };

            // Act
            bool result = invalidProduct.HasSqlProperties();

            // Assert
            Assert.IsFalse(result);
        }
    }
}
