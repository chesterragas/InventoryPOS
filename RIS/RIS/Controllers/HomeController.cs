using RIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RIS.Controllers
{
    public class HomeController : Controller
    {
        RISDBEntities db = new RISDBEntities();
        M_Users user = (M_Users)System.Web.HttpContext.Current.Session["user"];

        [SessionExpire]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}