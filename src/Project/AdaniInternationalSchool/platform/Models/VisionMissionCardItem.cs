namespace Project.AdaniInternationalSchool.Website.Models
{
    public class VisionMissionCardItem : BaseImageContentModel
    {
        public string URL { get; set; }
        public string Target { get; set; }
        public string Theme { get; set; }
        public string VideoDuration { get; set; }
        public GtmDataModel GtmData { get; set; }
    }
}