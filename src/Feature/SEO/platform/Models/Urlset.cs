using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sitecore.Feature.SEO.Models
{
    [XmlRoot("urlset")]
    public class Urlset
    {
        public Urlset() { Url = new List<Url>(); }

        [XmlElement("url")]
        public List<Url> Url { get; set; }

    }


    public class Url
    {
        [XmlElement("loc")]
        public string Loc { get; set; }

        [XmlElement("lastmod")]
        public string Lastmod { get; set; }

    }
}