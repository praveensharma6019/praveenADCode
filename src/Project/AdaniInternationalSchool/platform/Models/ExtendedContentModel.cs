namespace Project.AdaniInternationalSchool.Website.Models
{
    public class ExtendedContentModel : ImageContentModel
    {
        public string VideoSource { get; set; }
        public string VideoSourceMobile { get; set; }
        public string VideoSourceTablet { get; set; }
        public string DefaultVideoSource { get; set; }
        public string DefaultVideoSourceMobile { get; set; }
        public string DefaultVideoSourceTablet { get; set; }
        public string DefaultVideoSourceOgg { get; set; }
        public string DefaultVideoSourceOggMobile { get; set; }
        public string DefaultVideoSourceOggTablet { get; set; }
        public string MapSource { get; set; }
    }
}