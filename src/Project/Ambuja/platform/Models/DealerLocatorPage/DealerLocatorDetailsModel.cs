using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.DealerLocatorPage
{
    public class DealerLocatorDetailsModel
    {
        public List<LocatorDetails> details { get; set; }
    }
    public class LocatorDetails
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public string Type { get; set; }
        public List<city> cityOptions { get; set; }
    }
    public class city
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public string Type { get; set; }
        public List<area> areaOptions { get; set; }
    }
    public class area
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public string Type { get; set; }
    }
}