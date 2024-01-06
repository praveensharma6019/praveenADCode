using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Marathon.Website.Validation
{
    public class CouponCodeResult
    {
        public string status { get; set; }
        public string extra { get; set; }
        public string couponTitle { get; set; }
        public string EmailDomain { get; set; }
        public string EmployeeIDSource { get; set; }
        public string RunType { get; set; }
        public string RaceCategory { get; set; }
    }
    public class ApplyCodeResponse
    {
        public string PaymentStatus { get; set; }
        public string DiscountRate { get; set; }
        public decimal FinalAmount { get; set; }
        public string RegistrationStatus { get; set; }
        public decimal AmountReceived { get; set; }
    }
}