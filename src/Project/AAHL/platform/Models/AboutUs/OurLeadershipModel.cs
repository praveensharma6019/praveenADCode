using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AAHL.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.AboutUs
{
    public class OurLeadershipModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<OurLeadershipDetails> Items { get; set; }
    }
    public class OurLeadershipDetails : BannerDetails
    {
        public virtual string Heading { get; set; }

        public virtual string Designation { get; set; }
    }
}