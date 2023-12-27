using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class MoreServices
    {
        public string imageSrc { get; set; }
        public bool isAirportSelectNeeded { get; set; }
       
        public string btnText { get; set; }

        public string title { get; set; }
        public string ctaUrl { get; set; }
        public string uniqueId { get; set; }
        public string mobileImage { get; set; }
        public string autoId { get; set; }
        public string linkTarget { get; set; }
        public bool isAgePopup { get; set; }
        public TagName tagName { get; set; }
        public GTMTags tags { get; set; }
    }
}