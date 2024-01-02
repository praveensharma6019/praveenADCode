using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AAHL.Website.Models.OurStory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Cargo
{
    public class OurWayForwardModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<ForwardDetails> Items { get; set; }
    }
    public class ForwardDetails 
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual bool isActive { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<OurWaydetails> Items { get; set; }
    }
    public class OurWaydetails : ImageModel
    {
        [SitecoreField]
        public virtual string Heading { get; set;}
        [SitecoreField]
        public virtual string Description { get; set;}
    }
}