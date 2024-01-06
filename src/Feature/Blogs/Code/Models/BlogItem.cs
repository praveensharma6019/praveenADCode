using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Blogs.Models
{
    public class BlogItem
    {
        public Item Item { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
    }
}