using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class HowItWorkSteps :IHowItWorkSteps
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public HowItWorkSteps(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }
        public HowItWorkStepsWidgets GetStepDetails(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            HowItWorkStepsWidgets stepsWidgits = new HowItWorkStepsWidgets();
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
                stepsWidgits.widget.widgetItems = GetData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetStepDetails throws exception -> " + ex.Message);
            }


            return stepsWidgits;
        }
        private List<Object> GetData(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            _logRepository.Info("GetData started");
            List<object> _stepsObj = new List<object>();
            try
            {
                HowItWorkStepsModel _obj = new HowItWorkStepsModel();
                var ds = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (ds != null)
                {
                    _logRepository.Info("HowItWorkSteps datasource not null");
                    _obj.Title = !string.IsNullOrEmpty(ds.Fields[Templates.HowItWorkSteps.Fields.Title].Value.ToString()) ? ds.Fields[Templates.HowItWorkSteps.Fields.Title].Value.ToString() : "";
                    _obj.CTAUrl = _helper.GetLinkURL(ds, Templates.HowItWorkSteps.Fields.CTA.ToString());
                    _obj.CTAText = _helper.GetLinkText(ds, Templates.HowItWorkSteps.Fields.CTA.ToString());
                    _obj.AppCTAUrl = _helper.GetLinkURL(ds, Templates.HowItWorkSteps.Fields.AppCTA.ToString());
                    _obj.AppCTAText = _helper.GetLinkText(ds, Templates.HowItWorkSteps.Fields.AppCTA.ToString());
                    List<StepsTab> tabList = new List<StepsTab>();
                    if (((Sitecore.Data.Fields.MultilistField)ds.Fields[Templates.HowItWorkSteps.Fields.Tabs]).Count > 0)
                    {
                        _logRepository.Info("GetTabsData list population");
                        foreach (Sitecore.Data.Items.Item item in ((Sitecore.Data.Fields.MultilistField)ds.Fields[Templates.HowItWorkSteps.Fields.Tabs]).GetItems())
                        {
                            StepsTab cardItem = new StepsTab();
                            cardItem.Title = !string.IsNullOrEmpty(item.Fields[Templates.HowItWorkStepsTabContent.Fields.Title].Value.ToString()) ? item.Fields[Templates.HowItWorkStepsTabContent.Fields.Title].Value.ToString() : "";
                            List<Cards> _cardList = new List<Cards>();
                            if(((Sitecore.Data.Fields.MultilistField)item.Fields[Templates.HowItWorkStepsTabContent.Fields.Card]).Count > 0)
                            {
                                foreach(Sitecore.Data.Items.Item card in ((Sitecore.Data.Fields.MultilistField)item.Fields[Templates.HowItWorkStepsTabContent.Fields.Card]).GetItems())
                                {
                                    Cards _cardObj = new Cards();
                                    _cardObj.Title = !string.IsNullOrEmpty(card.Fields[Templates.HowItWorkStepsCard.Fields.Title].Value.ToString()) ? card.Fields[Templates.HowItWorkStepsCard.Fields.Title].Value.ToString() : "";
                                    _cardObj.Description = !string.IsNullOrEmpty(card.Fields[Templates.HowItWorkStepsCard.Fields.Description].Value.ToString()) ? card.Fields[Templates.HowItWorkStepsCard.Fields.Description].Value.ToString() : "";
                                    _cardObj.Value = !string.IsNullOrEmpty(card.Fields[Templates.HowItWorkStepsCard.Fields.Value].Value.ToString()) ? card.Fields[Templates.HowItWorkStepsCard.Fields.Value].Value.ToString() : "";
                                    _cardObj.mobileImage = _helper.GetImageURL(card, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                                    _cardObj.mobileImageAlt = _helper.GetImageAlt(card, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                                    _cardObj.webImage = _helper.GetImageURL(card, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                                    _cardObj.webImageAlt = _helper.GetImageAlt(card, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                                    _cardObj.SrcImage = _helper.GetImageURL(card, Templates.PranaamPackages.Fields.StanderedImage.ToString());
                                    _cardObj.SrcImageAlt = _helper.GetImageAlt(card, Templates.PranaamPackages.Fields.StanderedImage.ToString());
                                    _cardObj.thumbnailImage = _helper.GetImageURL(card, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                                    _cardObj.thumbnailImageAlt = _helper.GetImageAlt(card, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                                    _cardList.Add(_cardObj);
                                }
                                cardItem.Cards = _cardList;                                
                            }
                            tabList.Add(cardItem);
                        }                        
                        _obj.TabContent = tabList;
                        _stepsObj.Add(_obj);
                    }
                }
                else return null;
                _logRepository.Info("GetData ended");
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetData throws Exception -> " + ex.Message);
            }

            return _stepsObj;
        }
    }
}