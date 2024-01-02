namespace Project.AdaniInternationalSchool.Website.Models
{
    public class AdmissionCardGallery : ContentModel
    {
        public string SubDescription { get; set; }
        public string ImageAlt { get; set; }
        public string BackgroundImage { get; set; }

        public GtmDataModel GtmData { get; set; }
    }
}