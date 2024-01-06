using Sitecore.Power.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Power.Website.Model
{
    [Serializable]
    public class AdaniPowerUserSession
    {
        public static AdaniPowerTenderModel TenderUserSessionContext
        {
            get
            {
                return (AdaniPowerTenderModel)HttpContext.Current.Session["TenderUserLogin"];
            }
            set
            {
                HttpContext.Current.Session["TenderUserLogin"] = value;
            }
        }
    }
}