using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class InfrastructureCategory
    {
        public List<InfrastructureCategoryList> Features { get; set; }
    }

    public class InfrastructureCategoryList: ImageModel
    {
        public string Url { get; set; }
        public string Target { get; set; }
        public string ImgTitle { get; set; }

        public GtmDataModel GtmData { get; set; }
    }
}