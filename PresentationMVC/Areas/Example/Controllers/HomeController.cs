using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationMVC.Areas.Example.Controllers
{
    public class HomeController : Controller
    {
        // GET: Example/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}
