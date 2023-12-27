using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.Models
{
    public class BankOffers
    {
        public string Title { get; set; }
        public string CTALink { get; set; }
        public string CTALinkText { get; set; }
        public List<Offers> Offers { get; set; }
    }
    public class Offers
    {
        public string Title { get; set; }
        public string ImageSrc { get; set; }
        public string CTALink { get; set; }
        public string CTALinkText { get; set; }
        public string ThumbnailImage { get; set; }
        public string MobileImage { get; set; }
    }
}