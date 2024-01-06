using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Sitecore.Adani.Website.Models
{
    public class SitemapUrl
    {
        ///

        /// Location Parameter
        /// 

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("loc")]
        public string Loc { get; set; }

        ///

        /// Last modified on
        /// 

        [XmlElement("lastmod")]
        public string Lastmod { get; set; }

        //Add required properties here like changefreq, priority
    }
}