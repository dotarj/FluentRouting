// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Provides methods for defining the constraints of a <see cref="FluentRouteGroupBuilder{TController}"/>.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    public class FluentRouteGroupConstraintBuilder<TController> : IConstraintBuilder where TController : Controller
    {
        internal FluentRouteGroupConstraintBuilder(FluentRouteGroupBuilder<TController> groupBuilder)
        {
            if (groupBuilder == null)
            {
                throw new ArgumentNullException("routeGroup");
            }

            GroupBuilder = groupBuilder;
        }

        /// <summary>
        /// Gets the <see cref="FluentRouteGroupBuilder{TController}"/> which is configured.
        /// </summary>
        public FluentRouteGroupBuilder<TController> GroupBuilder { get; private set; }

        /// <summary>
        /// Adds a <see cref="IRouteConstraint"/> to the <see cref="IConstraintBuilder" />.
        /// </summary>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="constraintProvider">A function which returns an instance of an <see cref="IRouteConstraint"/>.</param>
        /// <remarks>
        /// The <see cref="IRouteConstraint"/> will only be added if the route constraints do not already contain a constraint 
        /// with the same name.
        /// </remarks>
        public void AddConstraint(string name, Func<IRouteConstraint> constraintProvider)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            
            if (constraintProvider == null)
            {
                throw new ArgumentNullException("constraintProvider");
            }

            foreach (var route in GroupBuilder.Routes)
            {
                if (!route.Constraints.ContainsKey(name))
                {
                    var constraint = constraintProvider();

                    route.Constraints.Add(name, constraint);
                }
            }
        }
    }
}