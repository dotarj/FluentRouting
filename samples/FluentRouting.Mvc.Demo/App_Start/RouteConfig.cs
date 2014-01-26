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

            routes.ForController<HomeController>()
                .CreateRoute("").ToMethod(controller => controller.Index())
                .CreateRoute("multipleroutestest1").ToMethod(controller => controller.MultipleRoutesTest())
                .CreateRoute("multipleroutestest2").ToMethod(controller => controller.MultipleRoutesTest())
                .CreateRoute("inlineconstrainttest/{id:range(0,100)}").ToMethod(controller => controller.InlineConstraintTest1(0))
                .CreateRoute("inlineconstrainttest/{id:range(101,200)}").ToMethod(controller => controller.InlineConstraintTest2(0))
                .WithGroupConstraints().HttpMethod(HttpMethod.Get);

            routes.ForController<ContactController>()
                .CreateRoute("contact").ToMethod(controller => controller.Index())
                .CreateRoute("contact").ToMethod(controller => controller.Post(null))
                    .WithConstraints().HttpMethod(HttpMethod.Post)
                .WithGroupConstraints().HttpMethod(HttpMethod.Get);
        }
    }
}