using Target = GreenShop.Web.Bff.Shopping.Helpers.RestSharpHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System.Linq;

namespace UnitTests.WebBffShopping.Helpers.RestSharpHelpers
{
    [TestClass]
    public class AssembleRestRequestTests
    {
        [TestMethod]
        public void ValidResource_ValidHttpMethod_ReturnsExpectedRestRequest()
        {
            // Align
            var validResource = "/api/categories";
            var validHttpMethod = Method.GET;

            // Act
            var result = Target.AssembleRestRequest(validResource, validHttpMethod);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RestRequest));
            Assert.AreEqual(validHttpMethod, result.Method);
            Assert.AreEqual(validResource, result.Resource);
        }

        [TestMethod]
        public void ValidResource_ValidHttpMethod_HasNoBody()
        {
            // Align
            var validResource = "/api/categories";
            var validHttpMethod = Method.GET;

            // Act
            var result = Target.AssembleRestRequest(validResource, validHttpMethod);

            // Assert
            Assert.IsFalse(result.Parameters.Any(x => x.Type == ParameterType.RequestBody));
        }

        [TestMethod]
        public void ValidResource_ValidHttpMethod_ValidJsonBody_ReturnsExpectedRestRequest()
        {
            // Align
            var validResource = "/api/categories";
            var validHttpMethod = Method.GET;
            var validJsonBody = "Sample string";

            // Act
            var result = Target.AssembleRestRequest(validResource, validHttpMethod, validJsonBody);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RestRequest));
            Assert.AreEqual(validHttpMethod, result.Method);
            Assert.AreEqual(validResource, result.Resource);
            Assert.AreEqual(validJsonBody, result.Parameters.FirstOrDefault(x => x.Type == ParameterType.RequestBody).Value);
        }
    }
}
