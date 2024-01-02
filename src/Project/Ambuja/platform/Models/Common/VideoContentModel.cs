namespace Project.AmbujaCement.Website.Models
{
    public class VideoContentModel : ImageModel
    {
        public string VideoSource { get; set; }
        public string VideoSourceMobile { get; set; }
        public string VideoSourceTablet { get; set; }
        public string VideoSourceOGG { get; set; }
        public string VideoSourceMobileOGG { get; set; }
        public string VideoSourceTabletOGG { get; set; }
        public string MediaType { get; set; }
        public bool AutoPlay { get; set; }
    }
}