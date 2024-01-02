using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AmbujaCement.Website.Models.Common;
using System.Collections.Generic;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.HomeBuilder
{
    public class SubNavModel
    {
        public virtual string Heading { get; set; }
        public virtual string offcanvasHeading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<subNav> subNavItems { get; set; }
    }
    public class subNav
    {
        [SitecoreFieldAttribute(FieldId = "{9E0A0CFE-FC9E-4AAB-B08D-3C611450A4DD}")]
        public virtual bool active { get; set; }
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
        [SitecoreFieldAttribute(FieldId = "{455B2DBF-5FDA-4AD4-A329-E72CB8043E2E}")]
        public virtual GtmDetails gtmData { get; set; }
    }
}