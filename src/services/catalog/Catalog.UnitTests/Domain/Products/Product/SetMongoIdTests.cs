using Microsoft.VisualStudio.TestTools.UnitTesting;
using Target = GreenShop.Catalog.Api.Domain.Products.Product;

namespace GreenShop.UnitTests.Domain.Products.Product
{
    [TestClass]
    public class SetMongoIdTests
    {
        [TestMethod]
        public void ValidMongoId_UpdatedSuccessfully()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            string mongoId = "507f191e810c19729de860ea";

            // Act
            productMock.SetMongoId(mongoId);

            // Assert
            Assert.AreEqual(mongoId, productMock.MongoId);
        }
    }
}
