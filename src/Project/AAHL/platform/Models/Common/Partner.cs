using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Common
{
    public class Partner
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<PartnerItem> Items { get; set; }
    }

    public class PartnerItem
    {
        [SitecoreField]
        public virtual string Title { get; set; }
        [SitecoreField]
        public virtual string Designation { get; set; }
        [SitecoreField]
        public virtual string Class { get; set; }
        [SitecoreField]
        public virtual string LinkPrefix { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
    }
}