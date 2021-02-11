using RIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RIS.Controllers.SessionExpire;

namespace RIS.Controllers
{
    public class EmployeeAttendanceController : Controller
    {

        RISDBEntities db = new RISDBEntities();
        M_Users user = (M_Users)System.Web.HttpContext.Current.Session["user"];
        // GET: EmployeeAttendance
        public ActionResult EmployeeAttendance()
        {
            return View();
        }

        public ActionResult GetServerDate()
        {
            string thed = db.GETTIME().FirstOrDefault();
            return Json(new { thed = thed }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeDataList(string datechosen)
        {
            if(datechosen == null || datechosen == "")
            {
                datechosen = db.GETTIME().FirstOrDefault();
            }
            List<GET_M_EmployeeAttendanceList_Result> list = db.GET_M_EmployeeAttendanceList(datechosen).ToList();
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClearData(string datechosen)
        {
            db.Delete_EmployeeLogs(datechosen);
            return Json(new { msg = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateEmployeeLogs(List<long> data)
        {
            try
            {
                db.Delete_EmployeeLogs(db.GETTIME().FirstOrDefault());
                foreach (long datas in data)
                {
                   
                    M_Employee a = (from c in db.M_Employee where c.ID == datas select c).FirstOrDefault();
                    T_EmployeeLogs logs = new T_EmployeeLogs();
                    logs.CreateDate = DateTime.Now;
                    logs.CreateID = user.UserName;
                    logs.UpdateDate = DateTime.Now;
                    logs.UpdateID = user.UserName;
                    
                    logs.DateAttendance = db.GETTIME().FirstOrDefault();
                    logs.EmployeeID = datas;
                    logs.Wages = a.DailyWage;

                    

                    db.T_EmployeeLogs.Add(logs);
                    db.SaveChanges();
                }

                return Json(new { msg = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {

                return Json(new { msg = err.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}