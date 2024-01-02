using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AAHL.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Home
{
    public class Banner
    {
        public virtual Widget Widget { get; set; }
    }

    public class Widget : WidgetModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<WidgetItem> WidgetItems { get; set; }
    }

    public class WidgetItem : WidgetItemModel
    {
        [SitecoreField]
        public virtual string TabletImage { get; set; }
        [SitecoreField]
        public virtual Image TImagePath { get; set; }
        [SitecoreField]
        public virtual string AltText { get; set; }
        [SitecoreField]
        public virtual string Direction { get; set; }
        [SitecoreField]
        public virtual bool videoEnable { get; set; }
        [SitecoreField]
        public virtual Link desktopVideo { get; set; }
        [SitecoreField]
        public virtual Link MVideo { get; set; }
        [SitecoreField]
        public virtual Link tVideo { get; set; }
    }

}