namespace Project.AdaniInternationalSchool.Website.Models
{
    public class AdmissionOverview : OverviewMethod
    {
        public string SectionID { get; set; }
        public string BackgroundImage { get; set; }

        public string ImageAlt { get; set; }
        public GtmDataModel GtmData { get; set; }
    }
}