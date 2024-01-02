using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AmbujaCement.Website.Models.Common;
using System.Collections.Generic;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.SiteMap
{
    [SitecoreType(TemplateId = "{34B3CE2A-5333-4F6C-8EFB-01AC6AFA6CDE}")]
    public class SitemapDataModel
    {
        [SitecoreField(FieldId = "{9A9ECAE1-6F51-4995-9E2B-5E7C763B4D8B}")]
        public virtual BannerSection BannerSection { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<SitemapLinks> SitemapLinks { get; set; }
    }

    [SitecoreType(TemplateId = "{AD590755-D52E-47E5-AA48-3A3E2E35F318}")]
    public class BannerSection : ImageSourceModel
    {
        
    }

    public class SitemapLinks //: GeneralLinkModel (Link as a string)
    {
        [SitecoreFieldAttribute(FieldId = "{7BE690ED-01D2-4F6B-8A18-7C2D58AD2AFF}")]
        public virtual Link LinkUrl { get; set; }
        public string Link
        {
            get
            {
                return LinkUrl?.Url;
            }

        }
        [SitecoreFieldAttribute(FieldId = "{A411F701-5EFD-4886-87AF-2A846BD18AFA}")]
        public virtual string LinkText { get; set; }

        [SitecoreFieldAttribute(FieldId = "{2E795C0F-28F1-4A3C-8C80-6665B4ABA7F7}")]
        public virtual string LinkTarget { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<SitemapInnerLinks> Items { get; set; }
    }

    public class SitemapInnerLinks //: GeneralLinkModel (Link as a string)
    {
        [SitecoreFieldAttribute(FieldId = "{7BE690ED-01D2-4F6B-8A18-7C2D58AD2AFF}")]
        public virtual Link LinkUrl { get; set; }
        public string Link
        {
            get
            {
                return LinkUrl?.Url;
            }

        }
        [SitecoreFieldAttribute(FieldId = "{A411F701-5EFD-4886-87AF-2A846BD18AFA}")]
        public virtual string LinkText { get; set; }

        [SitecoreFieldAttribute(FieldId = "{2E795C0F-28F1-4A3C-8C80-6665B4ABA7F7}")]
        public virtual string LinkTarget { get; set; }

        [SitecoreFieldAttribute(FieldId = "{1C468078-12C4-41CF-BF89-5728AE352962}")]
        public virtual GtmDetails GtmData { get; set; }
    }
}