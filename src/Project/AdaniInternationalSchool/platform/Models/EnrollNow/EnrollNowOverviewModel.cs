using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models.EnrollNow
{
    public class EnrollNowOverviewModel : BaseContentModel
    {
        public List<OverviewButton> Button { get; set; }
    }

    public class OverviewButton
    {
        public string URL { get; set; }
        public string Label { get; set; }
        public string Variant { get; set; }

        public GtmDataModel GtmData { get; set; }
    }
}