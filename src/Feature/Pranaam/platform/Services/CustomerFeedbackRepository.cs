using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class CustomerFeedbackRepository : ICustomerFeedback
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public CustomerFeedbackRepository(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }
        public CustomerFeedbackwidgets GetCustomerFeedback(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            CustomerFeedbackwidgets cfWidgits = new CustomerFeedbackwidgets();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;

                if (widget != null)
                {
                    
                    cfWidgits.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    cfWidgits.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                cfWidgits.widget.widgetItems = GetCustomerFeedbackData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetCustomerFeedback throws Exception -> " + ex.Message);
            }


            return cfWidgits;
        }
        private List<object> GetCustomerFeedbackData(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            _logRepository.Info("GetCustomerFeedbackData started");
            List<object> _cfObj = new List<object>();
            try
            {
                CustomerFeedback _obj = new CustomerFeedback();
                var feedbackDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (feedbackDatasource != null)
                {
                    _logRepository.Info("GetCustomerFeedbackData datasource not null");
                    _obj.title = !string.IsNullOrEmpty(feedbackDatasource.Fields[Templates.CustomerFeedback.Fields.Title].Value.ToString()) ? feedbackDatasource.Fields[Templates.CustomerFeedback.Fields.Title].Value.ToString() : "";
                    List<FeedbackCard> feedbackList = new List<FeedbackCard>();
                    if (((Sitecore.Data.Fields.MultilistField)feedbackDatasource.Fields[Templates.CustomerFeedback.Fields.FeedbackList]).Count > 0)
                    {
                        _logRepository.Info("GetCustomerFeedbackData list population");
                        foreach (Sitecore.Data.Items.Item item in ((Sitecore.Data.Fields.MultilistField)feedbackDatasource.Fields[Templates.CustomerFeedback.Fields.FeedbackList]).GetItems())
                        {
                            FeedbackCard cardItem = new FeedbackCard();
                            cardItem.title = !string.IsNullOrEmpty(item.Fields[Templates.CustomerFeedbackCard.Fields.Title].Value.ToString()) ? item.Fields[Templates.CustomerFeedbackCard.Fields.Title].Value.ToString() : "";
                            cardItem.description = !string.IsNullOrEmpty(item.Fields[Templates.CustomerFeedbackCard.Fields.Decription].Value.ToString()) ? item.Fields[Templates.CustomerFeedbackCard.Fields.Decription].Value.ToString() : "";
                            cardItem.imgSrc = _helper.GetImageURL(item, Templates.CustomerFeedbackCard.Fields.ServiceImage.ToString());
                            cardItem.alt = _helper.GetImageAlt(item, Templates.CustomerFeedbackCard.Fields.ServiceImage.ToString());
                            cardItem.mobileImage = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                            cardItem.mobileImageAlt = _helper.GetImageAlt(item, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                            cardItem.webImage = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                            cardItem.webImageAlt = _helper.GetImageAlt(item, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                            cardItem.thumbnailImage = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                            cardItem.thumbnailImageAlt = _helper.GetImageAlt(item, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                            feedbackList.Add(cardItem);
                        }
                        _obj.options = feedbackList;
                        _cfObj.Add(_obj);
                    }
                }
                else return null;
                _logRepository.Info("GetCustomerFeedbackData ended");
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetCustomerFeedbackData throws Exception -> " + ex.Message);
            }
            
            return _cfObj;
        }
    }
}