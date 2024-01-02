using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.SiteMap.Platform.Models
{
    public class SiteMapModel
    {
        public List<url> sitemapdata { get; set; }


    }
    public class url
    {
        public string loc { get; set; }
        public string lastmod { get; set; }
        public string changefreq { get; set; }
        public string priority { get; set; }
        public string mobile { get; set; }

        public url(string loc, string lastmod, string changefreq, string priority, string mobile)
        {
            this.loc = loc;
            this.lastmod = lastmod;
            this.changefreq = changefreq;
            this.priority = priority;
            this.mobile = mobile;
        }
    }

}