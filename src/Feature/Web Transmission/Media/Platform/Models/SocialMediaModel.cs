using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Models
{
    public class SocialMediaList
    {
        public List<SocialMediaModel> socialMediaList { get; set; }
    }

    public class SocialMediaModel
    {
        public string LinkText { get; set; }
        public string LinkURL { get; set; }
        public string LinkTarget { get; set; }
        public string ImageSrc  { get; set; }
        public string ImageAltText { get; set; }
    }
}