using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Models.VideoGallery
{
    public class VideoDetailsPageModelNew : VideoDetailsNew
    {        
        public List<VideoDetailsNew> SimilarVideos { get; set; }
    }
}