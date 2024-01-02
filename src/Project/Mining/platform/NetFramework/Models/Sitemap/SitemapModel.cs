using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using Sitecore.Pipelines.Rules.Taxonomy;
using System.Collections.Generic;

namespace Project.Mining.Website.Models.Sitemap
{
    [SitecoreType(TemplateId = "{C1CD0466-EA2F-46C9-9385-5716A68A7475}")]
    public class SitemapModel
    {
        [SitecoreField(FieldId = "{7E4433B8-1169-4AA5-BEEB-9E753171B618}")]
        public virtual BannerSection BannerSection { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<SitemapLinks> SitemapLinks { get; set; }
    }

    [SitecoreType(TemplateId = "{0F4677EB-9BDA-40C6-BAC4-972D0970E533}")]
    public class BannerSection : ImagesModel
    {

    }
    [SitecoreType(TemplateId = "{F8FA5ACD-95FF-45BD-A032-237D03E34598}")]
    public class SitemapLinks 
    {

        [SitecoreField(FieldId = "{DB2322B4-1437-4BA5-B116-83B649AE6656}")]
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<SitemapInnerLinks> Items { get; set; }
    }
    [SitecoreType(TemplateId = "{AE037FC9-561C-4E34-AD15-5EAE6DC7A1D6}")]
    public class SitemapInnerLinks : GtmDataModel
    {
        [SitecoreField(FieldId = "{E5C5C2E8-99FB-48DC-A1C0-203D547B4EFA}")]
        public virtual Link Link { get; set; }
        [SitecoreFieldAttribute(FieldId = "{492E4D47-52E0-44DE-A351-B703862DF225}")]
        public virtual GtmDataModel GtmData { get; set; }
    }
}