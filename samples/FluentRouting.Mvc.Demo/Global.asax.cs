// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System.Web.Mvc;
using System.Web.Routing;

namespace FluentRouting.Mvc.Demo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
