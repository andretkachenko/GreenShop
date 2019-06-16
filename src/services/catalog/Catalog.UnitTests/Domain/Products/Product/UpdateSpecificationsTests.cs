using GreenShop.Catalog.Api.Domain.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Target = GreenShop.Catalog.Api.Domain.Products.Product;

namespace GreenShop.UnitTests.Domain.Products.Product
{
    [TestClass]
    public class UpdateSpecificationsTests
    {
        [TestMethod]
        public void ValidSpecificationList_UpdatedSuccessfully()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            int maxSelect = 1;
            string specName = "Specification Name";
            string option1 = "First Option";
            string option2 = "Second Option";
            List<string> specOptions = new List<string>
            {
                option1,
                option2
            };
            var specList = new List<Specification>
            {
                new Specification(specName, maxSelect, specOptions)
            };

            // Act
            productMock.UpdateSpecifications(specList);

            // Assert
            Assert.AreEqual(specName, productMock.Specifications.First().Name);
            Assert.AreEqual(maxSelect, productMock.Specifications.First().MaxSelectionAvailable);
            Assert.AreEqual(option1, productMock.Specifications.First().Options.ToList()[0]);
            Assert.AreEqual(option2, productMock.Specifications.First().Options.ToList()[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullSpecificationList_ThrowsArgumentNullException()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            List<Specification> specList = null;

            // Act
            productMock.UpdateSpecifications(specList);

            // Assert
            Assert.Fail();
        }
    }
}
