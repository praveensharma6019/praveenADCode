using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AAHL.Website.Models.Partnerships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Our_Expertise
{
    public class RetailDevelopmentModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        public virtual string SubDescription { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<ImageModel> Items { get; set; }
    }
}