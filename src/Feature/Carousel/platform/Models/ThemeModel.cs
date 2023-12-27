using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class ThemeModel
    {
        public string image { get; set; }
        public string mobileImage { get; set; }
        public string video { get; set; }
        public string mobileVideo { get; set; }
        public bool webActive { get; set; }
        public bool mWebActive { get; set; }
        public string ctaUrl { get; set; }
        public string linkTarget { get; set; }
        public string Title { get; set; }
    }
}