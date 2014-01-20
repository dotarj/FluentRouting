// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Represents a group of routes.
    /// </summary>
    public class FluentRouteGroup : IEnumerable<Route>
    {
        private readonly RouteCollection routeCollection;
        private IList<Route> routes = new List<Route>();

        internal FluentRouteGroup(RouteCollection routeCollection)
        {
            if (routeCollection == null)
            {
                throw new ArgumentNullException("routeCollection");
            }

            this.routeCollection = routeCollection;
        }

        internal void AddRoute(string name, Route route)
        {
            routeCollection.Add(name, route);
            routes.Add(route);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{Route}"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<Route> GetEnumerator()
        {
            return routes.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return routes.GetEnumerator();
        }
    }
}
