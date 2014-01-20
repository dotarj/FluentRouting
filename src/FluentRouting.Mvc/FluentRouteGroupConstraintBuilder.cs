// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Provides methods for defining the constraints of a <see cref="FluentRouteGroup"/>.
    /// </summary>
    public class FluentRouteGroupConstraintBuilder : IConstraintBuilder
    {
        internal FluentRouteGroupConstraintBuilder(FluentRouteGroup routeGroup)
        {
            if (routeGroup == null)
            {
                throw new ArgumentNullException("routeGroup");
            }

            RouteGroup = routeGroup;
        }

        /// <summary>
        /// Gets the <see cref="FluentRouteGroup"/> which is configured.
        /// </summary>
        public FluentRouteGroup RouteGroup { get; private set; }

        /// <summary>
        /// Adds a <see cref="IRouteConstraint"/> to the <see cref="IConstraintBuilder" />.
        /// </summary>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="constraint">An instance of an <see cref="IRouteConstraint"/>.</param>
        /// <remarks>
        /// The <see cref="IRouteConstraint"/> will only be added if the route constraints do not already contain a constraint 
        /// with the same name.
        /// </remarks>
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

            foreach (var route in RouteGroup)
            {
                if (!route.Constraints.ContainsKey(name))
                {
                    route.Constraints.Add(name, constraint);
                }
            }
        }
    }
}
