using Glass.Mapper.Sc.Configuration.Attributes;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.OurStory
{
    public class FutureGrowthThroughExcellenace
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual string Summary { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<FutureGrowthThroughExcellenaceItem> Items { get; set; }
    }

    public class FutureGrowthThroughExcellenaceItem : ImageModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
    }
}