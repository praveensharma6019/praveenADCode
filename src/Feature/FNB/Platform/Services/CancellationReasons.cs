using Adani.SuperApp.Airport.Feature.Carousel.Platform;
using Adani.SuperApp.Airport.Feature.FNB.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public class CancellationReasons : ICancellationReasons
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public CancellationReasons(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetCancellationReasons(Rendering rendering, string queryString)
        {
           
            WidgetModel _outletstatus = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                _outletstatus.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                _outletstatus.widget.widgetItems = GetReasons(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetCancellationReasons gives -> " + ex.Message);
            }


            return _outletstatus;
        }

     

        private List<object> GetReasons(Rendering rendering)
        {
            List<Object> cancellationReason = new List<Object>();
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
                    Reasons reasons = null;
                    foreach (Sitecore.Data.Items.Item Item in datasource.Children)
                    {
                        reasons = new Reasons();
                        reasons.Code = !string.IsNullOrEmpty(Item.Fields[ReasonsCode.Code].Value.ToString()) ? Item.Fields[ReasonsCode.Code].Value.ToString() : string.Empty;
                        reasons.Label = !string.IsNullOrEmpty(Item.Fields[ReasonsCode.Label].Value.ToString()) ? Item.Fields[ReasonsCode.Label].Value.ToString() : string.Empty;
                        cancellationReason.Add(reasons);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" ParseOutletsatusdata gives -> " + ex.Message);
            }

            return cancellationReason;
        }
    }
}