using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Models.Common
{
    public class ImageModel
    {
        [SitecoreField]
        public virtual Image Image { get; set; }
        [SitecoreField]
        public virtual Image MobileImage { get; set; }
        [SitecoreField]
        public virtual Image TabletImage { get; set; }
    }
}