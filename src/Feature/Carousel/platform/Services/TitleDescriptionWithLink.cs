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
    public class TitleDescriptionWithLink : ITitleDescriptionWithLink
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public TitleDescriptionWithLink(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel AboutAirport(Sitecore.Mvc.Presentation.Rendering rendering, Item faqItem)
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
                OfferListData.widget.widgetItems = Aboutairport(faqItem);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetOfferList gives -> " + ex.Message);
            }


            return OfferListData;
        }

        public List<object> Aboutairport(Item datasource)
        {
            List<Object> list = new List<Object>();
            if (datasource != null && datasource.Children.Count > 0)
            {
                TitleDescriptionWithLinkModel model;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    model = new TitleDescriptionWithLinkModel();
                    model.title = item.Fields[Constant.Title].Value;
                    model.description = item.Fields[Constant.Description].Value;
                    model.link = item.Fields[Constant.Link] != null ? _helper.GetLinkURL(item, Constant.Link) : String.Empty;
                    model.image = item.Fields[Constant.Image] != null ? _helper.GetImageURL(item, Constant.Image) : String.Empty;
                    model.autoId = item.Fields[Constant.AutoId].Value;
                    model.linkTarget = item.Fields[Constant.Link] != null ? _helper.LinkUrlTarget(item.Fields[Constant.Link]) : String.Empty;
                    model.isAgePopup = _helper.GetCheckboxOption(item.Fields[Constant.isAgePopup]);
                    model.isInternational = _helper.GetCheckboxOption(item.Fields[Constant.isInternational]);
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
                    list.Add(model);
                }
            }

            return list;
        }
    }
}