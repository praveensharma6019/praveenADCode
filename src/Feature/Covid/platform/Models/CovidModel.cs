using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Covid.Models
{
    public class Covid
    {
        [JsonProperty(PropertyName = "covidcard")]
        public CovidModel covidCard { get; set; }
    }
    public class CovidModel
    {
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }
        
        [JsonProperty(PropertyName = "text")]
        public string text { get; set; }
        
        [JsonProperty(PropertyName = "src")]
        public string src { get; set; }
        
        [JsonProperty(PropertyName = "alt")]
        public string alt { get; set; }

        [JsonProperty(PropertyName = "btn")]
        public string btn { get; set; }

        [JsonProperty(PropertyName = "btnUrl")]
        public string btnUrl { get; set; }

        [JsonProperty(PropertyName = "CarousalItems")]
        public List<CovidCarousel> CarousalItems { get; set; }
        
        [JsonProperty(PropertyName = "mobileImage")]
        public string mobileImage { get; set; }

        [JsonProperty(PropertyName = "mobileImageAlt")]
        public string mobileImageAlt { get; set; }

        [JsonProperty(PropertyName = "webImage")]
        public string webImage { get; set; }

        [JsonProperty(PropertyName = "webImageAlt")]
        public string webImageAlt { get; set; }

        [JsonProperty(PropertyName = "thumbnailImage")]
        public string thumbnailImage { get; set; }

        [JsonProperty(PropertyName = "thumbnailImageAlt")]
        public string thumbnailImageAlt { get; set; }
    }

    public class CovidCarousel
    {
        [JsonProperty(PropertyName = "Summary")]
        public string Summary { get; set; }

        [JsonProperty(PropertyName = "Details")]
        public string Details { get; set; }

        [JsonProperty(PropertyName = "Image")]
        public string Image { get; set; }
    }
    public class Covidwidgets
    {
        [JsonProperty(PropertyName = "widget")]
        public WidgetItem widget { get; set; }
    }
}