using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class HowItWorkStepsModel
    {
        [JsonProperty(PropertyName = "tabContent")]
        public List<StepsTab> TabContent { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "btnText")]
        public string CTAText { get; set; }
        [JsonProperty(PropertyName = "btnUrl")]
        public string CTAUrl { get; set; }
        [JsonProperty(PropertyName = "appbtnText")]
        public string AppCTAText { get; set; }
        [JsonProperty(PropertyName = "appbtnUrl")]
        public string AppCTAUrl { get; set; }
    }
    public class StepsTab
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "cards")]
        public List<Cards> Cards { get; set; }
    }
    public class Cards
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "src")]
        public string SrcImage { get; set; }

        [JsonProperty(PropertyName = "alt")]
        public string SrcImageAlt { get; set; }

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
        
    public class HowItWorkStepsWidgets
    {
        [JsonProperty(PropertyName = "widget")]
        public WidgetItem widget { get; set; }
    }
}