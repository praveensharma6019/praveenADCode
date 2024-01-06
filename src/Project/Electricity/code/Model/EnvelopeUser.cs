using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Electricity.Website.Model
{
    public class EnvelopeUser
    {
        public List<SelectListItem> TenderList { get; set; }
        public List<SelectListItem> EnvelopeList { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string Mobile { get; set; }
    }
}