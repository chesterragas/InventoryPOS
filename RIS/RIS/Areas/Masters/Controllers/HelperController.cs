using RIS.Controllers;
using RIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RIS.Areas.Masters.Controllers
{
    [SessionExpire]
    public class HelperController : Controller
    {
        RISDBEntities db = new RISDBEntities();

        // GET: Masters/Helper
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDropdown_CategoryItem()
        {
            var list = (from w in db.M_ItemCategory.ToList()
                        where w.IsDeleted != true
                        select new { text = w.CategoryName, value = w.ID }).Distinct().ToList();
            return Json(new { list = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDropdown_Categoryservice()
        {
            var list = (from w in db.M_ServiceCategory.ToList()
                        where w.IsDeleted != true
                        select new { text = w.CategoryNameSer, value = w.ID2 }).Distinct().ToList();
            return Json(new { list = list }, JsonRequestBehavior.AllowGet);
        }
    }
}