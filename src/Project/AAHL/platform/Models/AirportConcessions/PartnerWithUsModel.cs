using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.AirportConcessions
{
    public class PartnerWithUsModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<PartnerWithUsChild> Items { get; set; }
    }
    public class PartnerWithUsChild : ImageModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual string Class { get; set; }
    }
}