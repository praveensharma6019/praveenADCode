using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class RewardsModal
    {
        //Title field in Sitecore
        public string title { get; set; }
        //Description field in Sitecore
        public string description { get; set; }
        //Standered Image field in Sitecore
        public string desktopImageSrc { get; set; }
        //ImageMobile field in Sitecore
        public string mobileImageSrc { get; set; }
        //CTA field in Sitecore
        public string ctaText { get; set; }
        //CTA field in Sitecore
        public string ctaLink { get; set; }
        //Type field in Sitecore
        public string rewardType { get; set; }
        // Description App field in Sitecore
        public string descriptionApp { get; set; }
        public string rewardUniqueId { get; set; }
        //Location Field from Sitecore
        public List<string> location { get; set; }
        public bool active { get; set; }
    }
}