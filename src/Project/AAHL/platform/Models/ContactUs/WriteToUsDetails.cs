using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.ContactUs
{
    public class WriteToUsDetails :ImageModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string SubHeading { get; set; }
        [SitecoreField]
        public virtual string Theme { get; set; }
    }
}