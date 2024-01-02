using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AAHL.Website.Models.OurLeadership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Sustainability
{
    public class SustainabilityStoriesModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<SustainabilityStoriesModelItems> Items { get; set; }
    }

    public class SustainabilityStoriesModelItems : ImageModel
    {
        public virtual string Title { get; set; }
        public virtual Link LinkUrl { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<SustainabilityStoriesModelItemsList> Childitems { get; set; }
    }
    public class SustainabilityStoriesModelItemsList : ImageModel
    {
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string SubDescription { get; set; }
    }
}