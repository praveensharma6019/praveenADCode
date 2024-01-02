using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AmbujaCement.Website.Models.Common;
using System.Collections.Generic;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.HomeBuilder
{
    public class OverviewModel : ImageSourceModel
    {
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

        [SitecoreFieldAttribute(FieldId = "{2E795C0F-28F1-4A3C-8C80-6665B4ABA7F7}")]
        public virtual string LinkTarget { get; set; }
        public virtual string readMore { get; set; }
        public virtual string readLess { get; set; }
        public virtual string paraHeading { get; set; }
        public virtual bool backHeader { get; set; }
    }
}