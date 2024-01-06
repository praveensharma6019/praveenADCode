namespace Sitecore.AGELPortal.Website
{
    using System.Web.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.AGELPortal.Website.Models;
    using Sitecore.AGELPortal.Website.Services;
    using Sitecore.Feature.Accounts.Services;
    using Sitecore.AGELPortal.Website;
    using System.Web;
    using System.Linq;
    using System;
    using System.Security.Claims;
    using Sitecore.Diagnostics;

    public class AGELRedirectUnauthenticatedAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        private AGELPortalRepository realtyRepo = new AGELPortalRepository();
        private AGELPortalDataContext rdb = new AGELPortalDataContext();

        public void OnAuthorization(System.Web.Mvc.AuthorizationContext context)
        {

            if (ClaimsPrincipal.Current.Claims.ToList().Count() > 10)
            {
                var checkClaims = ClaimsPrincipal.Current.Claims.ToList();
                Log.Info("Adani AD checkClaims Dashboard" + checkClaims, this);
                string useremail = checkClaims != null && checkClaims.Count > 0 ? checkClaims.Where(x => x.Type.Contains("upn")).FirstOrDefault().Value : "";
                var User = rdb.AGELPortalRegistrations.Where(x => x.email == useremail && x.status == true).FirstOrDefault();
                if (User != null)
                {
                    HttpContext.Current.Session["AGELPortalUser"] = User.Id.ToString();
                    HttpContext.Current.Session["AGELPortalUser1"] = User.Id;
                    HttpContext.Current.Session["AGELPortalUserName"] = User.name.ToString();
                    HttpContext.Current.Session["AGELPortalUserType"] = User.user_type.ToString();
                    HttpContext.Current.Session["Date"] = System.DateTime.Now.ToString();
                    //Log.Info("Adani AD AGELPortalUser Dashboard" + Session["AGELPortalUser"], this);
                    //Log.Info("Adani AD AGELPortalUser1 Dashboard" + Session["AGELPortalUser1"], this);
                    //Log.Info("Adani AD AGELPortalUserName Dashboard" + Session["AGELPortalUserName"], this);
                    //Log.Info("Adani AD AGELPortalUserType Dashboard" + Session["AGELPortalUserType"], this);
                }
            }
            AGELPortalRepository webAPIServices = new AGELPortalRepository();
            //HttpContext.Current.Session["AGELPortalUserName"]
            if (HttpContext.Current.Session["AGELPortalUser"] != null && webAPIServices.ValidateCurrentSession())
            {
                return;
            }
            else
            {
                var link = "/agelportal/home/login";
                context.Result = new Attributes.RedirectResultNoBody(link);
            }
            //try
            //{
            //    AGELPortalDataContext rdb = new AGELPortalDataContext();
            //    // AGELPortalRepository webAPIServices = new AGELPortalRepository();
            //    //if (Helpers.SessionHelper.AGELUserSessionContext.SessionId != null && webAPIServices.ValidateCurrentSession())

            //    Uri myUri = new Uri(context.HttpContext.Request.Url.AbsoluteUri);

            //    Guid Id = new Guid(HttpUtility.ParseQueryString(myUri.Query).Get("sessionId"));
            //    var currDate = DateTime.Now;
            //    var expireTime = System.Configuration.ConfigurationManager.AppSettings["DBSessionTimeout"];
            //    var beforeTime = currDate.AddMinutes(-double.Parse(expireTime));
            //    var user = rdb.AGELPortalUserSessions.Where(x => x.id == Id && x.created_date >= beforeTime).FirstOrDefault();
            //    //var user = "";
            //    if (user != null)
            //    {
            //        user.created_date = DateTime.Now;
            //        rdb.SubmitChanges();
            //        return;
            //    }
            //    else
            //    {

            //        var link = "/login";
            //        context.Result = new RedirectResult(link);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    var link = "/login";
            //    context.Result = new RedirectResult(link);
            //}
        }
    }
}