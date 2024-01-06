using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Affordable.Website.Models
{
    public class AffordableFormModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Budget { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Remarks { get; set; }
        public string Property_Interest { get; set; }
        public string FormType { get; set; }
        public string PageInfo { get; set; }
        public DateTime FormSubmitOn { get; set; }
        public string OTP { get; set; }
    }
}