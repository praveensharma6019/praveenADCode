using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.Home
{
    public class VerticalCarousel
    {
        public string StarImage { get; set; }
        public string StarImageAlt { get; set; }
        public string SectionHeading { get; set; }
        public List<VerticalCarouselDatum> Data { get; set; }
    }

    public class VerticalCarouselDatum : ImageModel
    {
        public string Id { get; set; }
        public List<VerticalCarouselDatumFeature> Features { get; set; }
        public string LinkText { get; set; }
        public string LinkTarget { get; set; }
        public string Link { get; set; }
        public GtmDataModel GtmData { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
    }

    public class VerticalCarouselDatumFeature
    {
        public string ImageSource { get; set; }
        public string ImageAlt { get; set; }
        public string Heading { get; set; }
    }

   


}