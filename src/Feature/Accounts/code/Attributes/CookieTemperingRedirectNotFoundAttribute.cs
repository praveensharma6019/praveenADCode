using Sitecore.Feature.Accounts.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Attributes
{
    public class CookieTemperingRedirectNotFoundAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            /*if (filterContext.HttpContext.Request.Cookies["ACR"] != null
                && !string.IsNullOrEmpty(filterContext.HttpContext.Request.Cookies["ACR"].Value))
            {
                if (filterContext.HttpContext.Request.Cookies["ASP.NET_SessionId"] == null
                    || string.IsNullOrEmpty(filterContext.HttpContext.Request.Cookies["ASP.NET_SessionId"].Value)
                    || filterContext.HttpContext.Request.Cookies["ASP.NET_SessionId"].Value != filterContext.HttpContext.Request.Browser.ToHashString())
                {
                    filterContext.Result = new RedirectResult("/NotFound");
                }
            }*/
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            /*if (filterContext.HttpContext.Request.Cookies["ACR"] == null)
            {
                filterContext.HttpContext.Response.Cookies.Add(new HttpCookie("ACR", Guid.NewGuid().ToString()) { HttpOnly = true, Secure = true });
            }
            filterContext.HttpContext.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", filterContext.HttpContext.Request.Browser.ToHashString()) { HttpOnly = true, Secure = true });*/
            base.OnActionExecuted(filterContext);
        }
    }
}