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
    public class SapAPI : ISapAPI
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public SapAPI(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetSapAPIData(Rendering rendering, string queryString, string storeType)
        {
            //FnBExclusiveOutlet fnBExclusiveOutlet = new FnBExclusiveOutlet();
            WidgetModel fnBSapApi = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                fnBSapApi.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                fnBSapApi.widget.widgetItems = ParseSapApidata(rendering, queryString, queryString);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetSapAPIData gives -> " + ex.Message);
            }


            return fnBSapApi;
        }

        private List<object> ParseSapApidata(Rendering rendering, string queryString1, string queryString2)
        {
            List<Object> SapAPIData = new List<Object>();
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
                    SapApiModel sapApiModel = null;
                    foreach (Sitecore.Data.Items.Item sapItem in datasource.Children)
                    {
                        sapApiModel = new SapApiModel();
                        sapApiModel.Title = !string.IsNullOrEmpty(sapItem.Fields[FnbSapApi.Title].Value.ToString()) ? sapItem.Fields[FnbSapApi.Title].Value.ToString() : string.Empty;
                        //sapApiModel.CTAText = _helper.GetLinkText(sapItem, "CTALink");
                        sapApiModel.CTALink = _helper.GetLinkURL(sapItem, "Link");
                        SapAPIData.Add(sapApiModel);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" ParseSapApidata gives -> " + ex.Message);
            }

            return SapAPIData;
        }
    }
}