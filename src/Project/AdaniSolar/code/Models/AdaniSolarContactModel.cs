using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniSolar.Website.Models
{
    public class AdaniSolarContactModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string MessageType { get; set; }
        public string Mobile { get; set; }
        public string CountryCode { get; set; }
        public string Category { get; set; }
        public string Region { get; set; }
        public string State { get; set; }
        public string ProjectCategory { get; set; }
        public string Message { get; set; }
        public string FormType { get; set; }
        public string PageInfo { get; set; }
        public DateTime FormSubmitOn { get; set; }

        public string emailMessage { get; set; }
        public string Response { get; set; }
        public string topic { get; set; }
        public string SubjectOfMessageText { get; set; }
        public string CategoryText { get; set; }
        public string StateText { get; set; }
    }

    public class Territories
    {
        public string name { get; set; }
        public string territoryid { get; set; }
    }

    public class Countries
    {
        public string ispl_name { get; set; }
        public string ispl_countryid { get; set; }
    }

    public class CRMObject
    {
        public string firstname { get; set; }
        public string emailaddress1 { get; set; }
        public string ispl_category { get; set; }
        public string mobilephone { get; set; }
        public string subject { get; set; }
        public string ispl_websitecountry { get; set; }
        public string ispl_websitemodulecategory { get; set; }
        public string ispl_websitestate { get; set; }
        public string ispl_websiteregion { get; set; }
        public string leadsourcecode { get; set; }
        public string description { get; set; }
    }
}