using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AAHL.Website.Models.Partnerships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.ContactUs
{
    public class Address
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Title { get; set; }
        [SitecoreField]
        public virtual string Subtitle { get; set; }
        [SitecoreField]
        public virtual string No { get; set; }
        [SitecoreField]
        public virtual string NoPrefix { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<ImageModel> Items { get; set; }
    }
}