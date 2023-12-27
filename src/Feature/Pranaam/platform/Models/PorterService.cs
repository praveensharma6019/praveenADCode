using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class PorterService
    {
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "desc")]
        public string desc { get; set; }

        [JsonProperty(PropertyName = "src")]
        public string src { get; set; }

        [JsonProperty(PropertyName = "btnTitle")]
        public string btnTitle { get; set; }

        [JsonProperty(PropertyName = "btnUrl")]
        public string btnUrl { get; set; }

        [JsonProperty(PropertyName = "price")]
        public string price { get; set; }

        [JsonProperty(PropertyName = "alt")]
        public string alt { get; set; }

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
    public class Porterwidgets
    {
        [JsonProperty(PropertyName = "widget")]
        public WidgetItem widget { get; set; }
    }

    public class StandaloneProductTerminal
    {
        public List<StandaloneProductTerminals> ListOfTerminals { get; set; }
    }
    public class StandaloneProductTerminals
    {
        public string TerminalName { get; set; }
        public StandaloneProductDetailsList StandaloneProductDetails { get; set; }
    }
    public class StandaloneProductDetailsList
    {
        public List<StandaloneProductDetail> StandaloneProductDetail { get; set; }
    }
    public class StandaloneProductDetail
    {
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        //public string PackageNumber { get; set; }
        public string Id { get; set; }
        public string AirportMasterId { get; set; }
        public string StandaloneProductImage { get; set; }

    }
}