using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class BookingSteps
    {
        [JsonProperty(PropertyName = "options")]
        public List<Steps> options { get; set; }
    }
    public class Steps
    {
        [JsonProperty(PropertyName = "imgSrc")]
        public string imgSrc { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "alt")]
        public string alt { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string value { get; set; }

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
    public class ServiceStepslwidgets
    {
        [JsonProperty(PropertyName = "widget")]
        public WidgetItem widget { get; set; }
    }
}