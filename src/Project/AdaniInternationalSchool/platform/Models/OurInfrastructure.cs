using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{

    public class OurInfrastructure: BaseContentModel
    {
        public OurInfrastructure()
        {
            Gallery = new List<InfrastructureGallery>();
        }

        public string BtnText { get; set; }
        public string BtnLink { get; set; }

        public GtmDataModel GtmData { get; set; }
        public List<InfrastructureGallery> Gallery { get; set; }
    }
}