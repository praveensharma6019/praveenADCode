using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.OurBelief
{
    public class ServiceExcellenceModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<ServiceExcellenceList> Items { get; set; }
    }
    public class ServiceExcellenceList
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        public virtual Image ImagePath { get; set; }
        public virtual Image MImagePath { get; set; }
        public virtual Image TImagePath { get; set; }
        public virtual string Class { get; set; }
    }
}