using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Common
{
    public class WidgetItemModel
    {
        [SitecoreField]
        public virtual string Title { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
        [SitecoreField]
        public virtual string Target { get; set; }
        [SitecoreField]
        public virtual string BtnText { get; set; }
        [SitecoreField]
        public virtual Image ImagePath { get; set; }
        [SitecoreField]
        public virtual Image MImagePath { get; set; }
        [SitecoreField]
        public virtual Image TImagePath { get; set; }
    }
}