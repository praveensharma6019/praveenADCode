using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Electricity.Website.Model
{
    [Serializable]
    public class ITSRUserSession
    {
        public static ITSRLoginModel ITSRUserSessionContext
        {
            get
            {
                return (ITSRLoginModel)HttpContext.Current.Session["ITSRUserLogin"];
            }
            set
            {
                HttpContext.Current.Session["ITSRUserLogin"] = value;
            }
        }
    }
}