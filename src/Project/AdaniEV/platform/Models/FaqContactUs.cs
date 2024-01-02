using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class FaqContactUs
    {
        public string id { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string type { get; set; }
        public string imageSrc { get; set; }
        public string ctaText { get; set; }
        public string ctaLink { get; set; }
    }

   
}