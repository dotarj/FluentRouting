// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Allows configuration to be performed for a <see cref="Route"/>.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    public class FluentRouteBuilder<TController> where TController : Controller
    {
        internal FluentRouteBuilder(Route route, FluentRouteGroupBuilder<TController> groupBuilder)
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (groupBuilder == null)
            {
                throw new ArgumentNullException("groupBuilder");
            }

            Route = route;
            GroupBuilder = groupBuilder;
        }

        /// <summary>
        /// Gets the <see cref="Route"/> which is configured.
        /// </summary>
        public Route Route { get; private set; }

        /// <summary>
        /// Gets the <see cref="FluentRouteGroupBuilder{TController}"/> which is configured.
        /// </summary>
        public FluentRouteGroupBuilder<TController> GroupBuilder { get; private set; }

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

            return GroupBuilder.CreateRoute(template);
        }

        /// <summary>
        /// Sets the name of the action.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <returns>The same <see cref="FluentRouteBuilder{TController}"/> instance so that multiple calls can be chained.</returns>
        public FluentRouteBuilder<TController> WithActionName(string actionName)
        {
            if (actionName == null)
            {
                throw new ArgumentNullException("actionName");
            }

            if (actionName == string.Empty)
            {
                throw new ArgumentException("Value cannot be empty.", "actionName");
            }

            Route.Defaults["action"] = actionName;

            return this;
        }

        /// <summary>
        /// Applies constraints to a <see cref="System.Web.Routing.Route"/> using a <see cref="FluentRouteConstraintBuilder{TController}"/>.
        /// </summary>
        /// <returns>The <see cref="FluentRouteConstraintBuilder{TController}"/> to perform configuration against.</returns>
        public FluentRouteConstraintBuilder<TController> WithConstraints()
        {
            return new FluentRouteConstraintBuilder<TController>(Route, GroupBuilder);
        }

        /// <summary>
        /// Applies constraints to a group of routes using a <see cref="FluentRouteGroupConstraintBuilder{TController}"/>.
        /// </summary>
        /// <returns>The <see cref="FluentRouteGroupConstraintBuilder{TController}"/> to perform configuration against.</returns>
        public FluentRouteGroupConstraintBuilder<TController> WithGroupConstraints()
        {
            return new FluentRouteGroupConstraintBuilder<TController>(GroupBuilder);
        }
    }
}