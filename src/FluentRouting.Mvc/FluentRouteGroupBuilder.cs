// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Allows configuration to be performed for a <see cref="FluentRouteGroup"/>.
    /// </summary>
    public class FluentRouteGroupBuilder
    {
        internal FluentRouteGroupBuilder(RouteCollection routeCollection)
        {
            if (routeCollection == null)
            {
                throw new ArgumentNullException("routeCollection");
            }

            RouteGroup = new FluentRouteGroup(routeCollection);
        }

        /// <summary>
        /// Gets the <see cref="FluentRouteGroup"/> which is configured.
        /// </summary>
        public FluentRouteGroup RouteGroup { get; private set; }
    }
}
