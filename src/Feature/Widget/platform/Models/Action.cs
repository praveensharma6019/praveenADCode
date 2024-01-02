using Newtonsoft.Json;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.Models
{
    public class PAction
    {
        [JsonProperty("buttontext")]
        public string ButtonText { get; set; }

        [JsonProperty("imgTitle")]
        public string imgTitle { get; set; }

        [JsonProperty("modalTitle")]
        public string ModalTitle { get; set; }

        [JsonProperty("downloadtext")]
        public string DownloadText { get; set; }
        [JsonProperty("downloadModalTitle")]
        public string downloadModalTitle { get; set; }

        [JsonProperty("sharetext")]
        public string ShareText { get; set; }

        [JsonProperty("backlink")]
        public string Backlink { get; set; }

        [JsonProperty("src")]
        public string Src { get; set; }

        [JsonProperty("imgAlt")]
        public string ImgAlt { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("copylink")]
        public string CopyLink { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("facebook")]
        public string Facebook { get; set; }

        [JsonProperty("whatsapp")]
        public string Whatsapp { get; set; }

        [JsonProperty("downloadurl")]
        public string Downloadurl { get; set; }
        public LifeatAdani lifeatAdani { get; set; }
    }

}