using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class TitleDescriptionWithLinkListService : ITitleDescriptionWithLinkList
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public TitleDescriptionWithLinkListService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel TitleDescriptionWithLinkListData(Sitecore.Mvc.Presentation.Rendering rendering, Item titleDescriptionWithLinkListItem)
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
                OfferListData.widget.widgetItems = TitleDescriptionWithLinkList(titleDescriptionWithLinkListItem);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetOfferList gives -> " + ex.Message);
            }
            return OfferListData;
        }

        public List<object> TitleDescriptionWithLinkList(Item datasource)
        {
            List<Object> TitleDescriptionWithLinkList = new List<Object>();
            if (datasource != null && datasource.Children.Count > 0)
            {
                TitleDescriptionWithLinkListModel titleDescriptionWithLinkListModel;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    titleDescriptionWithLinkListModel = new TitleDescriptionWithLinkListModel();
                    titleDescriptionWithLinkListModel.Title = item.Fields[Constant.Title].Value;
                    titleDescriptionWithLinkListModel.Description = item.Fields[Constant.Description].Value;
                    titleDescriptionWithLinkListModel.Title = item.Fields[Constant.Title].Value;
                    titleDescriptionWithLinkListModel.Image = item.Fields[Constant.Image] != null ? _helper.GetImageURL(item, Constant.Image) : String.Empty;
                    titleDescriptionWithLinkListModel.Link = item.Fields[Constant.Link] != null ? _helper.GetLinkURL(item, Constant.Link) : String.Empty;
                    titleDescriptionWithLinkListModel.AutoId = item.Fields[Constant.AutoId].Value;
                    titleDescriptionWithLinkListModel.UniqueId = item.Fields[Constant.UniqueId].Value;
                    titleDescriptionWithLinkListModel.MobileImage = item.Fields[Constant.Image] != null ? _helper.GetImageURL(item, Constant.MobileImage) : String.Empty;
                    titleDescriptionWithLinkListModel.linkTarget = item.Fields[Constant.Link] != null ? _helper.LinkUrlTarget(item.Fields[Constant.Link]) : String.Empty;
                    titleDescriptionWithLinkListModel.isAgePopup = _helper.GetCheckboxOption(item.Fields[Constant.isAgePopup]);
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
                    titleDescriptionWithLinkListModel.tags = tags;
                    TitleDescriptionWithLinkList.Add(titleDescriptionWithLinkListModel);
                }
            }

            return TitleDescriptionWithLinkList;
        }
    }
}
