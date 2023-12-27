using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class HeroSliderModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string autoid { get; set; }
        public string imagesrc { get; set; }
        public string bannerlogo { get; set; }
        public string subtitle { get; set; }
        public string uniqueid { get; set; }
        public string mobileimage { get; set; }
        public string btnText { get; set; }
        public bool isAirportSelectNeeded { get; set; }
        public string Link { get; set; }
        public string linkTarget { get; set; }
        public bool isAgePopup { get; set; }
        public string offerEligibility { get; set; }
        public int gridNumber { get; set; }
        public string cardBgColor { get; set; }
        public string listClass { get; set; }
        public GTMTags tags { get; set; }
        public bool checkValidity { get; set; } = false;
        public string effectiveFrom { get; set; }
        public string effectiveTo { get; set; }
    }
}