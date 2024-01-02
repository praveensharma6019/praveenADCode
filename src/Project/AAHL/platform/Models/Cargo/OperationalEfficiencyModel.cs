using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AAHL.Website.Models.OurStory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Cargo
{
    public class OperationalEfficiencyModel : ImageModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<OperationalDetails> Items { get; set; }
    }
    public class OperationalDetails 
    {
        [SitecoreField]
        public virtual string Title { get; set; }
        [SitecoreField]
        public virtual string Subtitle { get; set; }
        [SitecoreField]
        public virtual string iconImage { get; set; }
        [SitecoreField]
        public virtual string Class { get; set; }
    }
}