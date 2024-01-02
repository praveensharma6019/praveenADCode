using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class FacilitiesModel : BaseContentModel
    {
        public string SectionID { get; set; }
        public string Theme { get; set; }
        public string Variant { get; set; }
        public List<GalleryItemModel> Gallery { get; set; }
    }
}