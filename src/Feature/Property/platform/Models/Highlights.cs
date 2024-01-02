namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    using Newtonsoft.Json;

    public class Highlights
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("src")]
        public string Src { get; set; }

        [JsonProperty("imgAlt")]
        public string ImgAlt { get; set; }

        [JsonProperty("imgTitle")]
        public string ImgTitle { get; set; }

        [JsonProperty("logoSrc")]
        public string LogoSrc { get; set; }

        [JsonProperty("logoAlt")]
        public string LogoAlt { get; set; }

        [JsonProperty("logoTitle")]
        public string LogoTitle { get; set; }

        [JsonProperty("aboutImg")]
        public string AboutImg { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("iconDesc")]
        public string IconDesc { get; set; }

        [JsonProperty("degree")]
        public string Degree { get; set; }

        [JsonProperty("tour")]
        public string Tour { get; set; }
        [JsonProperty("tabType")]
        public string tabType { get; set; }
        [JsonProperty("ImgCount")]
        public string ImgCount { get; set; }

        [JsonProperty("srcMobile")]
        public string SrcMobile { get; set; }
    }
}