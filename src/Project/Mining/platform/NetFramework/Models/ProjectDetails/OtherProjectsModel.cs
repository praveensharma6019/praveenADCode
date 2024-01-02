using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Models.ProjectDetails
{
    public class OtherProjectsModel : SectionHeadingModel
    {
        public virtual string BgColor { get; set; }
        public virtual string CTAText { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<OtherProjectslist> Projects { get; set; }
        public class OtherProjectslist: ImageModel
        {
            public virtual string Heading { get; set; }
            public virtual string SubHeading { get; set; }
            public virtual string Description { get; set; }
            public virtual Link CtaLink { get; set; }
            public virtual string CtaClass { get; set; }
            public virtual string BgColor { get; set; }
        }
    }
}