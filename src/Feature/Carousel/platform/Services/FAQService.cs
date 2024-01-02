using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class FAQService : IFAQService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public FAQService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel FAQData(Sitecore.Mvc.Presentation.Rendering rendering, Item faqItem)
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
                OfferListData.widget.widgetItems = FAQList(faqItem);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetOfferList gives -> " + ex.Message);
            }


            return OfferListData;
        }
        public List<object> FAQList(Item datasource)
        {
            List<Object> faqList = new List<Object>();
            if (datasource != null && datasource.Children.Count > 0)
            {
                FAQ faqmodel;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    faqmodel = new FAQ();
                    faqmodel.title = item.Fields[Constant.FAQTitle].Value;
                    faqmodel.description = item.Fields[Constant.FAQDescription].Value;
                    faqmodel.autoId = item.Fields[Constant.AutoId].Value;
                    faqmodel.isAgePopup = _helper.GetCheckboxOption(item.Fields[Constant.isAgePopup]);
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
                    faqmodel.tags = tags;
                    faqList.Add(faqmodel);
                }
            }

            return faqList;
        }
    }
}