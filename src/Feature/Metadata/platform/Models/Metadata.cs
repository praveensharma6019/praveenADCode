using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.MetaData.Platform.Models
{
    public class Metadata
    {
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Keywords { get; set; }
        public string Canonical { get; set; }
        public string Viewport { get; set; }
        public string Robots { get; set; }
        public string OG_Title { get; set; }
        public string OG_Image { get; set; }
        public string OG_Description { get; set; }
    }
}