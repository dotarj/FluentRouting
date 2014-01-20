// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Provides methods for defining the constraints of a <see cref="Route"/>.
    /// </summary>
    public class FluentRouteConstraintBuilder : IConstraintBuilder
    {
        internal FluentRouteConstraintBuilder(Route route)
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            Route = route;
        }

        /// <summary>
        /// Gets the <see cref="Route"/> which is configured.
        /// </summary>
        public Route Route { get; private set; }

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
    }
}
