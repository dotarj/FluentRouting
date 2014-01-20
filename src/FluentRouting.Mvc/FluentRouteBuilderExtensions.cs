// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Provides a set of static methods for <see cref="FluentRouteBuilder"/>.
    /// </summary>
    public static class FluentRouteBuilderExtensions
    {
        /// <summary>
        /// Sets the name of the route.
        /// </summary>
        /// <param name="routeBuilder">The <see cref="FluentRouteBuilder"/> to perform configuration against.</param>
        /// <param name="name">The name of the action.</param>
        /// <returns>The same <see cref="FluentRouteBuilder"/> instance so that multiple calls can be chained.</returns>
        public static FluentRouteBuilder WithName(this FluentRouteBuilder routeBuilder, string name)
        {
            if (routeBuilder == null)
            {
                throw new ArgumentNullException("routeBuilder");
            }

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            routeBuilder.Name = name;

            return routeBuilder;
        }

        /// <summary>
        /// Sets the name of the action.
        /// </summary>
        /// <param name="routeBuilder">The <see cref="FluentRouteBuilder"/> to perform configuration against.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <returns>The same <see cref="FluentRouteBuilder"/> instance so that multiple calls can be chained.</returns>
        public static FluentRouteBuilder WithActionName(this FluentRouteBuilder routeBuilder, string actionName)
        {
            if (routeBuilder == null)
            {
                throw new ArgumentNullException("routeBuilder");
            }

            if (actionName == null)
            {
                throw new ArgumentNullException("actionName");
            }

            if (actionName == string.Empty)
            {
                throw new ArgumentException("Value cannot be empty.", "actionName");
            }

            routeBuilder.Route.Defaults["action"] = actionName;

            return routeBuilder;
        }

        /// <summary>
        /// Applies constraints to a <see cref="System.Web.Routing.Route"/> using a <see cref="FluentRouteConstraintBuilder"/>.
        /// </summary>
        /// <param name="routeBuilder">The <see cref="FluentRouteBuilder"/> to perform configuration against.</param>
        /// <param name="constraints">An action that performs configuration against a <see cref="FluentRouteConstraintBuilder"/>.</param>
        public static void WithConstraints(this FluentRouteBuilder routeBuilder, Action<FluentRouteConstraintBuilder> constraints)
        {
            if (routeBuilder == null)
            {
                throw new ArgumentNullException("routeBuilder");
            }

            if (constraints == null)
            {
                throw new ArgumentNullException("constraints");
            }

            constraints(new FluentRouteConstraintBuilder(routeBuilder.Route));
        }
    }
}
