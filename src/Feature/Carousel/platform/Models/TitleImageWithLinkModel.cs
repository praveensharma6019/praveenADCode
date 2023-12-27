using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class TitleImageWithLinkModel
    {
        public string title { get; set; }
        public string link { get; set; }
        public string uniqueId { get; set; }
        public string desktopImage { get; set; }
        public string mobileImage { get; set; }
        public bool isRestricted { get; set; }
        public bool isUrl { get; set; }
        public TagName tags { get; set; }

        public TitleImageWithLinkModel()
        {
            tags = new TagName();
        }
    }
}