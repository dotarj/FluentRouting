// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    /// <summary>
    /// Allows mapping of URL routes.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    public class FluentRouteMapBuilder<TController> where TController : Controller
    {
        private readonly FluentRouteGroupBuilder routeGroupBuilder;
        private readonly MethodCallExpression methodCallExpression;

        internal FluentRouteMapBuilder(FluentRouteGroupBuilder routeGroupBuilder, Expression<Action<TController>> action)
        {
            if (routeGroupBuilder == null)
            {
                throw new ArgumentNullException("routeGroupBuilder");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var methodCallExpression = action.Body as MethodCallExpression;

            if (methodCallExpression == null)
            {
                throw new ArgumentException("Expression must be a method call.", "action");
            }

            this.routeGroupBuilder = routeGroupBuilder;
            this.methodCallExpression = methodCallExpression;
        }

        /// <summary>
        /// Maps the specified URL route.
        /// </summary>
        /// <param name="template">The route template describing the URI pattern to match against.</param>
        /// <returns>The same <see cref="FluentRouteMapBuilder{TController}"/> instance so that multiple calls can be chained.</returns>
        public FluentRouteMapBuilder<TController> ToRoute(string template)
        {
            return ToRoute(template, new DefaultInlineConstraintResolver(), null);
        }

        /// <summary>
        /// Maps the specified URL route.
        /// </summary>
        /// <param name="template">The route template describing the URI pattern to match against.</param>
        /// <param name="constraintResolver">The <see cref="IInlineConstraintResolver"/> to use for resolving inline constraints in route templates.</param>
        /// <returns>The same <see cref="FluentRouteMapBuilder{TController}"/> instance so that multiple calls can be chained.</returns>
        public FluentRouteMapBuilder<TController> ToRoute(string template, IInlineConstraintResolver constraintResolver)
        {
            return ToRoute(template, constraintResolver, null);
        }

        /// <summary>
        /// Maps the specified URL route.
        /// </summary>
        /// <param name="template">The route template describing the URI pattern to match against.</param>
        /// <param name="routeBuilderAction">An action that performs configuration against a <see cref="FluentRouteBuilder"/>.</param>
        /// <returns>The same <see cref="FluentRouteMapBuilder{TController}"/> instance so that multiple calls can be chained.</returns>
        public FluentRouteMapBuilder<TController> ToRoute(string template, Action<FluentRouteBuilder> routeBuilderAction)
        {
            return ToRoute(template, new DefaultInlineConstraintResolver(), routeBuilderAction);
        }

        /// <summary>
        /// Maps the specified URL route.
        /// </summary>
        /// <param name="template">The route template describing the URI pattern to match against.</param>
        /// <param name="constraintResolver">The <see cref="IInlineConstraintResolver"/> to use for resolving inline constraints in route templates.</param>
        /// <param name="routeBuilderAction">An action that performs configuration against a <see cref="FluentRouteBuilder"/>.</param>
        /// <returns>The same <see cref="FluentRouteMapBuilder{TController}"/> instance so that multiple calls can be chained.</returns>
        public FluentRouteMapBuilder<TController> ToRoute(string template, IInlineConstraintResolver constraintResolver, Action<FluentRouteBuilder> routeBuilderAction)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            if (constraintResolver == null)
            {
                throw new ArgumentNullException("constraintResolver");
            }

            var actionName = GetActionName(methodCallExpression.Method);

            ValidateTemplate(template, actionName, typeof(TController));

            var routeBuilder = CreateRouteBuilder(template, constraintResolver);

            if (routeBuilderAction != null)
            {
                routeBuilderAction(routeBuilder);
            }

            routeGroupBuilder.RouteGroup.AddRoute(routeBuilder.Name, routeBuilder.Route);

            return this;
        }

        private static void ValidateTemplate(string routeTemplate, string actionName, Type controllerType)
        {
            if (routeTemplate.StartsWith("/", StringComparison.Ordinal))
            {
                throw new InvalidOperationException(string.Format("The route template '{0}' on the action named '{1}' on the controller named '{2}' cannot begin with a forward slash.", routeTemplate, actionName, controllerType.Name));
            }
        }

        private FluentRouteBuilder CreateRouteBuilder(string url, IInlineConstraintResolver constraintResolver)
        {
            var controllerName = GetControllerName(typeof(TController));
            var actionName = GetActionName(methodCallExpression.Method);

            var routeBuilder = new RouteBuilder(constraintResolver);
            var route = routeBuilder.BuildDirectRoute(url, null, controllerName, actionName, methodCallExpression.Method, null);
            var defaults = BuildRouteValuesFromMethodCallExpression(methodCallExpression);

            route.Defaults.Merge(defaults);

            return new FluentRouteBuilder(route);
        }

        private string GetActionName(MethodInfo methodInfo)
        {
            var actionName = methodInfo.Name;

            if (actionName.EndsWith("Async", StringComparison.OrdinalIgnoreCase))
            {
                actionName = actionName.Substring(0, actionName.Length - "Async".Length);
            }

            return actionName;
        }

        private string GetControllerName(Type controllerType)
        {
            var typeName = controllerType.Name;

            if (typeName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
            {
                typeName = typeName.Substring(0, typeName.Length - "Controller".Length);
            }

            return typeName;
        }

        private RouteValueDictionary BuildRouteValuesFromMethodCallExpression(MethodCallExpression methodCallExpression)
        {
            var routeValues = new RouteValueDictionary();

            var parameterExpressions = methodCallExpression.Method.GetParameters();

            if (parameterExpressions.Length > 0)
            {
                for (int i = 0; i < parameterExpressions.Length; i++)
                {
                    var value = GetArgumentValue(methodCallExpression.Arguments[i]);
                    var defaultValue = GetDefaultParameterValue(parameterExpressions[i]);

                    if (!object.Equals(value, defaultValue))
                    {
                        routeValues.Add(parameterExpressions[i].Name, value);
                    }
                }
            }

            return routeValues;
        }

        private object GetDefaultParameterValue(ParameterInfo parameter)
        {
            if (parameter.HasDefaultValue)
            {
                return parameter.DefaultValue;
            }

            var type = parameter.ParameterType;

            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        private object GetArgumentValue(Expression expression)
        {
            object value;
            var constantExpression = expression as ConstantExpression;

            if (constantExpression != null)
            {
                value = constantExpression.Value;
            }
            else
            {
                var lambdaExpression = Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object)));

                value = lambdaExpression.Compile()();
            }

            return value;
        }
    }
}