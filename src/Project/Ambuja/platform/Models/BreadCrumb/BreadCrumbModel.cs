using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AmbujaCement.Website.Models.Common;
using System.Collections.Generic;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.BreadCrumb
{
    public class BreadCrumbModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<BreadCrumbListdata> Items { get; set; }
        public virtual string Theme { get; set; }
        public class BreadCrumbListdata            // : LinkUrlAsStringModel   (Uncomment if required for string Link)
        {
            public virtual bool Active { get; set; }
            [SitecoreFieldAttribute(FieldId = "{7BE690ED-01D2-4F6B-8A18-7C2D58AD2AFF}")]
            public virtual Link LinkUrl { get; set; }
            public string Link
            {
                get
                {
                    return LinkUrl?.Url;
                }

            }

            [SitecoreFieldAttribute(FieldId = "{2E795C0F-28F1-4A3C-8C80-6665B4ABA7F7}")]
            public virtual string LinkTarget { get; set; }

            [SitecoreFieldAttribute(FieldId = "{A411F701-5EFD-4886-87AF-2A846BD18AFA}")]
            public virtual string LinkText { get; set; }

            [SitecoreFieldAttribute(FieldId = "{1C468078-12C4-41CF-BF89-5728AE352962}")]
            public virtual GtmDetails GtmData { get; set; }
        }
        
    }
}