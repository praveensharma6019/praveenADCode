using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class FAQ
    {
        public string title { get; set; }
        public string description { get; set; }
        public string autoId { get; set; }
        public bool isAgePopup { get; set; }
        public GTMTags tags { get; set; }
    }
}