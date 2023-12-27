using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

using Sitecore.Shell.Framework.Commands.TemplateBuilder;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class LeftImageWithDetailsServices : ILeftImageWithDetails
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public LeftImageWithDetailsServices(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel LeftImageWithDetailsData(Sitecore.Mvc.Presentation.Rendering rendering, Item leftImageWithDetailsItem)
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
                OfferListData.widget.widgetItems = LeftImageWithDetailsList(leftImageWithDetailsItem);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetOfferList gives -> " + ex.Message);
            }
            return OfferListData;
        }

        public List<object> LeftImageWithDetailsList(Item datasource)
        {

            List<Object> leftImageWithDetailsList = new List<Object>();
            if (datasource != null && datasource.Children.Count > 0)
            {
                LeftImageWithDetails leftImageWithDetailsModel;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    leftImageWithDetailsModel = new LeftImageWithDetails();
                    leftImageWithDetailsModel.name = item.Fields[Constant.name].Value;
                    leftImageWithDetailsModel.Description = item.Fields[Constant.Description].Value;
                    leftImageWithDetailsModel.backgroundColor = item.Fields[Constant.backgroundColor].Value;
                    leftImageWithDetailsModel.Title = item.Fields[Constant.Title].Value;
                    leftImageWithDetailsModel.Image = item.Fields[Constant.Image] != null ? _helper.GetImageURL(item, Constant.Image) : String.Empty;
                    leftImageWithDetailsModel.Link = item.Fields[Constant.Link] != null ? _helper.GetLinkURL(item, Constant.Link) : String.Empty;
                    leftImageWithDetailsModel.Video = item.Fields[Constant.RewardVideo] != null ? _helper.LinkUrl(item.Fields[Constant.RewardVideo]) : String.Empty;
                    leftImageWithDetailsModel.MobileVideo = item.Fields[Constant.RewardMobileVideo] != null ? _helper.LinkUrl(item.Fields[Constant.RewardMobileVideo]) : String.Empty;
                    leftImageWithDetailsModel.AutoId = item.Fields[Constant.AutoId].Value;
                    leftImageWithDetailsModel.UniqueId = item.Fields[Constant.UniqueId].Value;
                    leftImageWithDetailsModel.isAirportSelectNeeded = _helper.GetCheckboxOption(item.Fields[Constant.isAirportSelectNeeded]);
                    leftImageWithDetailsModel.textColor = item.Fields[Constant.textColor].Value;
                    leftImageWithDetailsModel.MobileImage = item.Fields[Constant.Image] != null ? _helper.GetImageURL(item, Constant.MobileImage) : String.Empty;
                    leftImageWithDetailsModel.linkTarget = item.Fields[Constant.Link] != null ? _helper.LinkUrlTarget(item.Fields[Constant.Link]) : String.Empty;
                    leftImageWithDetailsModel.isAgePopup = _helper.GetCheckboxOption(item.Fields[Constant.isAgePopup]);
                    leftImageWithDetailsModel.OfferEligibility = item.Fields[Constant.OfferEligibility].Value;
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
                    leftImageWithDetailsModel.tags = tags;
                    leftImageWithDetailsList.Add(leftImageWithDetailsModel);
                }
            }

            return leftImageWithDetailsList;
        }
    }
}