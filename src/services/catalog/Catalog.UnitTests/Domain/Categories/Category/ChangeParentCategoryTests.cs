using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Target = GreenShop.Catalog.Api.Domain.Categories.Category;

namespace GreenShop.UnitTests.Domain.Categories.Category
{
    [TestClass]
    public class ChangeParentCategoryTests
    {
        [TestMethod]
        public void ValidId_ParentCategoryIdChanged()
        {
            // Assign
            string name = "Name";
            int initParentCategory = 1;
            int newParentCategory = 2;
            Target categoryMock = new Target(name, initParentCategory);

            // Act
            categoryMock.ChangeParentCategory(newParentCategory);

            // Assert
            Assert.AreEqual(newParentCategory, categoryMock.ParentCategoryId);
        }
    }
}
