using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class BestSellerDutyFree
    {
        public string ImageSrc { get; set; }

        public bool isAirportSelectNeeded { get; set; }
        public string Title { get; set; }

        public string Price { get; set; }

        public string Amount { get; set; }
        public string OfferText { get; set; }
        public string UniqueId { get; set; }
        public string MobileImage { get; set; }
        public string SKUCode { get; set; }
        public string StoreType { get; set; }
        public string apiUrl { get; set; }

    }
}