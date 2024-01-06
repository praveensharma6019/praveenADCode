using System.Collections.Generic;

namespace Sitecore.WesternTransAlipurdar.Website.Models
{
    public class SocialLinks
    {
        public string Heading { get; set; }
      
        public List<SocialLinksItems> SocialLinksItems { get; set; }
    }
    public class SocialLinksItems
    {
        public string Link { get; set; }
        public string Image { get; set; }
        public string ImageAltText { get; set; }
        public bool isExternalLink { get; set; }
    }
}