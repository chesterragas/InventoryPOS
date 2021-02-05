using RIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RIS.Controllers.SessionExpire;

namespace RIS.Areas.Transaction.Controllers
{
    public class InboundOutboundController : Controller
    {
        RISDBEntities db = new RISDBEntities();
        M_Users user = (M_Users)System.Web.HttpContext.Current.Session["user"];
        // GET: Transaction/InboundOutbound
        public ActionResult InboundOutbound()
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

            List<GET_M_ItemList_Result> list = db.GET_M_ItemList().ToList();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                try
                {
                    list = list.Where(x => x.ItemName.ToLower().Contains(searchValue.ToLower())
                                       ).ToList<GET_M_ItemList_Result>();
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
            list = list.Skip(start).Take(length).ToList<GET_M_ItemList_Result>();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateStocks(int ID, long Amount, string StockType)
        {
            M_Stocks stocks = new M_Stocks();
            M_Item item = new M_Item();
            item = (from c in db.M_Item where c.ID == ID select c).FirstOrDefault();

            if (StockType == "Deduct" && Amount > item.CurrentStock)
            {
                return Json(new { msg = "Failed" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                switch (StockType)
                {
                    case "Add":
                        stocks.ItemID = ID;
                        stocks.StockQty = Amount;
                        stocks.UpdateDate = DateTime.Now;
                        stocks.UpdateID = user.UserName;
                        stocks.StockType = "In";
                        stocks.Price = item.Price;
                        db.M_Stocks.Add(stocks);



                        item.CurrentStock += Amount;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();


                        break;
                    case "Deduct":
                        stocks.ItemID = ID;
                        stocks.StockQty = Amount;
                        stocks.UpdateDate = DateTime.Now;
                        stocks.UpdateID = user.UserName;
                        stocks.Price = item.Price;
                        stocks.StockType = "Out";
                        db.M_Stocks.Add(stocks);


                        item.CurrentStock -= Amount;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();

                        break;
                }



                return Json(new { msg = "Success" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}