namespace Project.AdaniInternationalSchool.Website.Models
{
    public class BannerModel : ImageModel
    {
        public string Link { get; set; }
        public string Target { get; set; }
        public GtmDataModel GtmData { get; set; }
    }
}