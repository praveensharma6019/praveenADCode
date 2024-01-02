using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Models
{
    public class WhatWeStandForModel
    {
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<WhatWeStandList> Cards { get; set; }

        public  class WhatWeStandList:ImageModel
        {
            public virtual string Heading { get; set; }
            public virtual string Description { get; set; }
            public virtual string SubHeading { get; set; }
           
        }
    }
}