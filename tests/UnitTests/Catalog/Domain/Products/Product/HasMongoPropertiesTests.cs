using GreenShop.Catalog.Domain.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UnitTests.Wrappers;

namespace UnitTests.Catalog.Domain.Products.Product
{
    [TestClass]
    public class HasMongoPropertiesTests
    {
        [TestMethod]
        public void ProductWithSpecifications_ReturnTrue()
        {
            // Assign
            var validProduct = new ProductWrapper
            {
                WrapSpecifications = new List<Specification>
                {
                    new Specification("sampleSpecName", 1, new List<string>{ "opt1", "opt2", "opt3"})
                }
            };

            // Act
            var result = validProduct.HasMongoProperties();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProductWithoutSpecifications_ReturnFalse()
        {
            // Assign
            var invalidProduct = new ProductWrapper
            {
                WrapId = 1,
                WrapMongoId = "placeholder"
            };

            // Act
            var result = invalidProduct.HasMongoProperties();

            // Assert
            Assert.IsFalse(result);
        }
    }
}
