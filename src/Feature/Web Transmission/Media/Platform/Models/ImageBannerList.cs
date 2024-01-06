using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Models
{
    public class ImageBannerList
    {
       public List<ImageBanner> imageBanners;
    }
    public class ImageBanner
    {
        public string ImageUrl { get; set; }

        public string ImageAlt { get; set; }
    }

}