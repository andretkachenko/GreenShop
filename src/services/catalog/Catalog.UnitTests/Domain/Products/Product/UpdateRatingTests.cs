using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Target = GreenShop.Catalog.Api.Domain.Products.Product;

namespace GreenShop.UnitTests.Domain.Products.Product
{
    [TestClass]
    public class UpdateRatingTests
    {
        [TestMethod]
        public void PositiveRating_ChangesSuccessfully()
        {
            // Assign
            string name = "Name";
            int categoryId = 0;
            float rating = 1f;
            Target productMock = new Target(name, categoryId);

            // Act
            productMock.UpdateRating(rating);

            // Assert
            Assert.AreEqual(rating, productMock.Rating);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void NegativeRating_ThrowsArgumentException()
        {
            // Assign
            string name = "Name";
            int categoryId = 0;
            float rating = -1f;
            Target productMock = new Target(name, categoryId);

            // Act
            productMock.UpdateRating(rating);

            // Assert
            Assert.Fail();
        }
    }
}
