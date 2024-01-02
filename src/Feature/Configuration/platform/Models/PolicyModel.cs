using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Models
{
    public class PolicyModel
    {
        [JsonProperty(PropertyName = "disclaimer")]
        public Disclaimer Disclaimber { get; set; }
        [JsonProperty(PropertyName = "cookiePolicy")]
        public Cookie CookiePolicy { get; set; }
    }
    public class Disclaimer
    {
        [JsonProperty(PropertyName = "disclaimerLogo")]
        public string DisclaimerLogo { get; set; }
        [JsonProperty(PropertyName = "logoAlt")]
        public string LogoAlt { get; set; }
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        [JsonProperty(PropertyName = "btnUrl")]
        public string ButtonUrl { get; set; }
        [JsonProperty(PropertyName = "btnText")]
        public string ButtonText { get; set; }
        [JsonProperty(PropertyName = "btnCancelText")]
        public string CancelButtonText { get; set; }
    }

    public class Cookie
    {
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        [JsonProperty(PropertyName = "btnUrl")]
        public string ButtonUrl { get; set; }
        [JsonProperty(PropertyName = "btnText")]
        public string ButtonText { get; set; }
        [JsonProperty(PropertyName = "btnCancelText")]
        public string CancelButtonText { get; set; }
    }
}