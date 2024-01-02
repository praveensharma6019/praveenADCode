using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System.Collections.Generic;
using static Project.Mining.Website.Models.WhoWeAreModel;

namespace Project.Mining.Website.Models
{
    public class WhoWeAreModel : GtmDataModel
    {
        public virtual Link CTALink { get; set; }
        public virtual string CTAText { get; set; }
        public virtual string Description { get; set; }
        public virtual string SubHeading { get; set; }
        public virtual string Heading { get; set; }
        public virtual string CtaClass { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<WhoWeAreDataList> WhoWeAreCards { get; set; }

        public class WhoWeAreDataList
        {
            public virtual string ShortDescription { get; set; }
            public virtual string Heading { get; set; }
            public virtual Image Icon { get; set; }
            public virtual Image AnimationImage { get; set; }

        }

    }
}