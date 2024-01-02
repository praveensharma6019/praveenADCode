using Adani.SuperApp.Airport.Feature.Carousel.Platform;
using Adani.SuperApp.Airport.Feature.FNB.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public class OutletStatus : IOutletStatus
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public OutletStatus(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetOutletStatusData(Rendering rendering, string queryString, string storeType)
        {
           
            WidgetModel _outletstatus = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                _outletstatus.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                _outletstatus.widget.widgetItems = ParseOutletsatusdata(rendering, queryString, queryString);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetOutletStatusData gives -> " + ex.Message);
            }


            return _outletstatus;
        }

        private List<object> ParseOutletsatusdata(Rendering rendering, string queryString1, string queryString2)
        {
            List<Object> OutletStatusData = new List<Object>();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Info("Datasource not selected");
                }
                else
                {
                    OutletStatusModel _outletstatusModel = null;
                    foreach (Sitecore.Data.Items.Item StatusItem in datasource.Children)
                    {
                        _outletstatusModel = new OutletStatusModel();
                        _outletstatusModel.Title = !string.IsNullOrEmpty(StatusItem.Fields[OutletstoreStatus.Title].Value.ToString()) ? StatusItem.Fields[OutletstoreStatus.Title].Value.ToString() : string.Empty;
                        if ((Sitecore.Data.Fields.ImageField)StatusItem.Fields[OutletstoreStatus.Icon] != null)
                            _outletstatusModel.Icon = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)StatusItem.Fields[OutletstoreStatus.Icon]);
                        OutletStatusData.Add(_outletstatusModel);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" ParseOutletsatusdata gives -> " + ex.Message);
            }

            return OutletStatusData;
        }
    }
}