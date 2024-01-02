using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AAHL.Website.Models.AboutUs;
using System;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.AboutAHHL
{
    public class OurAirportModel
    {
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<AirportList> Items { get; set; }
    }

    public class AirportList
    {
        public virtual string Heading { get; set; }
        public virtual Link LinkUrl { get; set; }
        public virtual bool Isactive { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<SubItems> Items { get; set; }

    }
    public class SubItems
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<ItemsHeader> Items { get; set; }

    }

    public class ItemsHeader
    {
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<Itemdetail> Items { get; set; }
    }
    public class Itemdetail : ImageModel
    {
        public virtual string Subheading { get; set; }
        public virtual string Heading { get; set; }
        public virtual bool Isactive { get; set; }
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<DetailList> Items { get; set; }
    }

    public class DetailList : DetailModel
    {
        public virtual string Subheading { get; set; }
    }

    public class DescriptionList : ImageModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
    }

}