using Glass.Mapper.Sc.Configuration.Attributes;

namespace Project.Mining.Website.Models.Common
{
    public class SectionHeadingModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual string SubHeading { get; set; }
    }
}