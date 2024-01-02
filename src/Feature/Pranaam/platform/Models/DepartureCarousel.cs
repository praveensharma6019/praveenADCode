using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class DepartureCarousel
    {
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "Src")]
        public string Src { get; set; }

        [JsonProperty(PropertyName = "Alt")]
        public string Alt { get; set; }

        [JsonProperty(PropertyName = "Text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "BtnUrl")]
        public string BtnUrl { get; set; }

        [JsonProperty(PropertyName = "BtnText")]
        public string BtnText { get; set; }
        public string DesktopImage { get; set; }
        public string DesktopImageAlt { get; set; }
        public string MobileImage { get; set; }
        public string MobileImageAlt { get; set; }
        public string ThumbnailImage { get; set; }
        public string ThumbnailImageAlt { get; set; }
    }
    public class HeroContentItem
    {
        [JsonProperty(PropertyName = "HeroContent")]
        public DepartureCarousel HeroContent { get; set; }
    }
}