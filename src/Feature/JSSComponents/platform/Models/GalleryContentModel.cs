using System.Collections.Generic;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Models
{
    public class GalleryContentModel<T> : Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Models.ContentModel
    {
        public string SectionID { get; set; }
        public string Variant { get; set; }

        public GtmDataModel GtmData { get; set; }
        public List<T> Gallery { get; set; }
    }

}