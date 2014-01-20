// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Web.Routing;

namespace FluentRouting.Mvc
{
    internal static class RouteValueDictionaryExtensions
    {
        internal static void Merge(this RouteValueDictionary target, RouteValueDictionary source)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            foreach (var item in source)
            {
                if (!target.ContainsKey(item.Key))
                {
                    target.Add(item.Key, item.Value);
                }
            }
        }
    }
}