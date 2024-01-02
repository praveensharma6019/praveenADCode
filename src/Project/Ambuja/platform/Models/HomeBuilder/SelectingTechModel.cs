using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AmbujaCement.Website.Models.Common;
using System.Collections.Generic;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.HomeBuilder
{
    public class SelectingTechModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<selectingTechList> selectingTech { get; set; }
    }
    public class selectingTechList
    {
        public virtual string Heading { get; set; }
        public virtual string Theme { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<Techlist> gallery { get; set; }
    }
    public class Techlist : ImageSourceModel
    {
        public virtual string CardHeading { get; set; }
        public virtual string Description { get; set; }
        public virtual string PlayIcon { get; set; }

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
        [SitecoreFieldAttribute(FieldId = "{C0312D38-4F2E-45F1-8250-8F5B81F9428F}")]
        public virtual GtmDetails gtmData { get; set; }
    }
}