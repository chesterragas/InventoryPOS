using RIS.Controllers;
using RIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using static RIS.Controllers.SessionExpire;

namespace RIS.Areas.Masters.Controllers
{
    [SessionExpire]
    public class ServiceController : Controller
    {
        RISDBEntities db = new RISDBEntities();
        M_Users user = (M_Users)System.Web.HttpContext.Current.Session["user"];
        // GET: Masters/Services
        public ActionResult Service()
        {
            return View();
        }

        public ActionResult GetServiceList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<GET_M_ServiceList_Result> list = db.GET_M_ServiceList().ToList();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                try
                {
                    list = list.Where(x => x.ServiceName.ToLower().Contains(searchValue.ToLower())
                                       ).ToList<GET_M_ServiceList_Result>();
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
            list = list.Skip(start).Take(length).ToList<GET_M_ServiceList_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateService(M_Service data)
        {
            try
            {
                data.CreateID = user.CreateID;
                data.CreateDate = DateTime.Now;
                data.UpdateID = user.CreateID;
                data.UpdateDate = DateTime.Now;
                data.IsDeleted = false;
                M_Service checker = (from c in db.M_Service
                                     where c.ServiceName == data.ServiceName
                                      && c.ServiceCategory == data.ServiceCategory
                                      && c.Price == data.Price
                                      && c.IsDeleted == false
                                     select c).FirstOrDefault();
                if (checker == null)
                {
                    db.M_Service.Add(data);
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

        public ActionResult EditService(M_Service data)
        {
            try
            {
                M_Service dataval = new M_Service();
                dataval = (from u in db.M_Service.ToList()
                           where u.ID == data.ID
                           select u).FirstOrDefault();
                dataval.ServiceCategory = data.ServiceCategory;
                dataval.ServiceName = data.ServiceName;
                dataval.Price = data.Price;
                dataval.Photo = data.Photo;

                dataval.UpdateID = user.UserName;
                dataval.UpdateDate = DateTime.Now;

                M_Service checker = (from c in db.M_Service
                                     where c.ServiceName == data.ServiceName
                                     && c.ServiceCategory == data.ServiceCategory
                                     && c.Price == data.Price
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
                M_Service dataval = new M_Service();
                dataval = (from u in db.M_Service.ToList()
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

        [HttpPost]
        public ActionResult UploadServicePhoto()
        {
            #region Save to Server
            bool isSuccess = false;
            string serverMessage = string.Empty;
            var fileOne = Request.Files[0] as HttpPostedFileBase;
            string uploadPath = Server.MapPath(@"~/PictureResources/ServicePhoto/");
            string newFileOne = Path.Combine(uploadPath, fileOne.FileName);
            fileOne.SaveAs(HttpContext.Server.MapPath("~/PictureResources/ServicePhoto/") + Path.GetFileName(Regex.Replace(fileOne.FileName, @"\s+", "")));

            #endregion

            return Json(new { itemName = Path.GetFileName(Regex.Replace(fileOne.FileName, @"\s+", "")) }, JsonRequestBehavior.AllowGet);
        }
    }
}