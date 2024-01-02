using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class ServiceCarousal
    {
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }
        [JsonProperty(PropertyName = "items")]
        public List<HeroCarousel> items { get; set; }
    }
    public class ServiceCarousalwidgets
    {
        [JsonProperty(PropertyName = "widget")]
        public WidgetItem widget { get; set; }
    }
}