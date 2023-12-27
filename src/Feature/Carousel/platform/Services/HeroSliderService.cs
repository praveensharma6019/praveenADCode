using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    
    public class HeroSliderService : IHeroSliderService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public HeroSliderService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel HeroSliderData(Sitecore.Mvc.Presentation.Rendering rendering, Item faqItem)
        {
            WidgetModel OfferListData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);

                if (widget != null)
                {

                    OfferListData.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    OfferListData.widget = new WidgetItem();
                }
                OfferListData.widget.widgetItems = GetHeroSliderData(faqItem);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetOfferList gives -> " + ex.Message);
            }


            return OfferListData;
        }

        public List<object> GetHeroSliderData(Item datasource)
        {
            List<Object> list = new List<Object>();
            if (datasource != null && datasource.Children.Count > 0)
            {
                HeroSliderModel model;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    model = new HeroSliderModel();
                    model.title = item.Fields[Constant.Title].Value;
                    model.description = item.Fields[Constant.Description].Value;                   
                    model.imagesrc =item.Fields[Constant.Image] != null ? _helper.GetImageURL(item, Constant.Image) : String.Empty;
                    model.autoid = item.Fields[Constant.AutoId].Value;
                    model.bannerlogo = item.Fields[Constant.Image] != null ? _helper.GetImageURL(item, Constant.Image) : String.Empty;
                    model.subtitle = item.Fields[Constant.SubTitle1].Value;
                    model.uniqueid = item.Fields[Constant.UniqueId].Value;
                    model.mobileimage = item.Fields[Constant.MobileImage] != null ? _helper.GetImageURL(item, Constant.MobileImage) : String.Empty;
                    model.btnText = item.Fields[Constant.btnText].Value;
                    model.isAirportSelectNeeded = _helper.GetCheckboxOption(item.Fields[Constant.isAirportSelectNeeded]);
                    model.Link = item.Fields[Constant.Link] != null ? _helper.GetLinkURL(item, Constant.Link) : String.Empty;
                    model.isAgePopup = _helper.GetCheckboxOption(item.Fields[Constant.isAgePopup]);
                    model.linkTarget = item.Fields[Constant.Link] != null ? _helper.LinkUrlTarget(item.Fields[Constant.Link]) : String.Empty;
                    model.offerEligibility = item.Fields[Constant.OfferEligibility].Value;
                    model.cardBgColor= item.Fields[Constant.cardBgColor].Value;
                    model.listClass= item.Fields[Constant.listClass].Value;
                    model.gridNumber = Convert.ToInt32(item.Fields[Constant.gridNumber].Value);
                    GTMTags tags = new GTMTags
                    {
                        bannerCategory = item[Constant.bannerCategory],
                        faqCategory = item[Constant.faqCategory],
                        subCategory = item[Constant.subCategory],
                        category = item[Constant.Category],
                        businessUnit = item[Constant.businessUnit],
                        label = item[Constant.label],
                        source = item[Constant.source],
                        type = item[Constant.type],
                        eventName = item[Constant.eventName]
                    };
                    model.tags = tags;
                    model.checkValidity = item.Fields[Constant.CheckValidity] != null ? _helper.GetCheckboxOption(item.Fields[Constant.CheckValidity]) : false;
                    if (model.checkValidity)
                    {
                        Sitecore.Data.Fields.DateField EffFrom = item.Fields[Constant.effectiveFrom];
                        model.effectiveFrom = EffFrom != null ? EffFrom.DateTime.ToString() : string.Empty;
                        Sitecore.Data.Fields.DateField EffTo = item.Fields[Constant.effectiveTo];
                        model.effectiveTo = EffTo != null ? EffTo.DateTime.ToString() : string.Empty;
                        if (_helper.getISTDateTime((System.DateTime.Now).ToString()) >= Convert.ToDateTime(model.effectiveFrom) && Convert.ToDateTime(model.effectiveTo) >= _helper.getISTDateTime((System.DateTime.Now).ToString()))
                            list.Add(model);
                    }
                    else
                        list.Add(model);
                }
            }
            return list;
        }
    }
}