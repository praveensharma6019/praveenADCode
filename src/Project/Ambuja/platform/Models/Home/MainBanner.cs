using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models
{
    public class MainBanner 
    {
        public List<HomeMainBanner> Data { get; set; }
    }

    public class HomeMainBanner : VideoContentModel
    {
        public string Link { get; set; }
        public string LinkText { get; set; }
        public string LinkTarget { get; set; }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public bool IsOverlayRequired { get; set; }
        public GtmDataModel GtmData { get; set; }
    }
}