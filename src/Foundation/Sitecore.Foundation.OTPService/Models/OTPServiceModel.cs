using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.OTPService.Models
{
    public class OTPServiceModel
    {
        public string Name { get; set; }
        public string OTP { get; set; }
        public bool Status { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string OTPSitecoreDatasource { get; set; }
    }
}