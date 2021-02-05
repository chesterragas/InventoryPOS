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
    public class UsersController : Controller
    {
        RISDBEntities db = new RISDBEntities();
        M_Users user = (M_Users)System.Web.HttpContext.Current.Session["user"];
        // GET: Masters/Users
        public ActionResult Users()
        {
            return View();
        }


        public ActionResult GetUsersList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            
            List<M_Users> list = db.M_Users.Where(x=>x.IsDeleted != true).ToList();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                try
                {
                    list = list.Where(x => x.UserName.ToLower().Contains(searchValue.ToLower())
                                        || x.LastName.ToLower().Contains(searchValue.ToLower())
                                        || x.FirstName.ToLower().Contains(searchValue.ToLower())).ToList<M_Users>();
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
            list = list.Skip(start).Take(length).ToList<M_Users>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateUser(M_Users data)
        {
            try
            {
                data.CreateID = user.UserName;
                data.CreateDate = DateTime.Now;
                data.UpdateID = user.UserName;
                data.UpdateDate = DateTime.Now;
                data.IsDeleted = false;

                M_Users checker = (from c in db.M_Users
                                   where c.UserName == data.UserName
                                    && c.IsDeleted == false
                                    select c).FirstOrDefault();
                if (checker == null)
                {
                    db.M_Users.Add(data);
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

        public ActionResult EditUser(M_Users data)
        {
            try
            {
                M_Users dataval = new M_Users();
                dataval = (from u in db.M_Users.ToList()
                            where u.ID == data.ID
                            select u).FirstOrDefault();
                dataval.UserName = data.UserName;
                dataval.FirstName = data.FirstName;
                dataval.LastName = data.LastName;
                dataval.Email = data.Email;
                dataval.UpdateID = user.UserName;
                dataval.UpdateDate = DateTime.Now;

                M_Users checker = (from c in db.M_Users
                                   where c.UserName == data.UserName
                                    && c.FirstName == data.FirstName
                                    && c.LastName == data.LastName
                                    && c.Email == data.Email
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

        public ActionResult DeleteUser(int ID)
        {
            try
            {
                M_Users dataval = new M_Users();
                dataval = (from u in db.M_Users.ToList()
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
    }
}