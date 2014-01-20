// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net.Http;
using System.Web.Routing;

namespace FluentRouting.Mvc.Test
{
    [TestClass]
    public abstract class FluentRouteGroupBuilderExtensionsTests
    {
        protected RouteCollection routes;

        [TestInitialize]
        public void TestInitialize()
        {
            routes = new RouteCollection();
        }

        [TestClass]
        public class TheMapMethod : FluentRouteGroupBuilderExtensionsTests
        {
            [TestMethod]
            public void ShouldMapMultipleMethods()
            {
                // Act
                routes.Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute("a"))
                    .Map<TestController>(controller => controller.Index(1), map => map
                        .ToRoute("b"));

                // Assert
                var count = routes
                    .OfType<Route>()
                    .Count();

                Assert.AreEqual(2, count);
            }
        }
    }
}
