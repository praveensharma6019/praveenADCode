using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Bunkering.Website.Models
{
    public class BunkeringRequestQuoteModel
    {
        public string CompanyName { set; get; }
        public string ContactPerson { set; get; }
        public string Mobile { set; get; }
        public string Email { set; get; }
        public string ProductType { set; get; }
        public string ProductSpec { set; get; }
        public decimal ProductQuantity { set; get; }
        public string VesselName { set; get; }
        public string Port { set; get; }
        public string Berth { set; get; }
        public string Estimated_DOA { set; get; }
        public string Estimated_TOA { set; get; }
        public string Estimated_DOD { set; get; }
        public string Estimated_TOD { set; get; }
        public string Agent { set; get; }
        public string OtherDetails { set; get; }
        public string FormType { set; get; }
        public string PageInfo { get; set; }
        public DateTime FormSubmitOn { set; get; }
        public string OTP { get; set; }
        public string reResponse { get; set; }
    }
}