using RIS.Controllers;
using RIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RIS.Controllers.SessionExpire;

namespace RIS.Areas.Transaction.Controllers
{
    [SessionExpire]
    public class InboundOutboundHistoryController : Controller
    {
        RISDBEntities db = new RISDBEntities();
        M_Users user = (M_Users)System.Web.HttpContext.Current.Session["user"];
        // GET: Transaction/InboundOutboundHistory
        public ActionResult InboundOutboundHistory()
        {
            return View();
        }

        public ActionResult GetInboundOutboundList(long? Category, string Type)
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<GET_T_InboundOutboundHistory_Result> list = db.GET_T_InboundOutboundHistory(Category,Type).ToList();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                try
                {
                    list = list.Where(x => x.ItemName.ToLower().Contains(searchValue.ToLower())
                                           || x.CategoryName.ToLower().Contains(searchValue.ToLower())
                                           || x.Remarks.ToLower().Contains(searchValue.ToLower())
                                       ).ToList<GET_T_InboundOutboundHistory_Result>();
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
            list = list.Skip(start).Take(length).ToList<GET_T_InboundOutboundHistory_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
    }
}