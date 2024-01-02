using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models
{
    public class SiteMapXML
    {
        [SitecoreChildren]
        public virtual IEnumerable<SiteMapDetails> sitemapXML { get; set; }
    }
    public class SiteMapDetails
    {
        public virtual string Url { get; set; }
        public virtual string Priority { get; set; }
        public virtual string Lastmod { get; set; }
        public virtual string DescPriority { get; set; }
        public virtual string ChangeFrequency { get; set; }
    }
}