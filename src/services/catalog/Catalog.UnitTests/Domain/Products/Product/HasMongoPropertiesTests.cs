using GreenShop.Catalog.Api.Domain.Products;
using GreenShop.Catalog.UnitTests.Wrappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GreenShop.Catalog.UnitTests.Domain.Products.Product
{
    [TestClass]
    public class HasMongoPropertiesTests
    {
        [TestMethod]
        public void ProductWithSpecifications_ReturnTrue()
        {
            // Assign
            ProductWrapper validProduct = new ProductWrapper
            {
                WrapSpecifications = new List<Specification>
                {
                    new Specification("sampleSpecName", 1, new List<string>{ "opt1", "opt2", "opt3"})
                }
            };

            // Act
            bool result = validProduct.HasMongoProperties();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProductWithoutSpecifications_ReturnFalse()
        {
            // Assign
            ProductWrapper invalidProduct = new ProductWrapper
            {
                WrapId = 1,
                WrapMongoId = "placeholder"
            };

            // Act
            bool result = invalidProduct.HasMongoProperties();

            // Assert
            Assert.IsFalse(result);
        }
    }
}
