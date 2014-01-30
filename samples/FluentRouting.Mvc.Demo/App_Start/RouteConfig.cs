// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using FluentRouting.Mvc.Demo.Controllers;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentRouting.Mvc.Demo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.For<HomeController>()
                .CreateRoute("").WithName("bla").To(controller => controller.Index())
                .CreateRoute("multipleroutestest1").To(controller => controller.MultipleRoutesTest())
                .CreateRoute("multipleroutestest2").To(controller => controller.MultipleRoutesTest())
                .CreateRoute("inlineconstrainttest/{id:range(0,100)}").To(controller => controller.InlineConstraintTest1(0))
                .CreateRoute("inlineconstrainttest/{id:range(101,200)}").To(controller => controller.InlineConstraintTest2(0))
                .WithGroupConstraints().HttpMethod(HttpMethod.Get);

            routes.For<ContactController>()
                .CreateRoute("contact").To(controller => controller.Index())
                .CreateRoute("contact").To(controller => controller.Post(null))
                    .WithConstraints().HttpMethod(HttpMethod.Post)
                .WithGroupConstraints().HttpMethod(HttpMethod.Get);
        }
    }
}