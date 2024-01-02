using Newtonsoft.Json;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.Models
{
    public class JobData
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("downloadtext")]
        public string DownloadText { get; set; }

        [JsonProperty("downloadurl")]
        public string DownloadUrl { get; set; }

        [JsonProperty("sharetext")]
        public string ShareText { get; set; }

        [JsonProperty("shareurl")]
        public string ShareUrl { get; set; }

        [JsonProperty("buttontext")]
        public string ButtonText { get; set; }

        [JsonProperty("buttonurl")]
        public string ButtonUrl { get; set; }


        [JsonProperty("realtyLogo")]
        public string RealityLogo { get; set; }


        [JsonProperty("realtyAlt")]
        public string RealityAlt { get; set; }
    }
}