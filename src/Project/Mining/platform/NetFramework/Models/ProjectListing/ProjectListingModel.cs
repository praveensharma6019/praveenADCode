using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Models.ProjectListing
{
    public class ProjectListingModel : SectionHeadingModel
    {
        public virtual Link CTALink { get; set; }
        public virtual string CTAText { get; set; }
        public virtual string CtaClass { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<ProjectListinglist> OurServicesdata { get; set; }

        public class ProjectListinglist : ImageModel
        {
            public virtual string Heading { get; set; }
            public virtual string Description { get; set; }
            public virtual Link Link { get; set; }
        }
    }
}