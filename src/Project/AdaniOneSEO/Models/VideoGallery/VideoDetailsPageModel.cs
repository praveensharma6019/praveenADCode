using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Models.VideoGallery
{
    public class VideoDetailsPageModel : VideoDetails
    {
        [SitecoreField(FieldName = "SimilarVideos")]
        public virtual IEnumerable<VideoDetails> SimilarVideos { get; set; }
    }
}