using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System.Collections.Generic;

namespace Project.Mining.Website.Models.Services
{
    public class ServiceOurProjectsModel : SectionHeadingModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<OurProjectsList> OurProjectsList { get; set; }
        public virtual Link Link { get; set; }
    }
    public class OurProjectsList : ImageModel
    {
        public virtual string Heading { get; set; }
        public virtual string SubHeading { get; set; }
        public virtual string Description { get; set; }
        public virtual Link Link { get; set; }
    }
}