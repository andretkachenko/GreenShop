using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Target = GreenShop.Catalog.Api.Domain.Products.Product;

namespace GreenShop.UnitTests.Domain.Products.Product
{
    [TestClass]
    public class ChangeCategoryTests
    {
        [TestMethod]
        public void ValidCategoryId_SuccessfullyUpdated()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            int expectedCategoryId = 2;

            // Act
            productMock.ChangeCategory(expectedCategoryId);

            // Assert
            Assert.AreEqual(expectedCategoryId, productMock.CategoryId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ZeroCategoryId_ThrowsArgumentException()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            int invalidCategoryId = 0;

            // Act
            productMock.ChangeCategory(invalidCategoryId);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NegativeCategoryId_ThrowsArgumentException()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            int invalidCategoryId = -1;

            // Act
            productMock.ChangeCategory(invalidCategoryId);

            // Assert
            Assert.Fail();
        }
    }
}
