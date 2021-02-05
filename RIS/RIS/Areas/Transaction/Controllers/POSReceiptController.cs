using RIS.Controllers;
using RIS.Models;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RIS.Controllers.SessionExpire;

namespace RIS.Areas.Transaction.Controllers
{
    [SessionExpire]
    public class POSReceiptController : Controller
    {
        RISDBEntities db = new RISDBEntities();
        M_Users user = (M_Users)System.Web.HttpContext.Current.Session["user"];
        // GET: Transaction/POSReceipt
        public ActionResult POSReceipt()
        {
            return View();
        }


        public ActionResult GetPosList(DateTime? DateFrom, DateTime? DateTo)
        {
            DateFrom = (DateFrom == null) ? Convert.ToDateTime(DateTime.Now.ToShortDateString()) : DateFrom;
            DateTo = (DateTo == null) ? DateTime.Now : DateTo;
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<GET_POS_ReceiptList_Result> list = db.GET_POS_ReceiptList(DateFrom, DateTo).ToList();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                try
                {
                    list = list.Where(x => x.ReceiptRefNo.ToLower().Contains(searchValue.ToLower())
                                       ).ToList<GET_POS_ReceiptList_Result>();
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
            list = list.Skip(start).Take(length).ToList<GET_POS_ReceiptList_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPosListDetails(string ReceiptNo)
        {
           
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            ObjectParameter totals = new ObjectParameter("Total", typeof(decimal));
            List<GET_POS_ItemServiceList_details_Result> list = db.GET_POS_ItemServiceList_details(ReceiptNo, totals).ToList();
            int? totalsvalue = Convert.ToInt32(totals.Value);//list.Count;
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                try
                {
                    list = list.Where(x => x.ItemName.ToLower().Contains(searchValue.ToLower())
                                       ).ToList<GET_POS_ItemServiceList_details_Result>();
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
            list = list.Skip(start).Take(length).ToList<GET_POS_ItemServiceList_details_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

    }
}