using Sitecore.AGELPortal.Website;
using Sitecore.AGELPortal.Website.Models;
using System;
using System.Web;
using System.Web.SessionState;

namespace Sitecore.AGELPortal.Website.Models
{
    [Serializable]
    public class UserSession
    {
        public static string UserSessionContext
        {
            get
            {
                return HttpContext.Current.Session["AGELPortalUserName"].ToString();
            }
            set
            {
                HttpContext.Current.Session["AGELPortalUserName"] = value;
            }
        }
    }
}