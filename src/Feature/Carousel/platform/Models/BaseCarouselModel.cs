using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class BaseCarouselModel
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Url { get; set; }
        public string UrlTarget { get; set; }
        public string btnName { get; set; }
        public string Url2 { get; set; }
        public string Url2Target { get; set; }
        public string btnName2 { get; set; }
        public string SubTitle { get; set; }
        public string SubDetail { get; set; }
        public string WebImage { get; set; }
        public string MobileImage { get; set; }
        public string DeepLink { get; set; }
        public string DeepLink2 { get; set; }
        public bool isAgePopUp { get; set; }

    }

    public class MulticarouselModel
    { 
     public string Title { get; set; }

     public List<BaseCarouselModel> list { get; set; }
    }
     
}