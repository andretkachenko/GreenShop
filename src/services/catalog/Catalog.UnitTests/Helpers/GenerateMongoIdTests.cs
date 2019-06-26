using GreenShop.Catalog.Api.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace GreenShop.UnitTests.Helpers
{
    [TestClass]
    public class GenerateMongoIdTests
    {
        [TestMethod]
        public void ReturnsValidBsonId()
        {
            // Assign
            Regex regex = new Regex(@"^[0-9a-fA-F]{24}$");

            // Act
            string actualId = MongoHelper.GenerateMongoId();

            // Assert
            bool match = regex.IsMatch(actualId);
            Assert.IsTrue(match);
        }
    }
}
