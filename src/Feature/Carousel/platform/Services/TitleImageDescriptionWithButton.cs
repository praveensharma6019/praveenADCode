using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Security.Domains;
using static Adani.SuperApp.Airport.Feature.Carousel.Platform.Models.TitleDescriptionImageWithButtonModel;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class TitleImageDescriptionWithButton : ITitleImageDescriptionWithButton
    {
        //Ticket NO  18854
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public TitleImageDescriptionWithButton(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetData(Rendering rendering)
        {
            WidgetModel WidgetList = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                WidgetList.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                WidgetList.widget.widgetItems = GetDataList(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetData in TitleImageDescriptionWithButton gives -> " + ex.Message);
            }
            return WidgetList;
        }

        private List<object> GetDataList(Rendering rendering)
        {
            List<Object> DataList = new List<Object>();
            try
            {
                var datasourceItem = rendering.Item;
                // Null Check for datasource
                if (datasourceItem != null && datasourceItem.GetChildren() != null && datasourceItem.GetChildren().Count() > 0)
                {
                    TitleDescriptionImageWithButtonModel model = null;
                    Domains domains = null;
                    foreach (Sitecore.Data.Items.Item item in datasourceItem.GetChildren())
                    {
                        model = new TitleDescriptionImageWithButtonModel();
                        model.desktopImage = item.Fields[Constant.DeskImage] != null ? _helper.GetImageURL(item, Constant.DeskImage) : String.Empty;
                        model.mobileImage = item.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(item, Constant.MobileImage) : String.Empty;
                        model.title = item.Fields[Constant.Title] != null ? item.Fields[Constant.Title].Value : String.Empty;
                        model.description = item.Fields[Constant.Description] != null ? item.Fields[Constant.Description].Value : String.Empty;
                        model.subTitle = item.Fields[Constant.SubTitle1] != null ? item.Fields[Constant.SubTitle1].Value : String.Empty;
                        model.buttonText = item.Fields[Constant.ButtonText] != null ? item.Fields[Constant.ButtonText].Value : String.Empty;
                        model.isActive = item.Fields[Constant.IsActive].Value != null ? _helper.GetCheckboxOption(item.Fields[Constant.IsActive]) : false;

                        Item popup = item.GetChildren().FirstOrDefault(x => x.TemplateID == Constant.PopupTemplateId);
                        if(popup != null)
                        {
                            model.popupdetails.title = popup.Fields[Constant.Title] != null ? popup.Fields[Constant.Title].Value : String.Empty;
                            model.popupdetails.buttonText = popup.Fields[Constant.ButtonText] != null ? item.Fields[Constant.ButtonText].Value : String.Empty;
                            model.popupdetails.image = popup.Fields[Constant.DeskImage] != null ? _helper.GetImageURL(popup, Constant.DeskImage) : String.Empty;
                            model.popupdetails.subTitle = popup.Fields[Constant.SubTitle1] != null ? popup.Fields[Constant.SubTitle1].Value : String.Empty;
                        }

                        var domainItems = item.GetChildren().Where(x => x.TemplateID == Constant.DomainTemplateId).ToList();
                        if(domainItems.Count > 0 && domainItems != null)
                        foreach(var domainItem in domainItems)
                        {
                            domains = new Domains();
                            domains.value = domainItem.Fields[Constant.Value] != null ? domainItem.Fields[Constant.Value].Value : String.Empty;
                            domains.label = domainItem.Fields[Constant.Label] != null ? domainItem.Fields[Constant.Label].Value : String.Empty;
                            model.domainsList.Add(domains);
                        }

                        DataList.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetDataList in TitleImageDescriptionWithButton gives -> " + ex.Message);
            }
            return DataList;
        }

    }
}