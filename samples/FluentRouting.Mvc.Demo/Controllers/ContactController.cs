// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using FluentRouting.Mvc.Demo.Models;
using System.Web.Mvc;

namespace FluentRouting.Mvc.Demo.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Post(ContactModel contactModel)
        {
            return View();
        }
    }
}