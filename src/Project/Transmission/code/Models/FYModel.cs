using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Transmission.Website.Models
{
    public class FYModellist
    {
        public List<FYModel> lstFyModel { get; set; }
    }
    public class FYModel
    {
        public string year { get; set; }
        public string value { get; set; }
        public string offsetValue { get; set; }
    }
}