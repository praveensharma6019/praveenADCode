using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class FooterIllustration
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

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

        [JsonProperty(PropertyName = "btnText")]
        public string CTAText { get; set; }
        [JsonProperty(PropertyName = "btnUrl")]
        public string CTAUrl { get; set; }

        [JsonProperty(PropertyName = "desc")]
        public List<string> DescriptionData { get; set; }

        [JsonProperty(PropertyName = "appDesc")]
        public List<string> AppDesc { get; set; }
    }

    public class FooterIllustartionWidgets
    {
        [JsonProperty(PropertyName = "widget")]
        public WidgetItem widget { get; set; }
    }
}