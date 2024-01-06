using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class AdaniGasOTPModel
    {
        public string OTP_Validity_Minutes { get; set; }
        public string OTP { get; set; }
        public string MobileNo { get; set; }
        public string BPUA { get; set; }
        public bool IsOTPSent { get; set; }
        public bool IsError { get; set; }
        public string ReturnBPUA { get; set; }
        public string MessageFlag { get; set; }
        public string Message { get; set; }

    }
}