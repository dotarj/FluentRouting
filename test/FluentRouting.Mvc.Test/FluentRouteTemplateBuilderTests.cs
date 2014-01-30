// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net.Http;
using System.Web.Routing;

namespace FluentRouting.Mvc.Test
{
    [TestClass]
    public abstract class FluentRouteTemplateBuilderTests
    {
        protected RouteCollection routes;

        [TestInitialize]
        public void TestInitialize()
        {
            routes = new RouteCollection();
        }

        [TestClass]
        public class TheToMethod : FluentRouteTemplateBuilderTests
        {
            [TestMethod]
            public void ShouldMapMultipleMethods()
            {
                // Act
                routes.For<TestController>()
                    .CreateRoute("a").To(controller => controller.Index())
                    .CreateRoute("b").To(controller => controller.Index(1));

                // Assert
                var count = routes
                    .OfType<Route>()
                    .Count();

                Assert.AreEqual(2, count);
            }
        }
    }
}
