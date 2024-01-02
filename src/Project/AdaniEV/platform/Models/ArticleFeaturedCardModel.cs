using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
  
    public class ArticleFeaturedCardModel
    {
        public string id { get; set; }
        public string type { get; set; }
        public List<ArticleFeaturedCardItems> widgetItems { get; set; }
    }

    public class ArticleFeaturedCardItems
    {
        public string id { get; set; }
        public string imageSrc { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string ctaText { get; set; }
        public string ctaLink { get; set; }

        public ArticleCaption Caption { get; set; }

    }

    public class ArticleCaption
    {
        public string CaptionDate { get; set; }
        public string CaptionSource { get; set; }
    }
}
