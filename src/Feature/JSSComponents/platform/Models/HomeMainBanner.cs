using System.Collections.Generic;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Models
{
    public class HomeMainBanner: BaseContentModel
    {
        public string Link { get; set; }
        public string LinkText { get; set; }
        public GtmDataModel GtmData { get; set; }
        public List<BannerGallery> Data { get; set; }
    }
}