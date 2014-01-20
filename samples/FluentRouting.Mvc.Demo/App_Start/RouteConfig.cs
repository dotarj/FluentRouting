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

            routes.ForGroup(group => group
                .Map<HomeController>(controller => controller.Index(), map => map
                    .ToRoute(""))
                .Map<HomeController>(controller => controller.MultipleRoutesTest(), map => map
                    .ToRoute("multipleroutestest1")
                    .ToRoute("multipleroutestest2"))
                .Map<HomeController>(controller => controller.InlineConstraintTest1(0), map => map
                    .ToRoute("inlineconstrainttest/{id:range(0,100)}"))
                .Map<HomeController>(controller => controller.InlineConstraintTest2(0), map => map
                    .ToRoute("inlineconstrainttest/{id:range(101,200)}"))
                .Map<ContactController>(controller => controller.Index(), map => map
                    .ToRoute("contact"))
                .Map<ContactController>(controller => controller.Post(null), map => map
                    .ToRoute("contact", route => route
                        .WithConstraints(constraints => constraints
                            .HttpMethods(HttpMethod.Post))))
                .WithConstraints(constraints => constraints
                    .HttpMethod(HttpMethod.Get)));
        }
    }
}