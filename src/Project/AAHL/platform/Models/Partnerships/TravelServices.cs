using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Partnerships
{
    public class TravelServices
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<TravelServicesItem> Items { get; set; }
    }

    public class TravelServicesItem : ImageModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
        [SitecoreField]
        public virtual string IconClass { get; set; }
        [SitecoreField]
        public virtual string Title { get; set; }
    }
}