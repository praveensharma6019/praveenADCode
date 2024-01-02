using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models.Academics
{
    public class GuideModel : BaseContentModel
    {
        public string Type { get; set; }
        public string Variant { get; set; }
        public List<FeatureModel> Features { get; set; }

    }
}
