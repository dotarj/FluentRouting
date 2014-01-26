// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Provides methods for defining the constraints of a <see cref="Route"/>.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    public class FluentRouteConstraintBuilder<TController> : IConstraintBuilder where TController : Controller
    {
        internal FluentRouteConstraintBuilder(Route route, FluentRouteGroupBuilder<TController> groupBuilder)
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
        /// Adds a <see cref="IRouteConstraint"/> to the <see cref="IConstraintBuilder" />.
        /// </summary>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="constraint">An instance of an <see cref="IRouteConstraint"/>.</param>
        public void AddConstraint(string name, IRouteConstraint constraint)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (constraint == null)
            {
                throw new ArgumentNullException("constraint");
            }

            Route.Constraints[name] = constraint;
        }

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
        /// Applies constraints to a group of routes using a <see cref="FluentRouteGroupConstraintBuilder{TController}"/>.
        /// </summary>
        /// <returns>The <see cref="FluentRouteGroupConstraintBuilder{TController}"/> to perform configuration against.</returns>
        public FluentRouteGroupConstraintBuilder<TController> WithGroupConstraints()
        {
            return new FluentRouteGroupConstraintBuilder<TController>(GroupBuilder);
        }
    }
}
