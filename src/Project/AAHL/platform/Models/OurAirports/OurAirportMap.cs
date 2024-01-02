using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.OurAirports
{
    public class OurAirportMap : ImageModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<AirportsMapItem> airportList { get; set; }
    }
    public class AirportsMapItem
    {
        public virtual string Title { get; set; }
        public virtual Link LinkUrl { get; set; }
        public virtual MapPosition DesktopPosition { get; set; }
        public virtual MapPosition MPosition { get; set; }
        public virtual MapPosition TPosition { get; set; }
    }

    public class MapPosition
    {
        public virtual string Left { get; set;}
        public virtual string Top { get; set;}
    }
}