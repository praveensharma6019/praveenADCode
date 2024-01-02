using System;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class StoriesGallery: BaseImageContentModel
    {
        public string Link { get; set; }
        public string Target { get; set; }
        public string Date { get; set; }
        public GtmDataModel GtmData { get; set; }
    }
}