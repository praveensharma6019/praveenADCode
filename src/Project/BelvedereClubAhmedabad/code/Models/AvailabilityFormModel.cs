using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.BelvedereClubAhmedabad.Website.Models
{
    public class AvailabilityFormModel
    {
        public string Name { set; get; }
        public DateTime BookDateFrom { set; get; }
        public DateTime BookDateTo { set; get; }
        public int NoOfRoom { set; get; }
        public int NoOfAdults { set; get; }
        public int NoOfKids { set; get; }
        public string Email { set; get; }
        public string Mobile { set; get; }
        public DateTime FormSubmitOn { set; get; }
        public string OTP { get; set; }


    }
}