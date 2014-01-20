using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace FluentRouting.Mvc.Test
{
    [TestClass]
    public abstract class HostConstraintTests
    {
        protected RouteCollection routes;

        [TestInitialize]
        public void TestInitialize()
        {
            routes = new RouteCollection();
        }

        [TestClass]
        public class TheMatchMethod : HostConstraintTests
        {
            [TestMethod]
            public void ShouldMatchOnIncomingRequest()
            {
                // Arrange
                var host = "localhost";
                var allowedHost = "localhost";

                // Act
                var result = TestHostConstraint(host, allowedHost, RouteDirection.IncomingRequest);

                // Assert
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ShouldMatchOnUrlGeneration()
            {
                // Arrange
                var host = "localhost";
                var allowedHost = "localhost";

                // Act
                var result = TestHostConstraint(host, allowedHost, RouteDirection.UrlGeneration);

                // Assert
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ShouldNotMatchOnIncomingRequest()
            {
                // Arrange
                var host = "remotehost";
                var allowedHost = "localhost";

                // Act
                var result = TestHostConstraint(host, allowedHost, RouteDirection.IncomingRequest);

                // Assert
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ShouldNotMatchOnUrlGeneration()
            {
                // Arrange
                var host = "remotehost";
                var allowedHost = "localhost";

                // Act
                var result = TestHostConstraint(host, allowedHost, RouteDirection.UrlGeneration);

                // Assert
                Assert.IsFalse(result);
            }

            private bool TestHostConstraint(string host, string allowedHost, RouteDirection routeDirection)
            {
                var constraint = new HostConstraint(allowedHost);

                var context = new Mock<HttpContextBase>();
                var request = new Mock<HttpRequestBase>();

                request.Setup(_ => _.Url)
                    .Returns(new Uri(string.Concat("http://", host)));

                context.Setup(_ => _.Request)
                    .Returns(request.Object);

                var route = new Route("", null) { Defaults = new RouteValueDictionary() };
                var values = new RouteValueDictionary() { { "host", host } };

                return constraint.Match(context.Object, route, "host", values, routeDirection);
            }
        }
    }
}