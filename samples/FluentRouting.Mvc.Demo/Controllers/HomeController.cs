// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System.Web.Mvc;

namespace FluentRouting.Mvc.Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MultipleRoutesTest()
        {
            return View();
        }

        public ActionResult InlineConstraintTest1(int id)
        {
            return View();
        }

        public ActionResult InlineConstraintTest2(int id)
        {
            return View();
        }
    }
}