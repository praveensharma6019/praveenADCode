using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Adani.SuperApp.Airport.Foundation.Widget.Models;

namespace Adani.SuperApp.Airport.Foundation.Widget.Services
{
    public class WidgetService
    {
        public WidgetItem GetWidgetItem(Item widget)
        {
            WidgetItem widgetItem = new WidgetItem();
            widgetItem.widgetId = !string.IsNullOrEmpty(widget.Fields[Constant.widgetId].Value.ToString()) ? widget.Fields[Constant.widgetId].Value.ToString() : "";
            widgetItem.widgetType = !string.IsNullOrEmpty(widget.Fields[Constant.widgetType].Value.ToString()) ? widget.Fields[Constant.widgetType].Value.ToString() : "";
            widgetItem.title = !string.IsNullOrEmpty(widget.Fields[Constant.title].Value.ToString()) ? widget.Fields[Constant.title].Value.ToString() : "";
            widgetItem.subTitle = !string.IsNullOrEmpty(widget.Fields[Constant.subTitle].Value.ToString()) ? widget.Fields[Constant.subTitle].Value.ToString() : "";
            widgetItem.subItemRadius = !string.IsNullOrEmpty(widget.Fields[Constant.subItemRadius].Value.ToString()) ? widget.Fields[Constant.subItemRadius].Value.ToString() : "";
            widgetItem.subItemWidth = !string.IsNullOrEmpty(widget.Fields[Constant.subItemWidth].Value.ToString()) ? widget.Fields[Constant.subItemWidth].Value.ToString() : "";
            widgetItem.gridColumn = !string.IsNullOrEmpty(widget.Fields[Constant.gridColumn].Value.ToString()) ? widget.Fields[Constant.gridColumn].Value.ToString() : "";
            widgetItem.aspectRatio = !string.IsNullOrEmpty(widget.Fields[Constant.aspectRatio].Value.ToString()) ? widget.Fields[Constant.aspectRatio].Value.ToString() : ""; 

            foreach (Sitecore.Data.Items.Item childItem in widget.GetChildren())
            {
                if (childItem.Name == "itemMargin")
                {
                    ItemCSS itemCss = new ItemCSS();
                    itemCss.left = !string.IsNullOrEmpty(childItem.Fields[Constant.left].Value.ToString()) ? childItem.Fields[Constant.left].Value.ToString() : "";
                    itemCss.right = !string.IsNullOrEmpty(childItem.Fields[Constant.right].Value.ToString()) ? childItem.Fields[Constant.right].Value.ToString() : "";
                    itemCss.bottom = !string.IsNullOrEmpty(childItem.Fields[Constant.bottom].Value.ToString()) ? childItem.Fields[Constant.bottom].Value.ToString() : "";
                    itemCss.top = !string.IsNullOrEmpty(childItem.Fields[Constant.top].Value.ToString()) ? childItem.Fields[Constant.top].Value.ToString() : "";
                    widgetItem.itemMargin = itemCss;
                }
                if (childItem.Name == "subItemMargin")
                {
                    ItemCSS subitemCss = new ItemCSS();                   
                    subitemCss.left = !string.IsNullOrEmpty(childItem.Fields[Constant.left].Value.ToString()) ? childItem.Fields[Constant.left].Value.ToString() : "";
                    subitemCss.right = !string.IsNullOrEmpty(childItem.Fields[Constant.right].Value.ToString()) ? childItem.Fields[Constant.right].Value.ToString() : "";
                    subitemCss.bottom = !string.IsNullOrEmpty(childItem.Fields[Constant.bottom].Value.ToString()) ? childItem.Fields[Constant.bottom].Value.ToString() : "";
                    subitemCss.top = !string.IsNullOrEmpty(childItem.Fields[Constant.top].Value.ToString()) ? childItem.Fields[Constant.top].Value.ToString() : "";
                    widgetItem.subItemMargin = subitemCss;
                }
                if (childItem.Name == "actionTitle")
                {
                    ActionTitle actionTitle = new ActionTitle();
                    actionTitle.name = !string.IsNullOrEmpty(childItem.Fields[Constant.name].Value.ToString()) ? childItem.Fields[Constant.name].Value.ToString() : "";
                    actionTitle.deeplink = !string.IsNullOrEmpty(childItem.Fields[Constant.deeplink].Value.ToString()) ? childItem.Fields[Constant.deeplink].Value.ToString() : "";
                    actionTitle.actionId = !string.IsNullOrEmpty(childItem.Fields[Constant.actionId].Value.ToString()) ? childItem.Fields[Constant.actionId].Value.ToString() : "";
                    widgetItem.actionTitle = actionTitle;
                }               
            }

            return widgetItem;
        }
    }
}