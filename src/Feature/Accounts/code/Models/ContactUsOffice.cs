using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models
{
    public class ContactUsOffice
    {
        public List<SelectListItem> CityList { get; set; }

        public List<SelectListItem> StateList { get; set; }

        public List<companyDetails> companylist { get; set; }

       public string selectedState { get; set; }

        public string selectedCity { get; set; }

    }

    public class companyDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public string city { get; set; }

        public string state { get; set; }
    }
}