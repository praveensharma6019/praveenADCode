using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTrans.Website.Models
{
    public class InThisSection
    {
        public string Heading { get; set; }
        public List<InThisSectionItem> Data { get; set; }

    }


    public class InThisSectionItem
    {
        public string Heading { get; set; }
        public string Image { get; set; }
        public string ImageAltText { get; set; }
        public string CTALink { get; set; }
        public string CTAText { get; set; }
        public bool isExternalLink { get; set; }
    }
}