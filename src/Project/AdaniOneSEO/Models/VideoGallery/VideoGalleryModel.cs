using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Models.VideoGallery
{
    public class VideoGalleryModel
    {
        public virtual string GalleryTitle { get; set; }

        public virtual Image GalleryBanner { get; set; }
        public virtual Image GalleryMobileBanner { get; set; }
        public virtual Image GalleryTabletBanner { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<VideoDetails> Videos { get; set; }
    }

    public class VideoDetails
    {
        public virtual string VideoTitle { get; set; }
        public virtual string VideoDescription { get; set; }
        public virtual string VideoCategory { get; set; }
        public virtual string VideoSubCategory { get; set; }
        public virtual Image VideoThumbnail { get; set; }
        public virtual Link VideoUrl { get; set; }
        public virtual Link YoutubeVideoLink { get; set; }
    }
}