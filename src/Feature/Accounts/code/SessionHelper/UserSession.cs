using Sitecore.Feature.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.SessionHelper
{
    [Serializable]
    public class UserSession
    {
        public static Models.DashboardModel UserSessionContext
        {
            get
            {
                return (Models.DashboardModel)HttpContext.Current.Session["UserLogin"];
            }
            set
            {
                HttpContext.Current.Session["UserLogin"] = value;
            }
        }

        public static Models.PNGLoginModel AdaniGasUserSessionContext
        {
            get
            {
                return (Models.PNGLoginModel)HttpContext.Current.Session["UserLogin"];
            }
            set
            {
                HttpContext.Current.Session["UserLogin"] = value;
            }
        }

        public static Models.ChangeOfNameLECUserLoginSessionModel AEMLCONLECUserSessionContext
        {
            get
            {
                return (Models.ChangeOfNameLECUserLoginSessionModel)HttpContext.Current.Session["LECUserLogin"];
            }
            set
            {
                HttpContext.Current.Session["LECUserLogin"] = value;
            }
        }

        public static Models.ChangeOfNameLECUserLoginSessionForOTPModel AEMLCONLECUserSessionOTPContext
        {
            get
            {
                return (Models.ChangeOfNameLECUserLoginSessionForOTPModel)HttpContext.Current.Session["LECUserLoginOTP"];
            }
            set
            {
                HttpContext.Current.Session["LECUserLoginOTP"] = value;
            }
        }

        public static Models.LoginInfoComplaint AEMLComplaintUserSessionContext
        {
            get
            {
                return (Models.LoginInfoComplaint)HttpContext.Current.Session["ComplaintRegistrationModel"];
            }
            set
            {
                HttpContext.Current.Session["ComplaintRegistrationModel"] = value;
            }
        }
        public static Models.NewConnectionRegistrationModel AEMLNewConnectionSessionContext
        {
            get
            {
                return (Models.NewConnectionRegistrationModel)HttpContext.Current.Session["NewConnectionApplication"];
            }
            set
            {
                HttpContext.Current.Session["NewConnectionApplication"] = value;
            }
        }
        public static Models.NewConnectionApplication AEMLNewConnectionSessionUserID
        {
            get
            {
                return (Models.NewConnectionApplication)HttpContext.Current.Session["ID"];
            }
            set
            {
                HttpContext.Current.Session["ID"] = value;
            }
        }

        public static Models.AdaniGasCNGRegistration.AdaniGasCNG_DealerLoginModel CNGDealerUserSessionContext
        {
            get
            {
                return (Models.AdaniGasCNGRegistration.AdaniGasCNG_DealerLoginModel)HttpContext.Current.Session["CNGDealerUserLogin"];
            }
            set
            {
                HttpContext.Current.Session["CNGDealerUserLogin"] = value;
            }
        }
        public static Models.AdaniGasCNGRegistration.AdaniGasCNG_AdminLoginModel CNGAdminUserSessionContext
        {
            get
            {
                return (Models.AdaniGasCNGRegistration.AdaniGasCNG_AdminLoginModel)HttpContext.Current.Session["CNGAdminUserLogin"];
            }
            set
            {
                HttpContext.Current.Session["CNGAdminUserLogin"] = value;
            }
        }
        public static NameTransferAdminRegistationSessssion nameTransferAdminRegistationSessssion
        {
            get
            {
                return (NameTransferAdminRegistationSessssion)HttpContext.Current.Session["NameTransferLogin"];
            }
            set
            {
                HttpContext.Current.Session["NameTransferLogin"] = value;
            }
        }
    }
}