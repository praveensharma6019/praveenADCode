using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class GalleryContentModel<T> : ContentModel
    {
        public string SectionID { get; set; }
        public string Variant { get; set; }

        public GtmDataModel GtmData { get; set; }
        public List<T> Gallery { get; set; }
    }

}