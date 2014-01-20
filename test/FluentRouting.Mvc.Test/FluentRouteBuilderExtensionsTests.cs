// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Routing;

namespace FluentRouting.Mvc.Test
{
    [TestClass]
    public abstract class FluentRouteBuilderExtensionsTests
    {
        protected RouteCollection routes;

        [TestInitialize]
        public void TestInitialize()
        {
            routes = new RouteCollection();
        }

        [TestClass]
        public class TheWithActionNameMethod : FluentRouteBuilderExtensionsTests
        {
            [TestMethod]
            public void ShouldModifyTheActionName()
            {
                // Arrange
                var actionName = "testAction";

                // Act
                routes.Map<TestController>(controller => controller.Index(), map => map
                    .ToRoute("", route => route
                        .WithActionName(actionName)));

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
