// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Allows configuration to be performed for a <see cref="Route"/>.
    /// </summary>
    public class FluentRouteBuilder
    {
        internal FluentRouteBuilder(Route route)
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
        /// Gets the name of the <see cref="Route"/>
        /// </summary>
        public string Name { get; set; }
    }
}