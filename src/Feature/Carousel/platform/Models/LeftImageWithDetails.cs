using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class LeftImageWithDetails
    {
        public string Image { get; set; }

        public string name { get; set; }

        public string backgroundColor { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string textColor { get; set; }

        public string Link { get; set; }

        public string Video { get; set; }

        public string MobileVideo { get; set; }

        public string AutoId { get; set; }

        public string UniqueId { get; set; }

        public string MobileImage { get; set; }

        public bool isAirportSelectNeeded { get; set; }

        public string linkTarget { get; set; }

        public bool isAgePopup { get; set; }

        public string OfferEligibility { get; set; }

        public GTMTags tags { get; set; }
    }
}