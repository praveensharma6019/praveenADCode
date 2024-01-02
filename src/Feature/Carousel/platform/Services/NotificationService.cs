using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class NotificationService : INotificationService
    {
        //Ticket NO  18854
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public NotificationService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetNotificationData(Rendering rendering)
        {
            WidgetModel widgetList = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                widgetList.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                widgetList.widget.widgetItems = GetNotificationDataList(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetNotificationData in NotificationService gives -> " + ex.Message);
            }
            return widgetList;
        }

        private List<object> GetNotificationDataList(Rendering rendering)
        {
            List<Object> DataList = new List<Object>();
            try
            {
                Item datasourceItem = rendering.Item;
                // Null Check for datasource
                if (datasourceItem != null && datasourceItem.GetChildren() != null && datasourceItem.GetChildren().Count() > 0)
                {
                    NotificationModel model = null;
                    foreach (Item item in datasourceItem.GetChildren())
                    {
                        model = new NotificationModel();
                        model.type = item.Fields[Constant.type] != null ? item.Fields[Constant.type].Value : string.Empty;
                        model.description = item.Fields[Constant.Description] != null ? item.Fields[Constant.Description].Value : string.Empty;
                        model.moreButton = item.Fields[Constant.MoreButton] != null ? _helper.GetCheckboxOption(item.Fields[Constant.MoreButton]) : false;
                        model.ctaUrl = item.Fields[Constant.Link] != null ? _helper.GetLinkURL(item , Constant.Link) : string.Empty;
                        model.bgcolor= item.Fields[Constant.BGColor] != null ? item.Fields[Constant.BGColor].Value : string.Empty;
                        model.txtcolor = item.Fields[Constant.TextColor] != null ? item.Fields[Constant.TextColor].Value : string.Empty;
                        model.iconcolor = item.Fields[Constant.IconColor] != null ? item.Fields[Constant.IconColor].Value : string.Empty;
                        model.mwebenable = item.Fields[Constant.MobileEnable] != null ? _helper.GetCheckboxOption(item.Fields[Constant.MobileEnable]) : false;
                        model.desktopenable = item.Fields[Constant.DesktopEnable] != null ? _helper.GetCheckboxOption(item.Fields[Constant.DesktopEnable]) : false;
                        DataList.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetNotificationDataList in NotificationService gives -> " + ex.Message);
            }
            return DataList;
        }

    }
}