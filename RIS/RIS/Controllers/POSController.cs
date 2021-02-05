using RIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RIS.Controllers.SessionExpire;

namespace RIS.Controllers
{
    public class POSController : Controller
    {
        RISDBEntities db = new RISDBEntities();
        M_Users user = (M_Users)System.Web.HttpContext.Current.Session["user"];
        // GET: POS
        public ActionResult POS()
        {
            return View();
        }

        public ActionResult GetItemList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<GET_POS_ItemServiceList_Result> list = db.GET_POS_ItemServiceList(start,length).ToList();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                try
                {
                    list = list.Where(x => x.ItemName.ToLower().Contains(searchValue.ToLower())
                                       ).ToList<GET_POS_ItemServiceList_Result>();
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
            list = list.Skip(start).Take(length).ToList<GET_POS_ItemServiceList_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetReceipt(List<POS_Receipt> receiptlist)
        {
            string ReceiptRefNo = "REFNO"+DateTime.Now.ToString("yyyyMMddHHmmss");
            foreach(POS_Receipt s in receiptlist)
            {
                s.ReceiptRefNo = ReceiptRefNo;
                s.CreateID = "POS";
                s.CreateDate = DateTime.Now;
                db.POS_Receipt.Add(s);
                db.SaveChanges();
            }

            List<POS_Receipt> ItemList = new List<POS_Receipt>();
            ItemList = receiptlist.Where(x => x.Type == "Item").ToList();
            foreach (POS_Receipt s in ItemList)
            {
                M_Item item = new M_Item();
                item = (from c in db.M_Item where c.ID == s.ItemID select c).FirstOrDefault();
                item.CurrentStock -= s.Amount;
                db.Entry(item).State = EntityState.Modified;
               
                M_Stocks stock = new M_Stocks();
                stock.ItemID = s.ID;
                stock.StockType = "Out";
                stock.StockQty = s.Amount;
                stock.Remarks = string.Empty;
                stock.Price = item.Price;
                stock.UpdateID = "POS";
                stock.UpdateDate = DateTime.Now;
                db.M_Stocks.Add(stock);
                db.SaveChanges();
            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}