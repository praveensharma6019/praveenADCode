using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AAHL.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Home
{
    public class AboutAirport
    {
        public virtual AboutAirportWidget Widget { get; set; }
    }

    public class AboutAirportWidget : WidgetModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<AboutAirportWidgetItem> WidgetItems { get; set; }
    }

    public class AboutAirportWidgetItem : WidgetItemModel
    {
       
    }


}