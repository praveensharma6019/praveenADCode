using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;


namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class HeroCarouselwidgets
    {
        public WidgetItem widget { get; set; }
        //  public List<HeroCarousel> widgetItems { get; set; }
    }
    /// <summary>
    /// Class to get the Material Groups
    /// </summary>
    public class HeroCarousel
    {
        public string title { get; set; }
        public bool isAirportSelectNeeded { get; set; }
        public bool disableForAirport { get; set; }
        public string imageSrc { get; set; }

        public string description { get; set; }

        public string descriptionApp { get; set; }

        public string ctaLink { get; set; }

        public string appCtaLink { get; set; }

        public string deepLink { get; set; }

        public string subTitle { get; set; }

        public string materialGroup { get; set; }
        public string category { get; set; }
        public string subCategory { get; set; }
        public string brand { get; set; }
        public string skuCode { get; set; }
        //  public string airportCode { get; set; }
        public string storeType { get; set; }
        public bool restricted { get; set; }
        public string mobileImage { get; set; }
        public string webImage { get; set; }
        public string thumbnailImage { get; set; }
        public string bannerCondition { get; set; }
        public string ctaText { get; set; }
        public string uniqueId { get; set; }
        public string ctaUrl { get; set; }
        public string linkTarget { get; set; }
        public string autoId { get; set; }
        public bool isAgePopup { get; set; }
        public string promoCode { get; set; }
        public GTMTags tags { get; set; }
        public bool checkValidity { get; set; } = false;
        public string effectiveFrom { get; set; }
        public string effectiveTo { get; set; }
    }

}