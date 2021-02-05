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
    public class EmployeeController : Controller
    {
        RISDBEntities db = new RISDBEntities();
        M_Users user = (M_Users)System.Web.HttpContext.Current.Session["user"];
        // GET: Masters/Employee
        public ActionResult Employee()
        {
            return View();
        }

        public ActionResult GetEmployeeList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<GET_M_EmployeeList_Result> list = db.GET_M_EmployeeList().ToList();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                try
                {
                    list = list.Where(x => x.First_Name.ToLower().Contains(searchValue.ToLower())
                                       ).ToList<GET_M_EmployeeList_Result>();
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
            list = list.Skip(start).Take(length).ToList<GET_M_EmployeeList_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CreateEmployee(M_Employee data)
        {
            try
            {
                data.CreateID = user.CreateID;
                data.CreateDate = DateTime.Now;
                data.UpdateID = user.CreateID;
                data.UpdateDate = DateTime.Now;
                data.IsDeleted = false;
               
                M_Employee checker = (from c in db.M_Employee
                                      where c.Family_Name == data.Family_Name
                                       && c.First_Name == data.First_Name
                                       && c.DailyWage == data.DailyWage
                                       && c.IsDeleted == false
                                      select c).FirstOrDefault();
                if (checker == null)
                {
                    db.M_Employee.Add(data);
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

        public ActionResult EditEmployee(M_Employee data)
        {
            try
            {

                M_Employee dataval = new M_Employee();
                dataval = (from u in db.M_Employee.ToList()
                           where u.ID == data.ID
                           select u).FirstOrDefault();
                dataval.Family_Name = data.Family_Name;
                dataval.First_Name = data.First_Name;
                dataval.DailyWage = data.DailyWage;
              
                dataval.UpdateID = user.UserName;
                dataval.UpdateDate = DateTime.Now;

                M_Employee checker = (from c in db.M_Employee
                                      where c.Family_Name == data.Family_Name
                                       && c.First_Name == data.First_Name
                                       && c.DailyWage == data.DailyWage
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

        public ActionResult DeleteEmployee(int ID)
        {
            try
            {
                M_Employee dataval = new M_Employee();
                dataval = (from u in db.M_Employee.ToList()
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