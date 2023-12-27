using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class OurBusinessModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string Url { get; set; }
        public string UrlTarget { get; set; }
        public string UrlName { get; set; }
        public string image { get; set; }

        public List<BusinessDataList> BuisnessDataList { get; set; } 



    }

    public class BusinessDataList
    {
        public string ImageTitle { get; set; }
        public string ImageDescripton { get; set; }
        public string WebImage { get; set; }
        public string CTAUrl { get; set; }
        public string CTAUrlTarget { get; set; }
        public string MobileImage { get;set; }

       

    }
}