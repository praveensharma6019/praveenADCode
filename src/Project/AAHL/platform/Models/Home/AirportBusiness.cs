using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AAHL.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Home
{
    public class AirportBusiness
    {
        public virtual AirportBusinessWidget Widget { get; set; }
    }

    public class AirportBusinessWidget : WidgetModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<AirportBusinessWidgetItem> WidgetItems { get; set; }
    }

    public class AirportBusinessWidgetItem : WidgetItemModel
    {

    }
}