using Newtonsoft.Json;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.Models
{
    public class LifeatAdani
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("imgsrc")]
        public string Imgsrc { get; set; }

        [JsonProperty("imgalt")]
        public string Imgalt { get; set; }

        [JsonProperty("viewallJobs")]
        public string ViewAllJobs { get; set; }

        [JsonProperty("viewallJobslink")]
        public string ViewAllJobsLink { get; set; }
    }
}