using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.AboutUs
{
    public class StoryModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<OurStoryList> Items { get; set; }
        
    }
    public class OurStoryList : ImageModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        public virtual Link LinkUrl { get; set; }
    }
}