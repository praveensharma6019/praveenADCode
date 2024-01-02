namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class GalleryIconsData
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    public class ReraData
    {

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("rera")]
        public string Rera { get; set; }

        [JsonProperty("reraWebsite")]
        public string ReraWebsite { get; set; }
        [JsonProperty("reraWebsiteLink")]
        public string ReraWebsitelink { get; set; }
        [JsonProperty("reraWebsiteLinkTarget")]
        public string ReraWebsiteLinkTarget { get; set; }
        [JsonProperty("projectListedOn")]
        public string ProjectListedOn { get; set; }

        [JsonProperty("reraNumber")]
        public string ReraNumber { get; set; }

        [JsonProperty("@as")]
        public string As { get; set; }

        [JsonProperty("downloadLink")]
        public string downloadLink { get; set; }



        [JsonProperty("reranumber")]
        public string reranumber { get; set; }


        [JsonProperty("ModalTitle")]
        public string ModalTitle { get; set; }


        [JsonProperty("download")]
        public string download { get; set; }

        public List<GalleryIconModelsData> reraModal { get; set; }
    }

    public class GalleryIconModelsData
    {
        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("reraid")]
        public string reraid { get; set; }

        [JsonProperty("download")]
        public string download { get; set; }
        [JsonProperty("downloadLink")]
        public string downloadLink { get; set; }
        [JsonProperty("reraWebsiteLink")]
        public string ReraWebsitelink { get; set; }
        [JsonProperty("reraWebsiteLinkTarget")]
        public string reraWebsiteLinkTarget { get; set; }
        [JsonProperty("reraTitle")]
        public string reraTitle { get; set; }
        [JsonProperty("qrCodeImage")]
        public string qrCodeImage { get; set; }
    }


    public class ProjectHighlights
    {

        [JsonProperty("galleryIconsData")]
        public List<GalleryIconsData> GalleryIconsData { get; set; }
        public List<ReraData> reraData { get; set; }
    }

    public class PropertyHighLight
    {
        [JsonProperty("projectHighlights")]
        public ProjectHighlights ProjectHighlights { get; set; }
    }
}