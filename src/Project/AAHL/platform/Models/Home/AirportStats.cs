using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AAHL.Website.Models.Common;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.Home
{
    public class AirportStats
    {
        public virtual WidgetList Widget { get; set; }
    }

    public class WidgetList : WidgetModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<WidgetItemList> WidgetItems { get; set; }
    }

    public class WidgetItemList
    {
        [SitecoreField]
        public virtual string Value { get; set; }
        [SitecoreField]
        public virtual string Sign { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
    }
}