using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AAHL.Website.Models.GeneralAviation;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.GroundTransportation
{
    public class TravelExperience
    {
        [SitecoreChildren]
        public virtual IEnumerable<TravelData> Items { get; set; }
    }
    public class TravelData : ImageModel
    {
        [SitecoreField]
        public virtual string title { get; set; }
        [SitecoreField]
        public virtual string subtitle { get; set; }
        [SitecoreField]
        public virtual string iconClass { get; set; }
        [SitecoreField]
        public virtual string Class { get; set; }
    }
}