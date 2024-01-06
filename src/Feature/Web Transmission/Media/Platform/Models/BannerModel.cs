using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Models
{
    public class BannerModelList
    {
        public List<BannerModel> bannerList { get; set; }
    }
    public class BannerModel
    {
        public string Title { get; set; }
        public string ImageSrc { get; set; }
        public string ImageAlt { get; set; }
        public string LinkURL { get; set; }
    }
}