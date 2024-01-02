namespace Project.AdaniInternationalSchool.Website.Models
{
    public class WelcomeCardItemModel : ContentModel
    {
        public string ImageAlt { get; set; }
        public string PosterImage { get; set; }

        public string PlayText { get; set; }
        public bool autoplay { get; set; }

        public string VideoSource { get; set; }
        public string VideoSourceMobile { get; set; }
        public string VideoSourceTablet { get; set; }
        public string VideoSourceOgg { get; set; }
        public string VideoSourceMobileOgg { get; set; }
        public string VideoSourceTabletOgg { get; set; }
        public string DefaultVideoSource { get; set; }
        public string DefaultVideoSourceMobile { get; set; }
        public string DefaultVideoSourceTablet { get; set; }
        public string DefaultVideoSourceOgg { get; set; }
        public string DefaultVideoSourceMobileOgg { get; set; }
        public string DefaultVideoSourceTabletOgg { get; set; }

        public GtmDataModel GtmData { get; set; }
        public GtmVideoStartModel GtmVideoStart { get; set; }
        public GtmVideoStartModel GtmVideoComplete { get; set; }
        public GtmVideoProgressModel GtmVideoProgress { get; set; }
        public string uploadDate { get; set; }
        public string SeoName { get; set; }
        public string SeoDescription { get; set; }
    }
}