using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTransAlipurdar.Website.Models
{
    public class Cookie
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public string CTAText { get; set; }
        public string CTALink { get; set; }
        public bool isExternalLink { get; set; }
    }
}