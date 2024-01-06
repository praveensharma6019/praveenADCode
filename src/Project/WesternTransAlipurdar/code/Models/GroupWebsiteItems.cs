using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTransAlipurdar.Website.Models
{
    public class GroupWebsiteItems
    {
        public string Heading { get; set; }
        public string Image { get; set; }
        public string ImageAltText { get; set; }
        public string Link { get; set; }
        public bool isExternalLink { get; set; }
    }
}