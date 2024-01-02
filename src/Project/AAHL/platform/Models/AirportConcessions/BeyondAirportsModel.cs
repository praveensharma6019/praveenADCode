using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.AirportConcessions
{
    public class BeyondAirportsModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<BeyondAirportsChild> Items { get; set; }
    }
    public class BeyondAirportsChild : ImageModel
    {
        [SitecoreField]
        public virtual bool isActive { get; set; }
    }
}