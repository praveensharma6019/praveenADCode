using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.Common
{
    public class VideoModel
    {
        public string MediaType { get; set; }
        public string VideoSource { get; set; }
        public string VideoSourceMobile { get; set; }
        public string VideoSourceTablet { get; set; }
        public string VideoSourceOGG { get; set; }
        public string VideoSourceMobileOGG { get; set; }
        public string VideoSourceTabletOGG { get; set; }
        public bool AutoPlay { get; set; }

    }
}