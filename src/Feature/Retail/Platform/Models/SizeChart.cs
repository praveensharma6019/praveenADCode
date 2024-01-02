using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.Models
{
    public class SizeChart
    {
        public string SizeTitle { get; set; }
        public string SizeChartTitle { get; set; }
        public List<sizeChartImages> SizeChartImages { get; set; }
    }
    public class sizeChartImages
    {
        public string ImageSrc { get; set; }
        public string MobileImage { get; set; }
    }
}