using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.OurBelief
{
    public class OurValuesModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<OurValuesList> Items { get; set; }
       
    }

    public class OurValuesList
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        public virtual string IconImage { get; set; }
        public virtual string Theme { get; set; }
        public virtual string Class { get; set; }
    }
}