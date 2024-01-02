using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.OurLeadership
{
    public class OurLeadersModel
    {
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<LeaderList> Items { get; set; }
    }
    public class LeaderList : ImageModel
    {
        public virtual string Title { get; set; }
        public virtual string Designation { get; set; }
        public virtual Link LinkUrl { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<LeaderDetail> Items { get; set; }
    }
    public class LeaderDetail : ImageModel
    {
        public virtual string Title { get; set; }
        public virtual string Designation { get; set; }
        public virtual string Subtitle { get; set; }
        public virtual string Class { get; set; }
        public virtual string Next { get; set; }
        public virtual string Previous { get; set; }
        public virtual string Description { get; set; }
        public virtual string SubDescription { get; set; }
        public virtual Link LinkUrl { get; set; }
    }
}