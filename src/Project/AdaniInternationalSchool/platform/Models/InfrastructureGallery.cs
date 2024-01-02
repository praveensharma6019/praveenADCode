namespace Project.AdaniInternationalSchool.Website.Models
{
    public class InfrastructureGallery: ImageModel
    {
        public string Label { get; set; }
        public string Link { get; set; }

        public GtmDataModel GtmData { get; set; }
    }
}