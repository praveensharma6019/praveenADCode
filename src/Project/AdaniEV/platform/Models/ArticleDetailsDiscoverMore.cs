using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
  
    public class ArticleDetailsDiscoverMore
    {
        public string id { get; set; }
        public string type { get; set; }
        public List<ArticleDetailsDiscoverMoreCardItems> widgetItems { get; set; }
    }

    public class ArticleDetailsDiscoverMoreCardItems
    {
        public string id { get; set; }
        public string imageSrc { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string ctaLink { get; set; }

        public ArticleDetailsDiscoverMoreCaption Caption { get; set; }

    }

    public class ArticleDetailsDiscoverMoreCaption
    {
        public string CaptionDate { get; set; }
        public string CaptionSource { get; set; }
    }
}
