using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.About_Us
{
    public class CertificationsModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<TabComponent> Items { get; set; }
    }

    public class TabComponent
    {
        public virtual string Heading { get; set; }
        public virtual string Subheading { get; set; }
        public virtual string Description { get; set; }
        public virtual bool Isactive { get; set; }
        public virtual Link LinkUrl { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<CertificationList> subItems { get; set; }
    }

    public class CertificationList
    {
        public virtual string Heading { get; set; }
        public virtual string Subheading { get; set; }
        public virtual Image ImagePath { get; set; }
        public virtual Image MImagePath { get; set; }
        public virtual Image TImagePath { get; set; }
    }
}
