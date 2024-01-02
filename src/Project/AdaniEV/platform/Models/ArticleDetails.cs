using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{

    public class ArticleDetails
    {
        public string id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string readTime { get; set; }
        public string date { get; set; }
        public SocialMediaLinks SocialMediaLinks { get; set; } = new SocialMediaLinks();
        public Content Content { get; set; } = new Content();   
    }
    public class SocialMediaLinks
    {
        public Widget widget { get; set; } = new Widget();
    }

    public class Widget
    {
        public string id { get; set; }
        public List<ArticleDetailsSocialMediaLinksItems> articleDetailsSocialMediaLinksItems { get; set; } = new List<ArticleDetailsSocialMediaLinksItems>();
    }


    public class ArticleDetailsSocialMediaLinksItems
    {
        public string id { get; set; }
        public string ctaText { get; set; }
        public string ctaLink { get; set; }
        public string imageSrc { get; set; }
    }
    public class Content
    {
        public Contentwidget widget { get; set; } = new Contentwidget();
    }
    public class Contentwidget
    {
        public string id { get; set; }
        public string text { get; set; }
        public List<Images> images { get; set; } = new List<Images>();
    }

    public class Images
    {
        public string id { get; set; }
        public string src { get; set; }
    }

}
