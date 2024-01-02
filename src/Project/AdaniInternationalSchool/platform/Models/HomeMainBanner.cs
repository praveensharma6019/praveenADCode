using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{

    public class HomeMainBanner: BaseContentModel
    {
        public string Link { get; set; }
        public string LinkText { get; set; }

        public GtmDataModel GtmData { get; set; }


        public List<BannerGallery> Data { get; set; }
    }
}