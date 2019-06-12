using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Target = GreenShop.Catalog.Api.Domain.Products.Product;

namespace GreenShop.UnitTests.Domain.Products.Product
{
    [TestClass]
    public class UpdateDescriptionTests
    {
        [TestMethod]
        public void ValidDescription_SuccessfullyChanged()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            string newDescription = "New Description";

            // Act
            productMock.UpdateDescription(newDescription);

            // Assert
            Assert.AreEqual(newDescription, productMock.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullDescription_ThrowsArgumentNulException()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            string newDescription = null;

            // Act
            productMock.UpdateDescription(newDescription);


            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmptyDescription_ThrowsArgumentNulException()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            string newDescription = "";

            // Act
            productMock.UpdateDescription(newDescription);


            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhitespaceDescription_ThrowsArgumentNulException()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            string newDescription = " ";

            // Act
            productMock.UpdateDescription(newDescription);


            // Assert
            Assert.Fail();
        }
    }
}
