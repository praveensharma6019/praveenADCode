using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Sitecore.InspireBusinessPark.Website.Models
{
    public class Inspire
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string City { get; set; }
        public string FormType { get; set; }
        public string reResponse { get; set; }
        public string PageInfo { get; set; }
        public DateTime FormSubmitOn { get; set; }
        public string LeadSource { get; set; }
        public string UTMSource { get; set; }
    }
    
}
    