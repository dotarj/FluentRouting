// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net.Http;
using System.Web.Routing;

namespace FluentRouting.Mvc.Test
{
    [TestClass]
    public abstract class FluentRouteConstraintBuilderExtensionsTests
    {
        protected RouteCollection routes;

        [TestInitialize]
        public void TestInitialize()
        {
            routes = new RouteCollection();
        }

        [TestClass]
        public class TheCustomMethod : FluentRouteConstraintBuilderExtensionsTests
        {
            [TestMethod]
            public void ShouldAddAContraint()
            {
                // Act
                routes.Map<TestController>(controller => controller.Index(), map => map
                    .ToRoute("", route => route
                        .WithConstraints(constraints => constraints
                            .Custom("host", new HostConstraint("domain-a.com")))));

                // Assert
                var fluentRoute = routes
                    .OfType<Route>()
                    .FirstOrDefault();

                Assert.IsNotNull(fluentRoute);

                var constraint = fluentRoute.Constraints
                    .Values
                    .OfType<HostConstraint>()
                    .FirstOrDefault();

                Assert.IsNotNull(constraint);
            }
        }

        [TestClass]
        public class TheHostsMethod : FluentRouteConstraintBuilderExtensionsTests
        {
            [TestMethod]
            public void ShouldAddAHostContraintForLocalhost()
            {
                // Arrange
                var host = "localhost";

                // Act
                routes.Map<TestController>(controller => controller.Index(), map => map
                    .ToRoute("", route => route
                        .WithConstraints(constraints => constraints
                            .Host(host))));

                // Assert
                var fluentRoute = routes
                    .OfType<Route>()
                    .FirstOrDefault();

                Assert.IsNotNull(fluentRoute);

                var constraint = fluentRoute.Constraints
                    .Values
                    .OfType<HostConstraint>()
                    .FirstOrDefault();

                Assert.IsNotNull(constraint);
                Assert.AreEqual(host, constraint.AllowedHosts.FirstOrDefault(), true);
            }

            [TestMethod]
            public void ShouldAddAHostContraintForLocalhostAndRemotehost()
            {
                // Arrange
                var host = new[] { "localhost", "remotehost" };

                // Act
                routes.Map<TestController>(controller => controller.Index(), map => map
                    .ToRoute("", route => route
                        .WithConstraints(constraints => constraints
                            .Hosts(host))));

                // Assert
                var fluentRoute = routes
                    .OfType<Route>()
                    .FirstOrDefault();

                Assert.IsNotNull(fluentRoute);

                var constraint = fluentRoute.Constraints
                    .Values
                    .OfType<HostConstraint>()
                    .FirstOrDefault();

                Assert.IsNotNull(constraint);
                Assert.IsTrue(Enumerable.SequenceEqual(host, constraint.AllowedHosts));
            }
        }

        [TestClass]
        public class TheHttpMethodsMethod : FluentRouteConstraintBuilderExtensionsTests
        {
            [TestMethod]
            public void ShouldAddAHttpMethodContraintForHttpMethodGet()
            {
                // Arrange
                var httpMethod = HttpMethod.Get;

                // Act
                routes.Map<TestController>(controller => controller.Index(), map => map
                    .ToRoute("", route => route
                        .WithConstraints(constraints => constraints
                            .HttpMethod(httpMethod))));

                // Assert
                var fluentRoute = routes
                    .OfType<Route>()
                    .FirstOrDefault();

                Assert.IsNotNull(fluentRoute);

                var constraint = fluentRoute.Constraints
                    .Values
                    .OfType<HttpMethodConstraint>()
                    .FirstOrDefault();

                Assert.IsNotNull(constraint);
                Assert.AreEqual(httpMethod.ToString(), constraint.AllowedMethods.FirstOrDefault(), true);
            }

            [TestMethod]
            public void ShouldAddAHttpMethodContraintForHttpMethodGetAndHttpMethodPost()
            {
                // Arrange
                var httpMethods = new[] { HttpMethod.Get, HttpMethod.Post };

                // Act
                routes.Map<TestController>(controller => controller.Index(), map => map
                    .ToRoute("", route => route
                        .WithConstraints(constraints => constraints
                            .HttpMethods(httpMethods))));

                // Assert
                var fluentRoute = routes
                    .OfType<Route>()
                    .FirstOrDefault();

                Assert.IsNotNull(fluentRoute);

                var constraint = fluentRoute.Constraints
                    .Values
                    .OfType<HttpMethodConstraint>()
                    .FirstOrDefault();

                Assert.IsNotNull(constraint);
                Assert.IsTrue(Enumerable.SequenceEqual(httpMethods.Select(httpMethod => httpMethod.ToString()), constraint.AllowedMethods));
            }

            [TestMethod]
            public void ShouldAddAHttpMethodContraintForGet()
            {
                // Arrange
                var httpMethod = "GET";

                // Act
                routes.Map<TestController>(controller => controller.Index(), map => map
                    .ToRoute("", route => route
                        .WithConstraints(constraints => constraints
                            .HttpMethod(httpMethod))));

                // Assert
                var fluentRoute = routes
                    .OfType<Route>()
                    .FirstOrDefault();

                Assert.IsNotNull(fluentRoute);

                var constraint = fluentRoute.Constraints
                    .Values
                    .OfType<HttpMethodConstraint>()
                    .FirstOrDefault();

                Assert.IsNotNull(constraint);
                Assert.AreEqual(httpMethod, constraint.AllowedMethods.FirstOrDefault(), true);
            }

            [TestMethod]
            public void ShouldAddAHttpMethodContraintForGetAndPost()
            {
                // Arrange
                var httpMethods = new[] { "GET", "POST" };

                // Act
                routes.Map<TestController>(controller => controller.Index(), map => map
                    .ToRoute("", route => route
                        .WithConstraints(constraints => constraints
                            .HttpMethods(httpMethods))));

                // Assert
                var fluentRoute = routes
                    .OfType<Route>()
                    .FirstOrDefault();

                Assert.IsNotNull(fluentRoute);

                var constraint = fluentRoute.Constraints
                    .Values
                    .OfType<HttpMethodConstraint>()
                    .FirstOrDefault();

                Assert.IsNotNull(constraint);
                Assert.IsTrue(Enumerable.SequenceEqual(httpMethods, constraint.AllowedMethods));
            }
        }
    }
}
