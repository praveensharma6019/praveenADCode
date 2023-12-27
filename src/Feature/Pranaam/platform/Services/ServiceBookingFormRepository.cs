using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class ServiceBookingFormRepository : IServiceBookingForm
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public ServiceBookingFormRepository(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }
        public ServiceBookingForm GetServiceBookingData(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            ServiceBookingForm bookingFormWidgits = new ServiceBookingForm();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;

                if (widget != null)
                {
                   
                    bookingFormWidgits.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    bookingFormWidgits.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                bookingFormWidgits.widget.widgetItems = GetServiceBooking(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetPranaamSteps throws exception -> " + ex.Message);
            }


            return bookingFormWidgits;
        }
        private List<Object> GetServiceBooking(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            return null;
        }
    }
}
