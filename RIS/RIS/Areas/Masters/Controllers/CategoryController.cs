using RIS.Controllers;
using RIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RIS.Controllers.SessionExpire;

namespace RIS.Areas.Masters.Controllers
{
    [SessionExpire]
    public class CategoryController : Controller
    {
        RISDBEntities db = new RISDBEntities();
        M_Users user = (M_Users)System.Web.HttpContext.Current.Session["user"];
        // GET: Masters/Category
        public ActionResult Category()
        {
            return View();
        }

        #region Item Category
        public ActionResult GetCategoryList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<M_ItemCategory> list = db.M_ItemCategory.Where(x => x.IsDeleted != true).ToList();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                try
                {
                    list = list.Where(x => x.CategoryName.ToLower().Contains(searchValue.ToLower())
                                       ).ToList<M_ItemCategory>();
                }
                catch (Exception err) { }
            }
            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortDirection == "asc")
                {
                    list = list.OrderBy(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();
                }
                else
                {
                    list = list.OrderByDescending(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();
                }
            }
            int totalrows = list.Count;
            int totalrowsafterfiltering = list.Count;


            //paging
            list = list.Skip(start).Take(length).ToList<M_ItemCategory>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateCategory(M_ItemCategory data)
        {
            try
            {
                data.CreateID = "Admin";
                data.CreateDate = DateTime.Now;
                data.UpdateID = "Admin";
                data.UpdateDate = DateTime.Now;
                data.IsDeleted = false;

                M_ItemCategory checker = (from c in db.M_ItemCategory
                                   where c.CategoryName == data.CategoryName
                                    && c.IsDeleted == false
                                   select c).FirstOrDefault();
                if (checker == null)
                {
                    db.M_ItemCategory.Add(data);
                    db.SaveChanges();
                    return Json(new { msg = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msg = "Failed" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception err)
            {

                return Json(new { msg = err.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditCategory(M_ItemCategory data)
        {
            try
            {
                M_ItemCategory dataval = new M_ItemCategory();
                dataval = (from u in db.M_ItemCategory.ToList()
                           where u.ID == data.ID
                           select u).FirstOrDefault();
                dataval.CategoryName = data.CategoryName;
               
                dataval.UpdateID = user.UserName;
                dataval.UpdateDate = DateTime.Now;

                M_ItemCategory checker = (from c in db.M_ItemCategory
                                   where c.CategoryName == data.CategoryName
                                   
                                    && c.IsDeleted == false
                                   select c).FirstOrDefault();
                if (checker == null)
                {
                    db.Entry(dataval).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { msg = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msg = "Failed" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception err)
            {

                return Json(new { msg = err.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteCategory(int ID)
        {
            try
            {
                M_ItemCategory dataval = new M_ItemCategory();
                dataval = (from u in db.M_ItemCategory.ToList()
                           where u.ID == ID
                           select u).FirstOrDefault();
                dataval.IsDeleted = true;
                db.Entry(dataval).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { msg = "Success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception err)
            {

                return Json(new { msg = err.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


        #region Service Category
        public ActionResult GetServiceList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<M_ServiceCategory> list = db.M_ServiceCategory.Where(x => x.IsDeleted != true).ToList();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                try
                {
                    list = list.Where(x => x.CategoryNameSer.ToLower().Contains(searchValue.ToLower())
                                       ).ToList<M_ServiceCategory>();
                }
                catch (Exception err) { }
            }
            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortDirection == "asc")
                {
                    list = list.OrderBy(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();
                }
                else
                {
                    list = list.OrderByDescending(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();
                }
            }
            int totalrows = list.Count;
            int totalrowsafterfiltering = list.Count;


            //paging
            list = list.Skip(start).Take(length).ToList<M_ServiceCategory>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateService(M_ServiceCategory data)
        {
            try
            {
                data.CreateID = "Admin";
                data.CreateDate = DateTime.Now;
                data.UpdateID = "Admin";
                data.UpdateDate = DateTime.Now;
                data.IsDeleted = false;

                M_ServiceCategory checker = (from c in db.M_ServiceCategory
                                          where c.CategoryNameSer == data.CategoryNameSer
                                           && c.IsDeleted == false
                                          select c).FirstOrDefault();
                if (checker == null)
                {
                    db.M_ServiceCategory.Add(data);
                    db.SaveChanges();
                    return Json(new { msg = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msg = "Failed" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception err)
            {

                return Json(new { msg = err.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditService(M_ServiceCategory data)
        {
            try
            {
                M_ServiceCategory dataval = new M_ServiceCategory();
                dataval = (from u in db.M_ServiceCategory.ToList()
                           where u.ID2 == data.ID2
                           select u).FirstOrDefault();
                dataval.CategoryNameSer = data.CategoryNameSer;

                dataval.UpdateID = user.UserName;
                dataval.UpdateDate = DateTime.Now;

                M_ServiceCategory checker = (from c in db.M_ServiceCategory
                                          where c.CategoryNameSer == data.CategoryNameSer

                                           && c.IsDeleted == false
                                          select c).FirstOrDefault();
                if (checker == null)
                {
                    db.Entry(dataval).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { msg = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msg = "Failed" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception err)
            {

                return Json(new { msg = err.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteService(int ID)
        {
            try
            {
                M_ServiceCategory dataval = new M_ServiceCategory();
                dataval = (from u in db.M_ServiceCategory.ToList()
                           where u.ID2 == ID
                           select u).FirstOrDefault();
                dataval.IsDeleted = true;
                db.Entry(dataval).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { msg = "Success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception err)
            {

                return Json(new { msg = err.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}