using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;

namespace Sitecore.Adani.Website.Models
{
    [XmlRoot("urlset")]
    public class Urlset
    {
        ///

        /// Constructor to initialize Url Object
        /// 

        public Urlset() { Url = new List<SitemapUrl>(); }

        ///

        /// Urls collection
        /// 

        [XmlElement("url")]
        public List<SitemapUrl> Url { get; set; }

    }

    ///

    /// Class to generate url with its parameters for sitemap
    /// 

  
}