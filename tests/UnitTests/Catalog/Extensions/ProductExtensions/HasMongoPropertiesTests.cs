using GreenShop.Catalog.Models.Products;
using GreenShop.Catalog.Models.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTests.Catalog.Extensions.ProductExtensions
{
    [TestClass]
    public class HasMongoPropertiesTests
    {
        [TestMethod]
        public void ProductWithSpecifications_ReturnTrue()
        {
            // Assign
            var validProduct = new Product
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
            var result = validProduct.HasMongoProperties();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProductWithoutSpecifications_ReturnFalse()
        {
            // Assign
            var invalidProduct = new Product
            {
                Id = 1,
                MongoId = "placeholder"
            };

            // Act
            var result = invalidProduct.HasMongoProperties();

            // Assert
            Assert.IsFalse(result);
        }
    }
}
