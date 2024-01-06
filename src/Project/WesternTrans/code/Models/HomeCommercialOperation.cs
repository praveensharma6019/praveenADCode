using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTrans.Website.Models
{
    public class HomeCommercialOperation
    {
        public string Heading { get; set; }
        public string CTAText { get; set; }
        public string CTALink { get; set; }
        public bool isExternalLink { get; set; }
        public List<HomeCommercialOperationItems> Data { get;set; }
    }

    
}