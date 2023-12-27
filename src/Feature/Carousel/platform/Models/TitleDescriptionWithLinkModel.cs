using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class TitleDescriptionWithLinkModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string image { get; set; }
        public string autoId { get; set; }
        public string linkTarget { get; set; }
        public bool isAgePopup { get; set; }
        public bool isInternational { get; set; }
        public GTMTags tags { get; set; }
    }
}