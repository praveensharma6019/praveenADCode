using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System.Collections.Generic;

namespace Project.Mining.Website.Models
{
    public class PageBannerModel : ImageModel
    {
        public virtual Link CTALink { get; set; }
        public virtual string Description { get; set; }
        public virtual string SubHeading { get; set; }
        public virtual string Heading { get; set; }
    }
}