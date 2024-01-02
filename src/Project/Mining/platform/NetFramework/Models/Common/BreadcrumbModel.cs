using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace Project.Mining.Website.Models
{
    public class BreadcrumbModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<Breadcrumblist> breadcrumbData { get; set; }

        public class Breadcrumblist
        {
            public virtual Link CTALink { get; set; }
            public virtual string Class { get; set; }

        }
       
     }
}