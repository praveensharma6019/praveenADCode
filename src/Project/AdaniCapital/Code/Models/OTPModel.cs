using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniCapital.Website.Models
{
    [Serializable]
    public class OTPModel
    {
        public string Count
        {
            get;
            set;
        }

        public DateTime DateTime
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string MobileNo
        {
            get;
            set;
        }

        public string OTP
        {
            get;
            set;
        }

        public string PageInfo
        {
            get;
            set;
        }

        public bool SavedinDB
        {
            get;
            set;
        }
        public bool IsOTPvalid
        {
            get;
            set;
        }
        public string ValidityMessage
        {
            get;
            set;
        }
        public bool IsValidAttemptExceeded
        {
            get;
            set;
        }
        public string Status
        {
            get;
            set;
        }

        public OTPModel()
        {
        }
    }
}