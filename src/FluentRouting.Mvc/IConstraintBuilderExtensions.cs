// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Provides a set of static methods for <see cref="IConstraintBuilder"/>.
    /// </summary>
    public static class IConstraintBuilderExtensions
    {
        /// <summary>
        /// Adds a <see cref="HttpMethodConstraint"/> to the <see cref="IConstraintBuilder" />.
        /// </summary>
        /// <param name="constraintBuilder">The <see cref="IConstraintBuilder"/> to perform configuration against.</param>
        /// <param name="allowedMethod">The HTTP verb that is valid for the routes.</param>
        /// <returns>The same <see cref="IConstraintBuilder"/> instance so that multiple calls can be chained.</returns>
        public static T HttpMethod<T>(this T constraintBuilder, HttpMethod allowedMethod) where T : IConstraintBuilder
        {
            if (constraintBuilder == null)
            {
                throw new ArgumentNullException("constraintBuilder");
            }

            return constraintBuilder.HttpMethods(allowedMethod);
        }

        /// <summary>
        /// Adds a <see cref="HttpMethodConstraint"/> to the <see cref="IConstraintBuilder" />.
        /// </summary>
        /// <param name="constraintBuilder">The <see cref="IConstraintBuilder"/> to perform configuration against.</param>
        /// <param name="allowedMethods">The HTTP verbs that are valid for the routes.</param>
        /// <returns>The same <see cref="IConstraintBuilder"/> instance so that multiple calls can be chained.</returns>
        public static T HttpMethods<T>(this T constraintBuilder, params HttpMethod[] allowedMethods) where T : IConstraintBuilder
        {
            if (constraintBuilder == null)
            {
                throw new ArgumentNullException("constraintBuilder");
            }

            if (allowedMethods == null)
            {
                throw new ArgumentNullException("allowedMethods");
            }

            var allowedMethodsStrings = allowedMethods
                .Select(allowedMethod => allowedMethod.ToString())
                .ToArray();

            constraintBuilder.AddConstraint("httpMethod", new HttpMethodConstraint(allowedMethodsStrings));

            return constraintBuilder;
        }

        /// <summary>
        /// Adds a <see cref="HttpMethodConstraint"/> to the <see cref="IConstraintBuilder" />.
        /// </summary>
        /// <param name="constraintBuilder">The <see cref="IConstraintBuilder"/> to perform configuration against.</param>
        /// <param name="allowedMethod">The HTTP verb that is valid for the routes.</param>
        /// <returns>The same <see cref="IConstraintBuilder"/> instance so that multiple calls can be chained.</returns>
        public static T HttpMethod<T>(this T constraintBuilder, string allowedMethod) where T : IConstraintBuilder
        {
            if (constraintBuilder == null)
            {
                throw new ArgumentNullException("constraintBuilder");
            }

            return constraintBuilder.HttpMethods(allowedMethod);
        }

        /// <summary>
        /// Adds a <see cref="HttpMethodConstraint"/> to the <see cref="IConstraintBuilder" />.
        /// </summary>
        /// <param name="constraintBuilder">The <see cref="IConstraintBuilder"/> to perform configuration against.</param>
        /// <param name="allowedMethods">The HTTP verbs that are valid for the routes.</param>
        /// <returns>The same <see cref="IConstraintBuilder"/> instance so that multiple calls can be chained.</returns>
        public static T HttpMethods<T>(this T constraintBuilder, params string[] allowedMethods) where T : IConstraintBuilder
        {
            if (constraintBuilder == null)
            {
                throw new ArgumentNullException("constraintBuilder");
            }

            if (allowedMethods == null)
            {
                throw new ArgumentNullException("allowedMethods");
            }

            constraintBuilder.AddConstraint("httpMethod", new HttpMethodConstraint(allowedMethods));

            return constraintBuilder;
        }

        /// <summary>
        /// Adds a <see cref="HostConstraint"/> to the <see cref="IConstraintBuilder" />.
        /// </summary>
        /// <param name="constraintBuilder">The <see cref="IConstraintBuilder"/> to perform configuration against.</param>
        /// <param name="allowedHost">The host that is valid for the routes.</param>
        /// <returns>The same <see cref="IConstraintBuilder"/> instance so that multiple calls can be chained.</returns>
        public static T Host<T>(this T constraintBuilder, string allowedHost) where T : IConstraintBuilder
        {
            if (constraintBuilder == null)
            {
                throw new ArgumentNullException("constraintBuilder");
            }

            return constraintBuilder.Hosts(allowedHost);
        }

        /// <summary>
        /// Adds a <see cref="HostConstraint"/> to the <see cref="IConstraintBuilder" />.
        /// </summary>
        /// <param name="constraintBuilder">The <see cref="IConstraintBuilder"/> to perform configuration against.</param>
        /// <param name="allowedHosts">The hosts that are valid for the routes.</param>
        /// <returns>The same <see cref="IConstraintBuilder"/> instance so that multiple calls can be chained.</returns>
        public static T Hosts<T>(this T constraintBuilder, params string[] allowedHosts) where T : IConstraintBuilder
        {
            if (constraintBuilder == null)
            {
                throw new ArgumentNullException("constraintBuilder");
            }

            if (allowedHosts == null)
            {
                throw new ArgumentNullException("allowedHosts");
            }

            constraintBuilder.AddConstraint("host", new HostConstraint(allowedHosts));

            return constraintBuilder;
        }

        /// <summary>
        /// Adds a <see cref="IRouteConstraint"/> to the <see cref="IConstraintBuilder" />.
        /// </summary>
        /// <param name="constraintBuilder">The <see cref="IConstraintBuilder"/> to perform configuration against.</param>
        /// <param name="name">The name of the constraint.</param>
        /// <param name="constraint">An instance of an <see cref="IRouteConstraint"/>.</param>
        /// <returns>The same <see cref="IConstraintBuilder"/> instance so that multiple calls can be chained.</returns>
        public static T Custom<T>(this T constraintBuilder, string name, IRouteConstraint constraint) where T : IConstraintBuilder
        {
            if (constraintBuilder == null)
            {
                throw new ArgumentNullException("constraintBuilder");
            }

            if (constraint == null)
            {
                throw new ArgumentNullException("constraint");
            }

            constraintBuilder.AddConstraint(name, constraint);

            return constraintBuilder;
        }
    }
}