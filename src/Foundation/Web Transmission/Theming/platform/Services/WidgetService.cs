using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;
using Adani.BAU.Transmission.Foundation.Theming.Platform.Models;
using Adani.BAU.Transmission.Foundation.Logging.Platform.Repositories;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;

namespace Adani.BAU.Transmission.Foundation.Theming.Platform.Services
{
    public class WidgetService : IWidgetService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;

        public WidgetService(IHelper helper, ILogRepository logRepository)
        {

            this._helper = helper;
            this._logRepository = logRepository;
        }
        public WidgetItem GetWidgetItem(Item widget)
        {
            WidgetItem widgetItem = new WidgetItem();
            //LinkResolver linkResolver = new LinkResolver();
            try
            {
                widgetItem.widgetId = !string.IsNullOrEmpty(widget.Fields[Constant.widgetId].Value.ToString()) ? Convert.ToInt16(widget.Fields[Constant.widgetId].Value.Trim()) : 0;
                widgetItem.widgetType = !string.IsNullOrEmpty(widget.Fields[Constant.widgetType].Value.ToString()) ? widget.Fields[Constant.widgetType].Value.ToString() : "";
                widgetItem.title = !string.IsNullOrEmpty(widget.Fields[Constant.title].Value.ToString()) ? widget.Fields[Constant.title].Value.ToString() : "";
                widgetItem.subTitle = !string.IsNullOrEmpty(widget.Fields[Constant.subTitle].Value.ToString()) ? widget.Fields[Constant.subTitle].Value.ToString() : "";
                widgetItem.subItemRadius = !string.IsNullOrEmpty(widget.Fields[Constant.subItemRadius].Value.ToString()) ? Convert.ToDouble(widget.Fields[Constant.subItemRadius].Value.Trim()) : 0;
                widgetItem.subItemWidth = !string.IsNullOrEmpty(widget.Fields[Constant.subItemWidth].Value.ToString()) ? Convert.ToDouble(widget.Fields[Constant.subItemWidth].Value.Trim()) : 0;
                widgetItem.gridColumn = !string.IsNullOrEmpty(widget.Fields[Constant.gridColumn].Value.ToString()) ? Convert.ToInt16(widget.Fields[Constant.gridColumn].Value.Trim()) : 0;
                widgetItem.aspectRatio = !string.IsNullOrEmpty(widget.Fields[Constant.aspectRatio].Value.ToString()) ? Convert.ToDouble(widget.Fields[Constant.aspectRatio].Value.Trim()) : 0;
                widgetItem.borderRadius = !string.IsNullOrEmpty(widget.Fields[Constant.BorderRadius].Value.ToString()) ? Convert.ToDouble(widget.Fields[Constant.BorderRadius].Value.Trim()) : 0;

                foreach (Sitecore.Data.Items.Item childItem in widget.GetChildren())
                {
                    if (childItem.TemplateID == Templates.WidgetItemsCollection.ItemMarginTemplateID)
                    {
                        ItemCSS itemCss = new ItemCSS();
                        itemCss.left = !string.IsNullOrEmpty(childItem.Fields[Constant.left].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.left].Value.Trim()) : 0;
                        itemCss.right = !string.IsNullOrEmpty(childItem.Fields[Constant.right].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.right].Value.Trim()) : 0;
                        itemCss.bottom = !string.IsNullOrEmpty(childItem.Fields[Constant.bottom].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.bottom].Value.Trim()) : 0;
                        itemCss.top = !string.IsNullOrEmpty(childItem.Fields[Constant.top].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.top].Value.Trim()) : 0;
                        widgetItem.itemMargin = itemCss;
                    }
                    if (childItem.TemplateID == Templates.WidgetItemsCollection.SubItemMarginTemplateID)
                    {
                        ItemCSS subitemCss = new ItemCSS();
                        subitemCss.left = !string.IsNullOrEmpty(childItem.Fields[Constant.left].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.left].Value.Trim()) : 0;
                        subitemCss.right = !string.IsNullOrEmpty(childItem.Fields[Constant.right].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.right].Value.Trim()) : 0;
                        subitemCss.bottom = !string.IsNullOrEmpty(childItem.Fields[Constant.bottom].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.bottom].Value.Trim()) : 0;
                        subitemCss.top = !string.IsNullOrEmpty(childItem.Fields[Constant.top].Value.ToString()) ? Convert.ToDouble(childItem.Fields[Constant.top].Value.Trim()) : 0;
                        widgetItem.subItemMargin = subitemCss;
                    }
                    if (childItem.TemplateID == Templates.WidgetItemsCollection.ActionTitleTemplateID)
                    {
                        ActionTitle actionTitle = new ActionTitle();
                        actionTitle.name = !string.IsNullOrEmpty(childItem.Fields[Constant.name].Value.ToString()) ? childItem.Fields[Constant.name].Value.ToString() : "";
                        actionTitle.deeplink = _helper.LinkUrl(childItem.Fields[Constant.deeplink]);
                        actionTitle.actionId = !string.IsNullOrEmpty(childItem.Fields[Constant.actionId].Value.ToString()) ? Convert.ToInt16(childItem.Fields[Constant.actionId].Value.Trim()) : 0;
                        widgetItem.actionTitle = actionTitle;
                    }
                    if (childItem.Name == "carouselParam")
                    {
                        CarouselParam carouselParam = new CarouselParam();
                        carouselParam.enlargeCenterPage = !string.IsNullOrEmpty(childItem.Fields[Constant.enlargeCenterPage].Value.ToString()) ? childItem.Fields[Constant.enlargeCenterPage].Value.ToString() : "";
                        carouselParam.enableInfiniteScroll = !string.IsNullOrEmpty(childItem.Fields[Constant.enableInfiniteScroll].Value.ToString()) ? childItem.Fields[Constant.enableInfiniteScroll].Value.ToString() : "";
                        carouselParam.autoPlay = !string.IsNullOrEmpty(childItem.Fields[Constant.autoPlay].Value.ToString()) ? childItem.Fields[Constant.autoPlay].Value.ToString() : "";
                        carouselParam.viewportFraction = !string.IsNullOrEmpty(childItem.Fields[Constant.viewportFraction].Value.ToString()) ? childItem.Fields[Constant.viewportFraction].Value.ToString() : "";
                        widgetItem.carouselParam = carouselParam;
                    }

                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("Widget Item Exception -> " + ex.Message);
            }
            

            return widgetItem;
        }
    }
}