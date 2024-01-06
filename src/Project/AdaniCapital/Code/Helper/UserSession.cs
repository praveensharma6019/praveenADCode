using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniCapital.Website.Helper
{
    public class UserSession
    {
        public static Models.UserLoginModel UserSessionContext
        {
            get
            {
                return (Models.UserLoginModel)HttpContext.Current.Session["UserLogin"];
            }
            set
            {
                HttpContext.Current.Session["UserLogin"] = value;
            }
        }
    }
}