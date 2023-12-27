using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.Widget.Models
{
    public class WidgetItem
    {
        public string widgetId { get; set; }
        public string widgetType { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string subItemRadius { get; set; }
        public string subItemWidth { get; set; }
        public string gridColumn { get; set; }
        public string aspectRatio { get; set; }

        public ItemCSS itemMargin { get; set; }
        public ItemCSS subItemMargin { get; set; }

        public ActionTitle actionTitle { get; set; }

        public List<Object> widgetItems { get; set; }
    }
    public class ItemCSS
    {
        public string left { get; set; }
        public string right { get; set; }
        public string bottom { get; set; }
        public string top { get; set; }
    }

    public class ActionTitle
    {
        public string actionId { get; set; }
        public string deeplink { get; set; }
        public string name { get; set; }
    }
}