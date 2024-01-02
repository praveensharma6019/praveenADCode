using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class SliderWithImageAndDetail
    {
        public string title { get; set; }
        public bool isAirportSelectNeeded { get; set; }

        public string imageSrc { get; set; }

       
        public string btnText { get; set; }

        public string description { get; set; }

        public string ctaUrl { get; set; }
        public string uniqueId { get; set; }
        public string mobileImage { get; set; }
        public string subTitle { get; set; }
        public TagName tagName { get; set; }
        public string videoSrc { get; set; }
        public string autoId { get; set; }
        public string linkTarget { get; set; }
        public bool isAgePopup { get; set; }
        public int displaySequence { get; set; }
        public string promoCode { get; set; }

        public GTMTags tags { get; set; }

        public ContactDetailsItems contactDetail { get; set; }
        public bool checkValidity { get; set; } = false;
        public string effectiveFrom { get; set; }
        public string effectiveTo { get; set; }

    }
    public class ContactDetails
    {
        public string name { get; set; }
        public string title { get; set; }
        public string richText { get; set; }
    }
    public class ContactDetailsItems
    {
        public ContactDetails phone { get; set; }
        public ContactDetails email { get; set; }
    }
}