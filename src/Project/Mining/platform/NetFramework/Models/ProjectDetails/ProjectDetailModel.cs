using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System.Collections.Generic;

namespace Project.Mining.Website.Models.ProjectDetails
{
    public class ProjectDetailModel: SectionHeadingModel
    {
        public virtual Link CTALink { get; set; }
        public virtual string CtaClass { get; set; }

        public virtual string CTAText { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<ProjectsDetaillist> Projects { get; set; }
        public class ProjectsDetaillist
        {
            public virtual string Year { get; set; }
            public virtual string ProjectName { get; set; }
            public virtual string LocationHeading { get; set; }
            public virtual string Location { get; set; }
            public virtual string MineralHeading { get; set; }
            public virtual string MineralUsed { get; set; }
            public virtual string ProjectDetails { get; set; }
            public virtual string BgColor { get; set; }
            public virtual bool IsSelected { get; set; }
            public virtual Link Link { get; set; }
            [SitecoreChildren]
            public virtual IEnumerable<ItemDetaillist> items { get; set; }
            public class ItemDetaillist
            {
                public virtual string Title { get; set; }
                public virtual string Description { get; set; }
            }
        }
    }
}
