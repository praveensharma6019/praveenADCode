using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Realty.Website.Model
{
    public class RealtyFilter
    {
        public string qry { get; set; }
        public string project { get; set; }
        public string type { get; set; }
        public List<SelectListItem> PropertyTypeList { get; set; }

        public string status { get; set; }
        public List<SelectListItem> ProjectStatusList { get; set; }

        public string minarea { get; set; }
        public List<SelectListItem> MinimumAreaList { get; set; }

        public string maxarea { get; set; }
        public List<SelectListItem> MaximumAreaList { get; set; }

        public string minbudget { get; set; }
        public List<SelectListItem> MinimumBudgetList { get; set; }

        public string maxbudget { get; set; }
        public List<SelectListItem> MaximumBudgetList { get; set; }
    }
}