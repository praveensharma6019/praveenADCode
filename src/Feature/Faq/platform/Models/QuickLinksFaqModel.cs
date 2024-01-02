using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Faq.Platform.Models
{
    public class QuickLinksFaqModel
    {
        [JsonProperty(PropertyName = "faq")]
        public List<QuickLinksFaq> QuickLinksFaq { get; set; }
    }
    public class QuickLinksFaq
    {
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "Data")]
        public List<Data> FaqItems { get; set; }
    }
    public class Data
    {
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Body")]
        public string Body { get; set; }
    }

   

}