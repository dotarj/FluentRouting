using System.Web.Mvc;

namespace FluentRouting.Mvc.Test
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index(int id)
        {
            return View();
        }

        public ActionResult Index(TestModel model)
        {
            return View();
        }
    }
}
