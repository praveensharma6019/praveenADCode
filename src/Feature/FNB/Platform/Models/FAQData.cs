using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FNB.Models
{
    public class FAQModel
    {
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "list")]
        public List<FAQCard> list { get; set; }

        // Field added for the loyalty Module
        [JsonProperty(PropertyName = "ctaText")]
        public string ctaText { get; set; }
        [JsonProperty(PropertyName = "ctaURL")]
        public string ctaURL { get; set; }
        [JsonProperty(PropertyName = "faqHTML")]
        public String faqHTML { get; set; }
    }

    public class FAQCard
    {
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string body { get; set; }
    }
}