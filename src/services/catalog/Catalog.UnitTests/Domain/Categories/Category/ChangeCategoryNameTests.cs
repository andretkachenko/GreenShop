using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Target = GreenShop.Catalog.Api.Domain.Categories.Category;

namespace GreenShop.UnitTests.Domain.Categories.Category
{
    [TestClass]
    public class ChangeCategoryNameTests
    {
        [TestMethod]
        public void ValidName_NameChanged()
        {
            // Assign
            string initName = "InitName";
            string newName = "NewName";
            Target categoryMock = new Target(initName);

            // Act
            categoryMock.ChangeCategoryName(newName);

            // Assert
            Assert.AreEqual(newName, categoryMock.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NameNull_ThrowsArgumentNullException()
        {
            // Assign
            string initName = "InitName";
            string newName = null;
            Target categoryMock = new Target(initName);

            // Act
            categoryMock.ChangeCategoryName(newName);

            // Assert
            Assert.Fail();
        }
    }
}
