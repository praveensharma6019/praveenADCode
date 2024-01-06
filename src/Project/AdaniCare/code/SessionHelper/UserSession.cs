using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniCare.Website.Models
{
    [Serializable]
    public class UserSession
    {
        public static Models.AdaniCareConsumerDetails UserSessionContext
        {
            get
            {
                return (Models.AdaniCareConsumerDetails)HttpContext.Current.Session["UserLogin"];
            }
            set
            {
                HttpContext.Current.Session["UserLogin"] = value;
            }
        }
    }
}