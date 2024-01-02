namespace Project.AdaniInternationalSchool.Website.Models
{
    public class BannerGallery : ImageModel
    {
        public string MediaType { get; set; }
        public string VideoSource { get; set; }
        public string VideoSourceMobile { get; set; }
        public string VideoSourceTablet { get; set; }
        public string VideoSourceOGG { get; set; }
        public string VideoSourceMobileOGG { get; set; }
        public string VideoSourceTabletOGG { get; set; }
        public bool AutoPlay { get; set; }
        public bool IsOverlayRequired { get; set; }


        public GtmDataModel GtmData { get; set; }

    }
}