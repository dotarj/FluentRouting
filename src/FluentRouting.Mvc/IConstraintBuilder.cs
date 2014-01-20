// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Provides a method to apply a <see cref="IRouteConstraint"/>.
    /// </summary>
    public interface IConstraintBuilder
    {
        /// <summary>
        /// Adds a <see cref="IRouteConstraint"/> to the <see cref="IConstraintBuilder" />.
        /// </summary>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="constraint">An instance of an <see cref="IRouteConstraint"/>.</param>
        void AddConstraint(string name, IRouteConstraint constraint);
    }
}
