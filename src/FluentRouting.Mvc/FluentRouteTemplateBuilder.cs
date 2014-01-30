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
    /// Allows configuration to be performed for a route template.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    public class FluentRouteTemplateBuilder<TController> where TController : Controller
    {
        private readonly string template;

        private string name;
        private IInlineConstraintResolver inlineConstraintResolver;

        internal FluentRouteTemplateBuilder(string template, FluentRouteGroupBuilder<TController> groupBuilder)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            if (groupBuilder == null)
            {
                throw new ArgumentNullException("groupBuilder");
            }

            this.template = template;
            GroupBuilder = groupBuilder;
        }

        /// <summary>
        /// Gets the <see cref="FluentRouteGroupBuilder{TController}"/> which is configured.
        /// </summary>
        public FluentRouteGroupBuilder<TController> GroupBuilder { get; private set; }

        /// <summary>
        /// Maps a method to the specified route.
        /// </summary>
        /// <param name="action">A lambda expression representing the method to be mapped.</param>
        /// <returns>The <see cref="FluentRouteBuilder{TController}"/> to perform configuration against.</returns>
        public FluentRouteBuilder<TController> To(Expression<Action<TController>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var methodCallExpression = action.Body as MethodCallExpression;

            if (methodCallExpression == null)
            {
                throw new ArgumentException("Expression must be a method call.", "action");
            }

            var actionName = GetActionName(methodCallExpression.Method);

            ValidateTemplate(template, actionName, typeof(TController));

            var routeBuilder = CreateRouteBuilder(template, methodCallExpression);

            GroupBuilder.AddRoute(name, routeBuilder.Route);

            return routeBuilder;
        }

        /// <summary>
        /// Sets the <see cref="IInlineConstraintResolver"/> to use for resolving inline constraints in route templates.
        /// </summary>
        /// <param name="inlineConstraintResolver">An instance of an <see cref="IInlineConstraintResolver"/>.</param>
        /// <returns>The same <see cref="FluentRouteTemplateBuilder{TController}"/> instance so that multiple calls can be chained.</returns>
        public FluentRouteTemplateBuilder<TController> WithInlineConstraintResolver(IInlineConstraintResolver inlineConstraintResolver)
        {
            if (inlineConstraintResolver == null)
            {
                throw new ArgumentNullException("inlineConstraintResolver");
            }

            this.inlineConstraintResolver = inlineConstraintResolver;

            return this;
        }

        /// <summary>
        /// Sets the name of the route.
        /// </summary>
        /// <param name="name">The name of the action.</param>
        /// <returns>The same <see cref="FluentRouteTemplateBuilder{TController}"/> instance so that multiple calls can be chained.</returns>
        public FluentRouteTemplateBuilder<TController> WithName(string name)
        {
            this.name = name;

            return this;
        }

        private static void ValidateTemplate(string routeTemplate, string actionName, Type controllerType)
        {
            if (routeTemplate.StartsWith("/", StringComparison.Ordinal))
            {
                throw new InvalidOperationException(string.Format("The route template '{0}' on the action named '{1}' on the controller named '{2}' cannot begin with a forward slash.", routeTemplate, actionName, controllerType.Name));
            }
        }

        private FluentRouteBuilder<TController> CreateRouteBuilder(string template, MethodCallExpression methodCallExpression)
        {
            var controllerName = GetControllerName(typeof(TController));
            var actionName = GetActionName(methodCallExpression.Method);

            var routeBuilder = new RouteBuilder(inlineConstraintResolver ?? new DefaultInlineConstraintResolver());
            var route = routeBuilder.BuildDirectRoute(template, null, controllerName, actionName, methodCallExpression.Method, null);
            var defaults = BuildRouteValuesFromMethodCallExpression(methodCallExpression);

            route.Defaults.Merge(defaults);

            return new FluentRouteBuilder<TController>(route, GroupBuilder);
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
