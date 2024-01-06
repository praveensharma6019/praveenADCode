using System;
using System.Web;
using System.Web.SessionState;

namespace Sitecore.TrivandrumAirport.Website.Model
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

        public TenderUserSession()
        {
        }
    }
}