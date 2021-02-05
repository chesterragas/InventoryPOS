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
    public class ItemController : Controller
    {
        RISDBEntities db = new RISDBEntities();
        M_Users user = (M_Users)System.Web.HttpContext.Current.Session["user"];
        // GET: Masters/Item
        public ActionResult Item()
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

        [HttpPost]
        public ActionResult CreateItem(M_Item data)
        {
            try
            {
                data.CreateID = user.CreateID;
                data.CreateDate = DateTime.Now;
                data.UpdateID = user.CreateID;
                data.UpdateDate = DateTime.Now;
                data.IsDeleted = false;
                data.CurrentStock = 0;
                M_Item checker = (from c in db.M_Item
                                          where c.ItemName == data.ItemName
                                           && c.ItemCategory == data.ItemCategory
                                           && c.Price == data.Price
                                           && c.RetailPrice == data.RetailPrice
                                           && c.IsDeleted == false
                                  select c).FirstOrDefault();
                if (checker == null)
                {
                    db.M_Item.Add(data);
                    db.SaveChanges();

                    M_Stocks initialStock = new M_Stocks();
                    initialStock.ItemID = (from c in db.M_Item orderby c.ID descending select c.ID).FirstOrDefault();
                    initialStock.StockQty = 0;
                    initialStock.StockType = "Initial";
                    initialStock.UpdateID = user.UserName;
                    initialStock.UpdateDate = DateTime.Now;
                    db.M_Stocks.Add(initialStock);
                    db.SaveChanges();

                    long CurrentID = (from c in db.M_Item orderby c.ID select c.ID).FirstOrDefault();

                    return Json(new { msg = "Success", itemID= CurrentID }, JsonRequestBehavior.AllowGet);
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

        public ActionResult EditItem(M_Item data)
        {
            try
            {
                
                M_Item dataval = new M_Item();
                dataval = (from u in db.M_Item.ToList()
                           where u.ID == data.ID
                           select u).FirstOrDefault();
                dataval.ItemCategory = data.ItemCategory;
                dataval.ItemName = data.ItemName;
                dataval.Price = data.Price;
                dataval.Photo = data.Photo;
                dataval.RetailPrice = data.RetailPrice;

                dataval.UpdateID = user.UserName;
                dataval.UpdateDate = DateTime.Now;

                M_Item checker = (from c in db.M_Item
                                  where c.ItemName == data.ItemName
                                  && c.ItemCategory == data.ItemCategory
                                  && c.Price == data.Price
                                  && c.RetailPrice == data.RetailPrice
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

        public ActionResult DeleteItem(int ID)
        {
            try
            {
                M_Item dataval = new M_Item();
                dataval = (from u in db.M_Item.ToList()
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


        //public ActionResult UpdateStocks(int ID, long Amount, string StockType)
        //{
        //    M_Stocks stocks = new M_Stocks();
        //    M_Item item = new M_Item();
        //    item = (from c in db.M_Item where c.ID == ID select c).FirstOrDefault();

        //    if (StockType == "Deduct" && Amount > item.CurrentStock)
        //    {
        //        return Json(new { msg = "Failed" }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        switch (StockType)
        //        {
        //            case "Add":
        //                stocks.ItemID = ID;
        //                stocks.StockQty = Amount;
        //                stocks.UpdateDate = DateTime.Now;
        //                stocks.UpdateID = user.UserName;
        //                stocks.StockType = "In";
        //                db.M_Stocks.Add(stocks);



        //                item.CurrentStock += Amount;
        //                db.Entry(item).State = EntityState.Modified;
        //                db.SaveChanges();


        //                break;
        //            case "Deduct":
        //                stocks.ItemID = ID;
        //                stocks.StockQty = Amount;
        //                stocks.UpdateDate = DateTime.Now;
        //                stocks.UpdateID = user.UserName;
        //                stocks.StockType = "Out";
        //                db.M_Stocks.Add(stocks);


        //                item.CurrentStock -= Amount;
        //                db.Entry(item).State = EntityState.Modified;
        //                db.SaveChanges();

        //                break;
        //        }



        //        return Json(new { msg = "Success" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public ActionResult UploadItemPhoto()
        {
            #region Save to Server
            bool isSuccess = false;
            string serverMessage = string.Empty;
            var fileOne = Request.Files[0] as HttpPostedFileBase;
            string uploadPath = Server.MapPath(@"~/PictureResources/ItemPhoto/");
            string newFileOne = Path.Combine(uploadPath, fileOne.FileName);
            fileOne.SaveAs(HttpContext.Server.MapPath("~/PictureResources/ItemPhoto/") + Path.GetFileName(Regex.Replace(fileOne.FileName, @"\s+", "")));
            
            #endregion
            
            return Json(new { itemName = Path.GetFileName(Regex.Replace(fileOne.FileName, @"\s+", "")) }, JsonRequestBehavior.AllowGet);
        }
    }
}