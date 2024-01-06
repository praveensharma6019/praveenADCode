using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.BelvedereClubAhmedabad.Website.Models
{
    public class ClubAhmedabadEnquiryModel
    {
        public string Name { set; get; }
        public string Mobile { set; get; }
        public string Email { set; get; }
       public string InterestedIn { set; get; }
        public string City { set; get; }
        public string Message { set; get; }
        public string FormType { set; get; }
        public DateTime FormSubmitOn { set; get; }
        public string OTP { get; set; }
    }
}