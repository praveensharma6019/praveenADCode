using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class VisitOurSchoolDataItemModel : ExtendedContentModel
    {
        public List<LocationDetails> LocationData { get; set; }
    }

    public class LocationDetails
    {
        public string Detail { get; set; }
        public string Label { get; set; }
        public string Link { get; set; }
    }
}