using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.SitemapXML
{
    public class SiteMapXML
    {
        [SitecoreChildren]
        public virtual IEnumerable<SiteMapDetails> sitemapXML { get; set; }
    }

    public class SiteMapDetails
    {
        [SitecoreField(FieldId = "{C588E078-CAC1-46AA-AD1D-21B462A232DC}")]
        public virtual string Url { get; set; }

        public virtual string Priority { get; set; }
        public virtual string Lastmod { get; set; }
        public virtual string DescPriority { get; set; }
        public virtual string ChangeFrequency { get; set; }
    }
}