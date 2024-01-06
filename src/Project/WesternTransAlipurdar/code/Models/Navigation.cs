using System.Collections.Generic;

namespace Sitecore.WesternTransAlipurdar.Website.Models
{
    public class Navigation
    {
        public string CTALink { get; set; }
        public string CTAText { get; set; }
        public bool isExternalLink { get; set; }
        public string Image { get; set; }
        public string ImageAltText { get; set; }
        public string Description { get; set; }
        public List<SubNavigation> SubNavigation { get; set; }
    }
}