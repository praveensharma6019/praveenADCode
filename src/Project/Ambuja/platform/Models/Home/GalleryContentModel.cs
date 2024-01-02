using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models
{
    public class GalleryContentModel<T> : AmbujaCement.Website.Models.ContentModel
    {
        public string SectionID { get; set; }
        public string SubHeading { get; set; }
        public GtmDataModel GtmData { get; set; }
        public List<T> Gallery { get; set; }
        public string ImageSource { get; set; }
        public string ImageSourceMobile { get; set; }
        public string ImageSourceTablet { get; set; }
        public string ImageAlt { get; set; }
    }

}