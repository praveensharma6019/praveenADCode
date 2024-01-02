using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AAHL.Website.Models.AboutUs;
using System;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.AboutAHHL
{
    public class OurAirportTabModel
    {
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<AirportListTab> Items { get; set; }
    }

    public class AirportListTab
    {
        public virtual string Heading { get; set; }
        public virtual Link LinkUrl { get; set; }
        public virtual bool Isactive { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<SubItemsTab> Items { get; set; }

    }
    public class SubItemsTab
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<DescriptionTabList> Items { get; set; }

    }

    public class DescriptionTabList : ImageModel
    {
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
    }

}