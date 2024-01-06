using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Transmission.Website.Models
{
    public class TransmissionCostCalculatorRegistration
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Mobile { set; get; }
        public string Mobile_Verified { set; get; }
        public string Email_Verified { set; get; }
        public string Company { set; get; }
        public string INCOMPLETEREGISTRATION { set; get; }
        public string Message { set; get; }
        public string FormType { set; get; }
        public string PageInfo { get; set; }
        public string reResponse { get; set; }
        public DateTime FormSubmitOn { set; get; }
        public string OTP { get; set; }
    }
}