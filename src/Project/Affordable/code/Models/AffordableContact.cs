using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Sitecore.Affordable.Website.Models
{
    public class AffordableContact
    {
        public class EnquiryModel
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string mobile { get; set; }
            public string Budget { get; set; }
            public string country_code { get; set; }
            public List<SelectListItem> CountryList { get; set; }
            public string state_code { get; set; }
            public List<SelectListItem> StateList { get; set; }
            public string Remarks { get; set; }
            public string Projects_Interested__c { get; set; }
            public string PropertyLocation { set; get; }
            public string sale_type { get; set; }
            public List<SelectListItem> ProjectList { get; set; }
            public string Captcha { get; set; }
            public string OTP { get; set; }
            public string FormType { get; set; }
            public string PageInfo { get; set; }
            public DateTime FormSubmitOn { get; set; }
        }

        public class SearchSuggtions
        {
            public string Name { get; set; }
        }
    }
}