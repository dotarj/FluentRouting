// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Routing;

namespace FluentRouting.Mvc.Test
{
    [TestClass]
    public abstract class FluentRouteBuilderTests
    {
        protected RouteCollection routes;

        [TestInitialize]
        public void TestInitialize()
        {
            routes = new RouteCollection();
        }

        [TestClass]
        public class TheWithActionNameMethod : FluentRouteBuilderTests
        {
            [TestMethod]
            public void ShouldModifyTheActionName()
            {
                // Arrange
                var actionName = "testAction";

                // Act
                routes.For<TestController>().CreateRoute("").To(controller => controller.Index())
                    .WithActionName(actionName);

                // Assert
                var fluentRoute = routes
                    .OfType<Route>()
                    .FirstOrDefault();

                Assert.IsNotNull(fluentRoute);
                Assert.AreEqual(actionName, fluentRoute.Defaults["action"]);
            }
        }
    }
}
