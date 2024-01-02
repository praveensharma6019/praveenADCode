
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.Models
{
    public class UrlSet
    {
        ///

        /// Class to generate urlset for sitemap
        /// 

        [XmlRoot("urlset")]
        public class Urlset
        {
            ///

            /// Constructor to initialize Url Object
            /// 

            public Urlset() { Url = new List<Url>(); }

            ///

            /// Urls collection
            /// 

            [XmlElement("url")]
            public List<Url> Url { get; set; }

        }

        ///

        /// Class to generate url with its parameters for sitemap
        /// 

        public class Url
        {
            ///

            /// Location Parameter
            /// 

            [XmlElement("loc")]
            public string Loc { get; set; }

            ///

            /// Last modified on
            /// 

            [XmlElement("lastmod")]
            public string Lastmod { get; set; }

            [XmlElement("changefreq")]
            public string ChangeFrequency { get; set; }


            [XmlElement("priority")]
            public string Priority { get; set; }

            [XmlElement("mobile")]
            public string Mobile { get; set; }


            //Add required properties here like changefreq, priority
        }
    }
}