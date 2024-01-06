using System;
using System.Web;
using System.Web.SessionState;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsGMSUserSession
    {
        public static PortsGMSLoginModel PortsGMSUserSessionContext
        {
            get
            {
                return (PortsGMSLoginModel)HttpContext.Current.Session["PortsGMSUserLogin"];
            }
            set
            {
                HttpContext.Current.Session["PortsGMSUserLogin"] = value;
            }
        }

        public PortsGMSUserSession()
        {
        }
    }
}