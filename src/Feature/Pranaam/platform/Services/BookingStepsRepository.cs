using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class BookingStepsRepository : IBookingSteps
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public BookingStepsRepository(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }
        public ServiceStepslwidgets GetPranaamSteps(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            ServiceStepslwidgets stepsWidgits = new ServiceStepslwidgets();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;

                if (widget != null)
                {
                    
                    stepsWidgits.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    stepsWidgits.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                stepsWidgits.widget.widgetItems = GetPranaamData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetPranaamSteps throws exception -> " + ex.Message);
            }


            return stepsWidgits;
        }
        private List<Object> GetPranaamData(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            _logRepository.Info("GetPranaamData started");
            List<Object> _listObj = new List<object>();
            try
            {
                List<Steps> StepsList = new List<Steps>();
                var renDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (renDatasource != null && renDatasource.Children.Count > 0)
                {
                    _logRepository.Info("GetPranaamData datasource not null");
                    foreach (Sitecore.Data.Items.Item item in renDatasource.Children)
                    {
                        Steps cardItem = new Steps();
                        cardItem.title = !string.IsNullOrEmpty(item.Fields[Templates.Steps.Fields.BookingTitle].Value.ToString()) ? item.Fields[Templates.Steps.Fields.BookingTitle].Value.ToString() : "";
                        cardItem.alt = _helper.GetImageAlt(item, Templates.Steps.Fields.BookingImg.ToString());
                        cardItem.imgSrc = _helper.GetImageURL(item, Templates.Steps.Fields.BookingImg.ToString());
                        cardItem.value = !string.IsNullOrEmpty(item.Fields[Templates.Steps.Fields.BookingValue].Value.ToString()) ? item.Fields[Templates.Steps.Fields.BookingValue].Value.ToString() : "";
                        cardItem.mobileImage = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                        cardItem.mobileImageAlt = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                        cardItem.webImage = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                        cardItem.webImageAlt = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                        cardItem.thumbnailImage = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                        cardItem.thumbnailImageAlt = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                        _listObj.Add(cardItem);
                    }
                }
                else return null;
                _logRepository.Info("GetPranaamData ended");
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetPranaamData throws exception -> " + ex.Message);
            }

            return _listObj;
        }
    }
}