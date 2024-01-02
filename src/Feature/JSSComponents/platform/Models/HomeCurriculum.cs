using System.Collections.Generic;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Models
{
    public class HomeCurriculum : BaseContentModel
    {
        public string BtnLink { get; set; }
        public string BtnText { get; set; }
        public  GtmDataModel GtmData { get; set; }        
        public List<HomeLearningGalleryItem> Gallery { get; set; }
        public List<HomeLearningGalleryItem> AcademicDetails { get; set; }
        public List<FeaturesList> Features { get; set; }
    }

    public class HomeLearningGalleryItem : BaseImageContentModel
    {
        public string Link { get; set; }
        public GtmDataModel GtmData { get; set; }
    }
}