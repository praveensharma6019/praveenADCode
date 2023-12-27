using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class AddOnService
    {
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "Items")]
        public List<Service> Items { get; set; }
    }

    public class Service
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "Src")]
        public string Src { get; set; }

        [JsonProperty(PropertyName = "Alt")]
        public string Alt { get; set; }

        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "InitialPrice")]
        public string InitialPrice { get; set; }

        [JsonProperty(PropertyName = "FinalPrice")]
        public string FinalPrice { get; set; }

        [JsonProperty(PropertyName = "BtnText")]
        public string BtnText { get; set; }

        [JsonProperty(PropertyName = "BtnUrl")]
        public string BtnUrl { get; set; }
        [JsonProperty(PropertyName = "btnVariant")]
        public string btnVariant { get; set; }
        public string DesktopImage { get; set; }
        public string DesktopImageAlt { get; set; }
        public string MobileImage { get; set; }
        public string MobileImageAlt { get; set; }
        public string ThumbnailImage { get; set; }
        public string ThumbnailImageAlt { get; set; }
    }
    public class AddOnServicewidgets
    {
        [JsonProperty(PropertyName = "widget")]
        public WidgetItem widget { get; set; }
    }
}