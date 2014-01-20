// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Enables you to define which HTTP host header values are allowed when ASP.NET routing determines
    /// whether a URL matches a route.
    /// </summary>
    public class HostConstraint : IRouteConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostConstraint"/> class by using the HTTP host 
        /// header values are allowed for the route.
        /// </summary>
        /// <param name="allowedHosts">The HTTP host header values that are valid for the route.</param>
        /// <exception cref="ArgumentNullException">The allowedHosts parameter is null.</exception>
        public HostConstraint(params string[] allowedHosts)
        {
            if (allowedHosts == null)
            {
                throw new ArgumentNullException("allowedHosts");
            }

            AllowedHosts = allowedHosts.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the collection of allowed HTTP verbs for the route.
        /// </summary>
        /// <returns>A collection of allowed HTTP host header values for the route.</returns>
        public ICollection<string> AllowedHosts { get; private set; }

        /// <summary>
        /// Determines whether the request was made with an HTTP host heade value that is one of
        /// the allowed values for the route.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <param name="route">The object that is being checked to determine whether it matches the URL.</param>
        /// <param name="parameterName">The name of the parameter that is being checked.</param>
        /// <param name="values">An object that contains the parameters for a route.</param>
        /// <param name="routeDirection">An object that indicates whether the constraint check is being performed
        /// when an incoming request is processed or when a URL is generated.</param>
        /// <returns>When ASP.NET routing is processing a request, true if the request was made
        /// by using an allowed HTTP host header value; otherwise, false. When ASP.NET routing is
        /// constructing a URL, true if the supplied values contain an HTTP header value that
        /// matches one of the allowed HTTP header values; otherwise, false. The default is true.</returns>
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (parameterName == null)
            {
                throw new ArgumentNullException("parameterName");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            switch (routeDirection)
            {
                case RouteDirection.IncomingRequest:
                    return AllowedHosts.Any(method => String.Equals(method, httpContext.Request.Url.Host, StringComparison.OrdinalIgnoreCase));
                case RouteDirection.UrlGeneration:
                    // We need to see if the user specified the host explicitly.  Consider these two routes:
                    //
                    // a) Route: template = "/{foo}", Constraints = { host = new HostConstraint("domain-a.com") }
                    // b) Route: template = "/{foo}", Constraints = { host = new HostConstraint("domain-b.com") }
                    //
                    // A user might know ahead of time that a URI he/she is generating might be used with a particular host. 
                    // If a URI will be used for a domain-b.com but we match on (a) while generating the URI, then
                    // the domain-a.com-specific route will be used for URI generation, which might have undesired behavior.
                    // To prevent this, a user might call RouteCollection.GetVirtualPath(..., { host = "domain-b.com" }) to
                    // signal that he is generating a URI that will be used for domain-b.com, so he wants the URI
                    // generation to be performed by the (b) route instead of the (a) route, consistent with what would
                    // happen on incoming requests.
                    object parameterValue;

                    if (!values.TryGetValue(parameterName, out parameterValue))
                    {
                        return true;
                    }

                    var parameterValueString = parameterValue as string;

                    if (parameterValueString == null)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, "The constraint for route parameter '{0}' on the route with URL '{1}' must have a string value in order to use an HostConstraint.", parameterName, route.Url));
                    }

                    return AllowedHosts.Any(method => string.Equals(method, parameterValueString, StringComparison.OrdinalIgnoreCase));
                default:
                    throw new InvalidEnumArgumentException(string.Empty, (int)routeDirection, typeof(RouteDirection));
            }
        }
    }
}