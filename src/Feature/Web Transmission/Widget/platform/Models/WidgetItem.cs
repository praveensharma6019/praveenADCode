using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Feature.Widget.Platform.Models
{
    public class WidgetItem
    {
        public int widgetId { get; set; }
        public string widgetType { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public double subItemRadius { get; set; }
        public double subItemWidth { get; set; }
        public int gridColumn { get; set; }
        public double aspectRatio { get; set; }

        public ItemCSS itemMargin { get; set; }
        public ItemCSS subItemMargin { get; set; }

        public ActionTitle actionTitle { get; set; }

        public List<Object> widgetItems { get; set; }
    }
    public class ItemCSS
    {
        public double left { get; set; }
        public double right { get; set; }
        public double bottom { get; set; }
        public double top { get; set; }
    }

    public class ActionTitle
    {
        public int actionId { get; set; }
        public string deeplink { get; set; }
        public string name { get; set; }
    }
}