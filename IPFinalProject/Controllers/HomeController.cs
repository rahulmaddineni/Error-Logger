using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ErrorLoggerModel;
using LoadersandLogic;

namespace IPFinalProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (TempData["ShowNotification"] as String == "true")
            {
                ViewBag.AppNotification = TempData["UserCreatedNotification"] as String;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Error Logger";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult page404()
        {
            return View();
        }

        public ActionResult page500()
        {
            return View();
        }

        public ActionResult pageOffline()
        {
            return View();
        }

        public ActionResult pageUserDisabled()
        {
            return View();
        }
    }
}