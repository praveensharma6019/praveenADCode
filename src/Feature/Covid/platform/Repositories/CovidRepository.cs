using Adani.SuperApp.Airport.Feature.Covid.Interfaces;
using Adani.SuperApp.Airport.Feature.Covid.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.Covid.Repositories
{
    public class CovidRepository : ICovidUpdates
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public CovidRepository(ILogRepository logRepository, IWidgetService widgetService,IHelper helper)
        {

            this._logRepository = logRepository;
            this._widgetservice = widgetService;
            this._helper = helper;
        }
        public Covidwidgets GetCovidInformation(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Covidwidgets covidWidgits = new Covidwidgets();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;

                if (widget != null)
                {
                   
                    covidWidgits.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    covidWidgits.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                covidWidgits.widget.widgetItems = GetCovidInformationData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetCovidInformation throws Exception -> " + ex.Message);
            }


            return covidWidgits;
        }
        private List<object> GetCovidInformationData(Rendering rendering)
        {
            _logRepository.Info("GetCovidInformationData Initiated");
            List<object> _covidObj = new List<object>();
            try
            {
                CovidModel _obj = new CovidModel();
                var CovidDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (CovidDatasource != null)
                {
                    _logRepository.Info("CovidDatasource Not Null");
                    _obj.title = !string.IsNullOrEmpty(CovidDatasource.Fields[Templates.Covid.Fields.Summary].Value.ToString()) ? CovidDatasource.Fields[Templates.Covid.Fields.Summary].Value.ToString() : "";
                    _obj.text = !string.IsNullOrEmpty(CovidDatasource.Fields[Templates.Covid.Fields.Details].Value.ToString()) ? CovidDatasource.Fields[Templates.Covid.Fields.Details].Value.ToString() : "";
                    _obj.src = _helper.GetImageURL(CovidDatasource, Templates.Covid.Fields.Image.ToString());
                    _obj.alt = _helper.GetImageAlt(CovidDatasource, Templates.Covid.Fields.Image.ToString());
                    _obj.btn = _helper.GetLinkText(CovidDatasource, Templates.Covid.Fields.CTA.ToString());
                    _obj.btnUrl = _helper.GetLinkURL(CovidDatasource, Templates.Covid.Fields.CTA.ToString());
                    _obj.mobileImage = _helper.GetImageURL(CovidDatasource, Templates.Covid.Fields.MobileImage.ToString());
                    _obj.mobileImageAlt = _helper.GetImageURL(CovidDatasource, Templates.Covid.Fields.MobileImage.ToString());
                    _obj.webImage = _helper.GetImageURL(CovidDatasource, Templates.Covid.Fields.WebImage.ToString());
                    _obj.webImageAlt = _helper.GetImageURL(CovidDatasource, Templates.Covid.Fields.WebImage.ToString());
                    _obj.thumbnailImage = _helper.GetImageURL(CovidDatasource, Templates.Covid.Fields.ThumbnailImage.ToString());
                    _obj.thumbnailImageAlt = _helper.GetImageURL(CovidDatasource, Templates.Covid.Fields.ThumbnailImage.ToString());
                    List<CovidCarousel> CovidList = new List<CovidCarousel>();
                    if (((Sitecore.Data.Fields.MultilistField)CovidDatasource.Fields[Templates.Covid.Fields.CovidCarousal]).Count > 0)
                    {
                        _logRepository.Info("CovidCarousal count greater than 0");
                        foreach (Sitecore.Data.Items.Item item in ((Sitecore.Data.Fields.MultilistField)CovidDatasource.Fields[Templates.Covid.Fields.CovidCarousal]).GetItems())
                        {
                            CovidCarousel cardItem = new CovidCarousel();
                            cardItem.Summary = !string.IsNullOrEmpty(item.Fields[Templates.CovidCrausal.Fields.Summary].Value.ToString()) ? item.Fields[Templates.CovidCrausal.Fields.Summary].Value.ToString() : "";
                            cardItem.Details = !string.IsNullOrEmpty(item.Fields[Templates.CovidCrausal.Fields.Details].Value.ToString()) ? item.Fields[Templates.CovidCrausal.Fields.Details].Value.ToString() : "";
                            cardItem.Image = _helper.GetImageURL(item, Templates.CovidCrausal.Fields.Image.ToString());
                            CovidList.Add(cardItem);
                        }
                        _obj.CarousalItems = CovidList;
                    }
                    _covidObj.Add(_obj);
                }
                else return null;
                _logRepository.Info("GetCovidInformationData Ended");
            }
            catch(Exception ex)
            {
                _logRepository.Error("GetCovidInformationData throws Exception -> " + ex.Message);
            }
            
            return _covidObj;
        }
    }
}