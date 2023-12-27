using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class PopularRoutesModel
    {
        public string TabName { get; set; }
        public List<PopularRoutesDataList> PopularRouteList { get; set; }

    }
    public class PopularRoutesDataList
    {
        public string title { get; set; }
        public string description { get; set; }
        public string WebImage { get; set; }
        public string MobileImage { get; set; }
        public string RedirectLink { get; set; }

    }
}