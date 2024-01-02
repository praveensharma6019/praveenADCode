using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.OurAirports
{
    public class TravelReadyAirports
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<TravelReadyAirportsItem> Items { get; set; }
    }

    public class TravelReadyAirportsItem : ImageModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual bool Isactive { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
    }
}