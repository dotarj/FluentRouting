// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Provides a set of static methods for <see cref="RouteCollection"/>.
    /// </summary>
    public static class RouteCollectionExtenstions
    {
        /// <summary>
        /// Creates a <see cref="FluentRouteGroupBuilder{TController}"/> which can be used to map and configure routes.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="routeCollection">A collection of routes for the application.</param>
        /// <returns>The <see cref="FluentRouteGroupBuilder{TController}"/> to perform configuration against.</returns>
        public static FluentRouteGroupBuilder<TController> For<TController>(this RouteCollection routeCollection) where TController : Controller
        {
            if (routeCollection == null)
            {
                throw new ArgumentNullException("routeCollection");
            }

            return new FluentRouteGroupBuilder<TController>(routeCollection);
        }
    }
}