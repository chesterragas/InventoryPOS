using RIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RIS.Controllers
{
    public class LoginController : Controller
    {
        RISDBEntities db = new RISDBEntities();
        M_Users user = (M_Users)System.Web.HttpContext.Current.Session["user"];
        // GET: Login
        public ActionResult Login()
        {
            System.Web.HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            System.Web.HttpContext.Current.Response.AddHeader("Expires", "0");
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return View();
        }
        public class UsersLog
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool Rememberme { get; set; }
        }

        public string EncodePasswordMd5(string pass) //Encrypt using MD5    
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string    
            return BitConverter.ToString(encodedBytes).Replace("-", string.Empty);
        }

        public ActionResult LogOff()
        {
            System.Web.HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            System.Web.HttpContext.Current.Response.AddHeader("Expires", "0");
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return Json(new { result = "Out" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Authenticate(UsersLog user)
        {
            string result = "";
            try
            {
                string pass = EncodePasswordMd5(user.Password);
                M_Users check = (from c in db.M_Users
                                 where c.UserName == user.UserName
                                 && c.Password == pass
                                 && c.IsDeleted == false
                                 select c).FirstOrDefault();
              

                if (check != null)
                {
                    bool rememberme = false;
                    if (user.Rememberme)
                    {
                        rememberme = true;
                    }
                    System.Web.HttpContext.Current.Session["UserName"] = check.FirstName + ' ' + check.LastName;
                    System.Web.HttpContext.Current.Session["user"] = check;
                    FormsAuthentication.SetAuthCookie(user.UserName, true);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             user.UserName,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                             rememberme,
                             user.ToString());
                
                }
                result = (check == null) ? "Failed" : "Success";
                if (result == "Failed")
                {
                    //Error_Logs error = new Error_Logs();
                    //error.PageModule = "Login";
                    //error.ErrorLog = "Incorrect Username or Password";
                    //error.DateLog = db.TT_GETTIME().FirstOrDefault();//DateTime.Now;;
                    //error.Username = user.UserName;
                    //db.Error_Logs.Add(error);
                    //db.SaveChanges();
                }

                string urlmail = (Session["urlmail"] != null) ? Session["urlmail"].ToString() : "/Home/Index";
                return Json(new { result = result, urlmail = urlmail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
               
                return Json(new { result = result, urlmail = "" }, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult ChangePassword(UserPass pass)
        {
            M_Users currentuser = user;

            if(user.Password == EncodePasswordMd5(pass.Password))
            {
                user.Password = EncodePasswordMd5(pass.NewPassword);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { msg = "Success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { msg = "Failed" }, JsonRequestBehavior.AllowGet);
            }
            

           
        }
    }
}