using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class ArticleModel
    {
        public Articles articles { get; set; }
    }
    public class Articles
    {
        public string heading { get; set; }
        public List<ArticleData> data { get; set; }
    }

    public class ArticleData
    {
        public string src { get; set; }
        public string title { get; set; }
        public string date { get; set; }
    }

}