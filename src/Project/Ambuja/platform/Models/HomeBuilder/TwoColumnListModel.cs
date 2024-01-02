using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AmbujaCement.Website.Models.Common;
using System.Collections.Generic;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.HomeBuilder
{
    public class TwoColumnListModel
    {
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<columnlist> list { get; set; }
    }
    public class columnlist
    {
        [SitecoreFieldAttribute(FieldId = "{710EBAA2-B458-4B1C-BAED-97B1364ED2BB}")]
        public virtual Image IconUrl { get; set; }
        public string IconImage
        {
            get
            {
                return IconUrl?.Src;
            }
        }
        [SitecoreFieldAttribute(FieldId = "{76AD28FB-E6AB-42DE-BA60-BD1EED28CF19}")]
        public virtual string IconAlt { get; set; }
        public virtual string IconClass { get; set; }
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
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
        public virtual bool isReadmore { get; set; }
        public virtual string ReadMore { get; set; }
        public virtual string ReadLess { get; set; }

        [SitecoreFieldAttribute(FieldId = "{8D31338E-4F90-4493-968B-D28C9AB62B4F}")]
        public virtual GtmDetails gtmData { get; set; }
    }
}