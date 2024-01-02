using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Models.AboutUs
{
    public class LeadershipMessage 
    {
        [SitecoreField]
        public virtual string Heading { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<LeaderShipMessagelist> LeadershipMessageList { get; set; }
        public class LeaderShipMessagelist : ImageModel
        {
            public virtual string Title { get; set; }
            public virtual string Description { get; set; }
            public virtual string SubHeading { get; set; }
            public virtual Link CTALink { get; set; }
            public virtual Link Class { get; set; }
        }
    }
}