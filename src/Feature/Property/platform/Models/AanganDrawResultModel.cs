namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    using Newtonsoft.Json;

    public class AanganDrawResultModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Heading")]
        public string heading { get; set; }

        [JsonProperty(PropertyName = "ImageSource")]
        public string imageSource { get; set; }

        [JsonProperty(PropertyName = "ImageSourceMobile")]
        public string imageSourceMobile { get; set; }

        [JsonProperty(PropertyName = "ImageSourceTablet")]
        public string ImageSourceTablet { get; set; }

        [JsonProperty(PropertyName = "ImgAlt")]
        public string ImgAlt { get; set; }
    }
}