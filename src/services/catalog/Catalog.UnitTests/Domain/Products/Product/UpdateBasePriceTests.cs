using Microsoft.VisualStudio.TestTools.UnitTesting;
using Target = GreenShop.Catalog.Api.Domain.Products.Product;

namespace GreenShop.UnitTests.Domain.Products.Product
{
    [TestClass]
    public class UpdateBasePriceTests
    {
        [TestMethod]
        public void ValidPrice_SuccessfullyUpdated()
        {
            // Assign
            decimal price = 12m;
            string initName = "Init Name";
            int categoryId = 1;
            Target productMock = new Target(initName, categoryId);

            // Act
            productMock.UpdateBasePrice(price);

            // Assert
            Assert.AreEqual(price, productMock.BasePrice);
        }
    }
}
