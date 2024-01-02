
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AAHL.Website.Models.Common;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Home
{
    public class PageMetaData
    {
        [SitecoreField]
        public virtual string PageName { get; set; }
        [SitecoreField]
        public virtual Link Url { get; set; }
        [SitecoreField]
        public virtual string Beardcrumb { get; set; }
        [SitecoreField]
        public virtual string H1Heading { get; set; }
        [SitecoreField]
        public virtual Link Canonical { get; set; }
        [SitecoreField]
        public virtual string Robotstag { get; set; }
        [SitecoreField]
        public virtual string Title { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual string Keywords { get; set; }
        [SitecoreField]
        public virtual string Ogtitle { get; set; }
        [SitecoreField]
        public virtual string Ogdescription { get; set; }
        [SitecoreField]
        public virtual string Ogkeywords { get; set; }
        [SitecoreField]
        public virtual Image Ogimage { get; set; }
        
    }

}