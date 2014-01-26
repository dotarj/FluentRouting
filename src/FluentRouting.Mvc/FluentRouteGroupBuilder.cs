// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Allows configuration to be performed for a group of routes.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    public class FluentRouteGroupBuilder<TController> where TController : Controller
    {
        private readonly List<Route> routes = new List<Route>();
        private readonly RouteCollection routeCollection;

        internal FluentRouteGroupBuilder(RouteCollection routeCollection)
        {
            if (routeCollection == null)
            {
                throw new ArgumentNullException("routeCollection");
            }

            this.routeCollection = routeCollection;
        }

        /// <summary>
        /// Gets the list of routes in this <see cref="FluentRouteGroupBuilder{TController}"/>
        /// </summary>
        public IReadOnlyList<Route> Routes { get { return routes.AsReadOnly(); } }

        /// <summary>
        /// Maps the specified URL route.
        /// </summary>
        /// <param name="template">The route template describing the URI pattern to match against.</param>
        /// <returns>The <see cref="FluentRouteTemplateBuilder{TController}"/> to perform configuration against.</returns>
        public FluentRouteTemplateBuilder<TController> CreateRoute(string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            return new FluentRouteTemplateBuilder<TController>(template, this);
        }

        /// <summary>
        /// Applies constraints to a group of routes using a <see cref="FluentRouteGroupConstraintBuilder{TController}"/>.
        /// </summary>
        /// <returns>The <see cref="FluentRouteGroupConstraintBuilder{TController}"/> to perform configuration against.</returns>
        public FluentRouteGroupConstraintBuilder<TController> WithGroupConstraints()
        {
            return new FluentRouteGroupConstraintBuilder<TController>(this);
        }

        internal void AddRoute(string name, Route route)
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            routeCollection.Add(name, route);
            routes.Add(route);
        }
    }
}