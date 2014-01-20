// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Provides a set of static methods for <see cref="FluentRouteGroupBuilder"/>.
    /// </summary>
    public static class FluentRouteGroupBuilderExtensions
    {
        /// <summary>
        /// Maps a method the specified route(s).
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="routeGroupBuilder">The <see cref="FluentRouteGroupBuilder"/> to perform configuration against.</param>
        /// <param name="action">A lambda expression representing the method to be mapped.</param>
        /// <param name="map">An action that performs configuration against a <see cref="FluentRouteMapBuilder{TContoller}"/>.</param>
        /// <returns>The same <see cref="FluentRouteGroupBuilder"/> instance so that multiple calls can be chained.</returns>
        public static FluentRouteGroupBuilder Map<TController>(this FluentRouteGroupBuilder routeGroupBuilder, Expression<Action<TController>> action, Action<FluentRouteMapBuilder<TController>> map) where TController : Controller
        {
            if (routeGroupBuilder == null)
            {
                throw new ArgumentNullException("routeGroupBuilder");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (map == null)
            {
                throw new ArgumentNullException("map");
            }

            map(new FluentRouteMapBuilder<TController>(routeGroupBuilder, action));

            return routeGroupBuilder;
        }

        /// <summary>
        /// Applies constraints to a <see cref="FluentRouteGroup"/> using a <see cref="FluentRouteGroupConstraintBuilder"/>.
        /// </summary>
        /// <param name="routeGroupBuilder">The <see cref="FluentRouteGroupBuilder"/> to perform configuration against.</param>
        /// <param name="constraints">An action that performs configuration against a <see cref="FluentRouteGroupConstraintBuilder"/>.</param>
        public static void WithConstraints(this FluentRouteGroupBuilder routeGroupBuilder, Action<FluentRouteGroupConstraintBuilder> constraints)
        {
            if (routeGroupBuilder == null)
            {
                throw new ArgumentNullException("routeGroupBuilder");
            }

            if (constraints == null)
            {
                throw new ArgumentNullException("constraints");
            }

            constraints(new FluentRouteGroupConstraintBuilder(routeGroupBuilder.RouteGroup));
        }
    }
}
