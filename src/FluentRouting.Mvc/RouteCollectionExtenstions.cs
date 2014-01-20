// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Provides a set of static methods for <see cref="RouteCollection"/>.
    /// </summary>
    public static class RouteCollectionExtenstions
    {
        /// <summary>
        /// Maps a method the specified route(s).
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="routeCollection">A collection of routes for the application.</param>
        /// <param name="action">A lambda expression representing the method to be mapped.</param>
        /// <param name="map">An action that performs configuration against a <see cref="FluentRouteMapBuilder{TContoller}"/>.</param>
        /// <returns>The same <see cref="RouteCollection"/> instance so that multiple calls can be chained.</returns>
        public static RouteCollection Map<TController>(this RouteCollection routeCollection, Expression<Action<TController>> action, Action<FluentRouteMapBuilder<TController>> map) where TController : Controller
        {
            if (routeCollection == null)
            {
                throw new ArgumentNullException("routeCollection");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (map == null)
            {
                throw new ArgumentNullException("map");
            }

            var routeGroup = new FluentRouteGroupBuilder(routeCollection);

            map(new FluentRouteMapBuilder<TController>(routeGroup, action));

            return routeCollection;
        }

        /// <summary>
        /// Creates a <see cref="FluentRouteGroupBuilder"/> which can be used to map and configure routes.
        /// </summary>
        /// <param name="routeCollection">A collection of routes for the application.</param>
        /// <param name="group">An action that performs configuration against a <see cref="FluentRouteGroupBuilder"/>.</param>
        /// <returns>The same <see cref="RouteCollection"/> instance so that multiple calls can be chained.</returns>
        public static RouteCollection ForGroup(this RouteCollection routeCollection, Action<FluentRouteGroupBuilder> group) 
        {
            if (routeCollection == null)
            {
                throw new ArgumentNullException("routeCollection");
            }

            if (group == null)
            {
                throw new ArgumentNullException("group");
            }

            group(new FluentRouteGroupBuilder(routeCollection));

            return routeCollection;
        }
    }
}