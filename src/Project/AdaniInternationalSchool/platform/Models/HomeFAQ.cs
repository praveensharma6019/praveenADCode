using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{

    public class HomeFAQ
    {
        public string ViewAllLink { get; set; }
        public string ViewAllLabel { get; set; }
        public string SectionHeading { get; set; }
        public string Target { get; set; }

        public GtmDataModel GtmData { get; set; }

        public List<FAQDataItem> Data { get; set; }
    }
}