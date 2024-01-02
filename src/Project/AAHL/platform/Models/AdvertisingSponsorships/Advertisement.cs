using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AAHL.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.AdvertisingSponsorships
{
    public class Advertisement
    {

        [SitecoreChildren]
        public virtual IEnumerable<AdvertisementItem> Items { get; set; }
    }

    public class AdvertisementItem
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual bool Isactive { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<AdvertisementChilditems> Childitems { get; set; }
    }
    public class AdvertisementChilditems
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<Advertisementsubitems> Subitems { get; set; }
    }
    public class Advertisementsubitems : ImageModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
    }
}