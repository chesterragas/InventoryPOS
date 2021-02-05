using RIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RIS.Controllers
{
    public class SessionExpire : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            M_Users user = (M_Users)HttpContext.Current.Session["user"];
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (user == null)
            {

                System.Web.HttpContext.Current.Session["urlmail"] = url;
                filterContext.Result = new RedirectResult("/Login/Login");
                return;
            }
           
            base.OnActionExecuting(filterContext);


        }

        public static class TypeHelper
        {
            public static object GetPropertyValue(object obj, string name)
            {
                return obj == null ? null : obj.GetType()
                .GetProperty(name)
                .GetValue(obj, null);
            }
        }
    }
}