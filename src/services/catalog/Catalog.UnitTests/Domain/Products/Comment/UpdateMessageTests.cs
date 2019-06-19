using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Target = GreenShop.Catalog.Api.Domain.Products.Comment;

namespace GreenShop.UnitTests.Domain.Products.Comment
{
    [TestClass]
    public class UpdateMessageTests
    {
        [TestMethod]
        public void ValidMessage_UpdatedSuccefully()
        {
            // Assign
            int authorId = 1;
            int productId = 1;
            string initMessage = "Init message";
            Target commentMock = new Target(authorId, initMessage, productId);
            string expectedMessage = "New message";

            // Act
            commentMock.UpdateMessage(expectedMessage);

            // Assert
            Assert.AreEqual(expectedMessage, commentMock.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyMessage_ThrowsArgumentException()
        {
            // Assign
            int authorId = 1;
            int productId = 1;
            string initMessage = "Init message";
            Target commentMock = new Target(authorId, initMessage, productId);
            string expectedMessage = string.Empty;

            // Act
            commentMock.UpdateMessage(expectedMessage);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullMessage_ThrowsArgumentException()
        {
            // Assign
            int authorId = 1;
            int productId = 1;
            string initMessage = "Init message";
            Target commentMock = new Target(authorId, initMessage, productId);
            string expectedMessage = null;

            // Act
            commentMock.UpdateMessage(expectedMessage);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhitespaceMessage_ThrowsArgumentException()
        {
            // Assign
            int authorId = 1;
            int productId = 1;
            string initMessage = "Init message";
            Target commentMock = new Target(authorId, initMessage, productId);
            string expectedMessage = " ";

            // Act
            commentMock.UpdateMessage(expectedMessage);

            // Assert
            Assert.Fail();
        }
    }
}
