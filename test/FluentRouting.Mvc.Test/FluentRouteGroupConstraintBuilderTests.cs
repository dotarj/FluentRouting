// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net.Http;
using System.Web.Routing;

namespace FluentRouting.Mvc.Test
{
    [TestClass]
    public abstract class FluentRouteGroupConstraintBuilderTests
    {
        protected RouteCollection routes;

        [TestInitialize]
        public void TestInitialize()
        {
            routes = new RouteCollection();
        }

        [TestClass]
        public class TheCustomMethod : FluentRouteGroupConstraintBuilderTests
        {
            [TestMethod]
            public void ShouldAddAContraint()
            {
                // Act
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index())
                    .WithGroupConstraints()
                        .Custom("host", new HostConstraint("localhost"));

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
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index())
                    .WithConstraints()
                        .Custom("host", new HostConstraint(routeHost))
                    .WithGroupConstraints()
                        .Custom("host", new HostConstraint(routeGroupHost));

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
        public class TheHostMethod : FluentRouteGroupConstraintBuilderTests
        {
            [TestMethod]
            public void ShouldAddAHostContraintForLocalhost()
            {
                // Arrange
                var host = "localhost";

                // Act
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index())
                    .WithGroupConstraints()
                        .Host(host);

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
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index())
                    .WithConstraints()
                        .Host(routeHost)
                    .WithGroupConstraints()
                        .Host(routeGroupHost);

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
        public class TheHostsMethod : FluentRouteGroupConstraintBuilderTests
        {
            [TestMethod]
            public void ShouldAddAHostContraintForLocalhostAndRemotehost()
            {
                // Arrange
                var host = new[] { "localhost", "remotehost" };

                // Act
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index())
                    .WithGroupConstraints()
                        .Hosts(host);

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
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index(1))
                    .WithConstraints()
                        .Hosts(routeHost)
                    .WithGroupConstraints()
                        .Hosts(routeGroupHost);

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
        public class TheHttpMethodMethod : FluentRouteGroupConstraintBuilderTests
        {
            [TestMethod]
            public void ShouldAddAHttpMethodContraintForHttpMethodGet()
            {
                // Arrange
                var httpMethod = HttpMethod.Get;

                // Act
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index(new TestModel()))
                    .WithGroupConstraints()
                        .HttpMethod(httpMethod);

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
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index())
                    .WithGroupConstraints()
                        .HttpMethod(httpMethod);

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
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index())
                    .WithConstraints()
                        .HttpMethod(routeHttpMethod)
                    .WithGroupConstraints()
                        .HttpMethod(routeGroupHttpMethod);

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
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index())
                    .WithConstraints()
                        .HttpMethod(routeHttpMethod)
                    .WithGroupConstraints()
                        .HttpMethod(routeGroupHttpMethod);

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
        public class TheHttpMethodsMethod : FluentRouteGroupConstraintBuilderTests
        {
            [TestMethod]
            public void ShouldAddAHttpMethodContraintForHttpMethodGetAndHttpMethodPost()
            {
                // Arrange
                var httpMethods = new[] { HttpMethod.Get, HttpMethod.Post };

                // Act
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index())
                    .WithGroupConstraints()
                        .HttpMethods(httpMethods);

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
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index())
                    .WithGroupConstraints()
                        .HttpMethods(httpMethods);

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
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index())
                    .WithConstraints()
                        .HttpMethods(routeHttpMethods)
                    .WithGroupConstraints()
                        .HttpMethods(routeGroupHttpMethods);

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
                routes.ForController<TestController>().CreateRoute("").ToMethod(controller => controller.Index())
                    .WithConstraints()
                        .HttpMethods(routeHttpMethods)
                    .WithGroupConstraints()
                        .HttpMethods(routeGroupHttpMethods);

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
