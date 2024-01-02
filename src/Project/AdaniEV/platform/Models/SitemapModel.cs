using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class SitemapModel
    {
       public List<SitemapItem> sitemapItems= new List<SitemapItem>();
    }

    public class SitemapItem
    {
        public string imgUrl { get; set; }
        public string title { get; set; }
    }

}