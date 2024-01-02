using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{

    public class WhyUsImageBanner
    {
        public ImageModel ImageBanner { get; set; }
    }

    public class ImageBanner
    {
        public string imageSource { get; set; }
        public string imageSourceMobile { get; set; }
        public string imageSourceTablet { get; set; }
        public string imageAlt { get; set; }
    }

}