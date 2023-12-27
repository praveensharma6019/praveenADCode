using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniOneSEO.Website.Models.SitemapXml
{
    public class SitemapXmlModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<SiteMapDetails> sitemapXML { get; set; }
    }
    public class SiteMapDetails
    {
        [SitecoreField(FieldId = "{0909DBA6-612C-4149-9123-8C9FFF56FAAE}")]
        public virtual string Url { get; set; }

        public virtual string Priority { get; set; }
        public virtual string Lastmod { get; set; }
        public virtual string DescPriority { get; set; }
        public virtual string ChangeFrequency { get; set; }
    }
}