using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class SiteMapXML
    {
        public string Url { get; set; }
        public string Priority { get; set; }
        public string Lastmod { get; set; }
        public double DescPriority { get; set; }
    }

}