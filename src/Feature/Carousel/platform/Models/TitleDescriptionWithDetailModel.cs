using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class TitleDescriptionWithDetailModel
    {
        public string title { get; set; }
        public string detail { get; set; }      

        public List<DetailsList> DetailsDataList { get; set; }
    }

    public class DetailsList
    {
        public string value { get; set; }
        public string sign { get; set; }
        public string detail { get; set; }       



    }
}