// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net.Http;
using System.Web.Routing;

namespace FluentRouting.Mvc.Test
{
    [TestClass]
    public abstract class FluentRouteGroupConstraintBuilderExtensionsTests
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
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute(""))
                    .WithConstraints(constraints => constraints
                        .Custom("host", new HostConstraint("localhost"))));

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

            [TestMethod]
            public void ShouldNotOverrideAnExistingHostContraint()
            {
                // Arrange
                var routeHost = "localhost";
                var routeGroupHost = "localhost";

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute("", route => route
                            .WithConstraints(constraints => constraints
                                .Custom("host", new HostConstraint(routeHost)))))
                    .WithConstraints(constraints => constraints
                        .Custom("host", new HostConstraint(routeGroupHost))));

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
                Assert.AreEqual(routeHost, constraint.AllowedHosts.FirstOrDefault(), true);
            }
        }

        [TestClass]
        public class TheHostMethod : FluentRouteConstraintBuilderExtensionsTests
        {
            [TestMethod]
            public void ShouldAddAHostContraintForLocalhost()
            {
                // Arrange
                var host = "localhost";

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute(""))
                    .WithConstraints(constraints => constraints
                        .Host(host)));

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
            public void ShouldNotOverrideAnExistingHostContraint()
            {
                // Arrange
                var routeHost = "localhost";
                var routeGroupHost = "localhost";

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute("", route => route
                            .WithConstraints(constraints => constraints
                                .Host(routeHost))))
                    .WithConstraints(constraints => constraints
                        .Host(routeGroupHost)));

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
                Assert.AreEqual(routeHost, constraint.AllowedHosts.FirstOrDefault(), true);
            }
        }

        [TestClass]
        public class TheHostsMethod : FluentRouteConstraintBuilderExtensionsTests
        {
            [TestMethod]
            public void ShouldAddAHostContraintForLocalhostAndRemotehost()
            {
                // Arrange
                var host = new[] { "localhost", "remotehost" };

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute(""))
                    .WithConstraints(constraints => constraints
                        .Hosts(host)));

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

            [TestMethod]
            public void ShouldNotOverrideAnExistingHostContraint()
            {
                // Arrange
                var routeHost = new[] { "localhost", "remotehost" };
                var routeGroupHost = new[] { "localhost" };

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(1), map => map
                        .ToRoute("", route => route
                            .WithConstraints(constraints => constraints
                                .Hosts(routeHost))))
                    .WithConstraints(constraints => constraints
                        .Hosts(routeGroupHost)));

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
                Assert.IsTrue(Enumerable.SequenceEqual(routeHost, constraint.AllowedHosts));
            }
        }

        [TestClass]
        public class TheHttpMethodMethod : FluentRouteConstraintBuilderExtensionsTests
        {
            [TestMethod]
            public void ShouldAddAHttpMethodContraintForHttpMethodGet()
            {
                // Arrange
                var httpMethod = HttpMethod.Get;

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(new TestModel()), map => map
                        .ToRoute(""))
                    .WithConstraints(constraints => constraints
                        .HttpMethod(httpMethod)));

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
            public void ShouldAddAHttpMethodContraintForGet()
            {
                // Arrange
                var httpMethod = "GET";

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute(""))
                    .WithConstraints(constraints => constraints
                        .HttpMethod(httpMethod)));

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
            public void ShouldNotOverrideAnExistingHttpMethodContraint1()
            {
                // Arrange
                var routeHttpMethod = "GET";
                var routeGroupHttpMethod = "POST";

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute("", route => route
                            .WithConstraints(constraints => constraints
                                .HttpMethod(routeHttpMethod))))
                    .WithConstraints(constraints => constraints
                        .HttpMethod(routeGroupHttpMethod)));

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
                Assert.AreEqual(routeHttpMethod, constraint.AllowedMethods.FirstOrDefault(), true);
            }

            [TestMethod]
            public void ShouldNotOverrideAnExistingHttpMethodContraint2()
            {
                // Arrange
                var routeHttpMethod = HttpMethod.Get;
                var routeGroupHttpMethod = HttpMethod.Post;

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute("", route => route
                            .WithConstraints(constraints => constraints
                                .HttpMethod(routeHttpMethod))))
                    .WithConstraints(constraints => constraints
                        .HttpMethod(routeGroupHttpMethod)));

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
                Assert.AreEqual(routeHttpMethod.ToString(), constraint.AllowedMethods.FirstOrDefault(), true);
            }
        }

        [TestClass]
        public class TheHttpMethodsMethod : FluentRouteConstraintBuilderExtensionsTests
        {
            [TestMethod]
            public void ShouldAddAHttpMethodContraintForHttpMethodGetAndHttpMethodPost()
            {
                // Arrange
                var httpMethods = new[] { HttpMethod.Get, HttpMethod.Post };

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute(""))
                    .WithConstraints(constraints => constraints
                        .HttpMethods(httpMethods)));

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
            public void ShouldAddAHttpMethodContraintForGetAndPost()
            {
                // Arrange
                var httpMethods = new[] { "GET", "POST" };

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute(""))
                    .WithConstraints(constraints => constraints
                        .HttpMethods(httpMethods)));

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

            [TestMethod]
            public void ShouldNotOverrideAnExistingHttpMethodContraint1()
            {
                // Arrange
                var routeHttpMethods = new[] { "GET", "POST" };
                var routeGroupHttpMethods = new[] { "GET" };

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute("", route => route
                            .WithConstraints(constraints => constraints
                                .HttpMethods(routeHttpMethods))))
                    .WithConstraints(constraints => constraints
                        .HttpMethods(routeGroupHttpMethods)));

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
                Assert.IsTrue(Enumerable.SequenceEqual(routeHttpMethods, constraint.AllowedMethods));
            }

            [TestMethod]
            public void ShouldNotOverrideAnExistingHttpMethodContraint2()
            {
                // Arrange
                var routeHttpMethods = new[] { HttpMethod.Get, HttpMethod.Post };
                var routeGroupHttpMethods = new[] { HttpMethod.Get };

                // Act
                routes.ForGroup(group => group
                    .Map<TestController>(controller => controller.Index(), map => map
                        .ToRoute("", route => route
                            .WithConstraints(constraints => constraints
                                .HttpMethods(routeHttpMethods))))
                    .WithConstraints(constraints => constraints
                        .HttpMethods(routeGroupHttpMethods)));

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
                Assert.IsTrue(Enumerable.SequenceEqual(routeHttpMethods.Select(httpMethod => httpMethod.ToString()), constraint.AllowedMethods));
            }
        }
    }
}
