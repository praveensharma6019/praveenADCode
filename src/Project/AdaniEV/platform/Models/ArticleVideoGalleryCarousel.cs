using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class ArticleVideoGalleryCarousel
    {
        public string id { get; set; }
        public string type { get; set; }
        public List<ArticleVideoGalleryList> widgetItems { get; set; }
    }

    public class ArticleVideoGalleryList
    {
         public string id { get; set; }
         public string title { get; set; }
         public string imageSrc { get; set; }
         public string videoSrc { get; set; }
    }
}