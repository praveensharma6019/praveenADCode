using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniOneSEO.Website.Models.VideoGallery
{
    public class VideoGalleryModelNew
    {
        public string GalleryTitle { get; set; }

        public string GalleryBanner { get; set; }
        public string GalleryMobileBanner { get; set; }
        public string GalleryTabletBanner { get; set; }
        public List<VideoDetailsNew> Videos { get; set; }
    }
    public class VideoDetailsNew
    {
        public string VideoTitle { get; set; }
        public  string VideoDescription { get; set; }
        public  string VideoCategory { get; set; }
        public  string VideoSubCategory { get; set; }
        public  string VideoThumbnail { get; set; }
        public  string VideoUrl { get; set; }
        public  string YoutubeVideoLink { get; set; }
    }
}