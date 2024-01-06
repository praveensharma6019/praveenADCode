using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Electricity.Website.Model
{
    [Serializable]
    public class TenderUserSession
    {
        public static TenderLoginModel TenderUserSessionContext
        {
            get
            {
                return (TenderLoginModel)HttpContext.Current.Session["TenderUserLogin"];
            }
            set
            {
                HttpContext.Current.Session["TenderUserLogin"] = value;
            }
        }
    }
}