using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Collections;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class RewardServices : IRewardServices
    {
        //Ticket NO  18854
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public RewardServices(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }
        public WidgetModel GetAdaniRewardsServices(Rendering rendering,string Location,string Type)
        {
            WidgetModel RewardListData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                RewardListData.widget= widget != null? _widgetservice.GetWidgetItem(widget): new WidgetItem();
                RewardListData.widget.widgetItems = GetAdaniRewardsData(rendering, Location,Type);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetAdaniRewardsServices gives -> " + ex.Message);
            }
            return RewardListData;
        }

        private List<object> GetAdaniRewardsData(Rendering rendering, string location,string Type)
        {
            List<Object> RewardDataList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasourceItem = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasourceItem != null)
                {
                 RewardsModal rewardsModal = null;
                var filterDataBasedOnLocation = GetFilteredRewardList(datasourceItem.GetChildren(), getLocationID(location),Type);
                foreach (Sitecore.Data.Items.Item rewardsItem in filterDataBasedOnLocation)
                {
                    var LocationType = GetLocationCode((Sitecore.Data.Fields.MultilistField)rewardsItem.Fields[Constant.LoyaltyRewards.Location], Constant.LoyaltyRewards.Location);
                    Sitecore.Data.Fields.CheckboxField Active = rewardsItem.Fields[Constant.LoyaltyRewards.Active];
                    rewardsModal = new RewardsModal {
                        title = !string.IsNullOrEmpty(rewardsItem.Fields[Constant.LoyaltyRewards.Title].Value) ? rewardsItem.Fields[Constant.LoyaltyRewards.Title].Value : string.Empty,
                        description = !string.IsNullOrEmpty(rewardsItem.Fields[Constant.LoyaltyRewards.Description].Value) ? rewardsItem.Fields[Constant.LoyaltyRewards.Description].Value : string.Empty,
                        descriptionApp = !string.IsNullOrEmpty(rewardsItem.Fields[Constant.LoyaltyRewards.DescriptionApp].Value) ? rewardsItem.Fields[Constant.LoyaltyRewards.DescriptionApp].Value : string.Empty,
                        rewardType = !string.IsNullOrEmpty(rewardsItem.Fields[Constant.LoyaltyRewards.Type].Value) ? rewardsItem.Fields[Constant.LoyaltyRewards.Type].Value : string.Empty,
                        ctaLink = _helper.GetLinkURL(rewardsItem, Constant.LoyaltyRewards.CTA),
                        ctaText = _helper.GetLinkText(rewardsItem, Constant.LoyaltyRewards.CTA),
                        desktopImageSrc = _helper.GetImageURL(rewardsItem, Constant.LoyaltyRewards.StanderedImage),
                        mobileImageSrc = _helper.GetImageURL(rewardsItem, Constant.LoyaltyRewards.ImageMobile),
                        rewardUniqueId = !string.IsNullOrEmpty(rewardsItem.Fields[Constant.LoyaltyRewards.RewardUniqueId].Value) ? rewardsItem.Fields[Constant.LoyaltyRewards.RewardUniqueId].Value : string.Empty,
                        location = LocationType,
                        active = Active.Checked
                    };
                    RewardDataList.Add(rewardsModal);
                }
                    
                }
                else
                    _logRepository.Error(" GetAdaniRewardsData data source is empty");
            }
            catch (Exception ex)
            {

                _logRepository.Error(" OfferDiscountService GetOfferListData gives -> " + ex.Message);
            }
            return RewardDataList;
        }

        private List<Item> GetFilteredRewardList(ChildList childList, string LocationID,string Type)
        {
            List<Item> childlst = new List<Item>();

            string appTypeID = (Type == Constant.LoyaltyRewards.Apptype) ? Constant.LoyaltyRewards.IsApp : Constant.LoyaltyRewards.IsWeb;
            if (!string.IsNullOrEmpty(LocationID.Trim()))
            {
                childlst = childList.Where(p => ((Sitecore.Data.Fields.CheckboxField)(p.Fields[Constant.LoyaltyRewards.Active])).Checked &&
                                                ((Sitecore.Data.Fields.CheckboxField)(p.Fields[appTypeID])).Checked &&
                                                   p[Constant.LoyaltyRewards.Location].Contains(LocationID)).ToList();
            }
            return childlst;
        }

        private string getLocationID(string location)
        {
            string LocationID = string.Empty;
            if (!string.IsNullOrEmpty(location))
            {
                switch (location.ToLower())
                {
                    case "svpia-ahmedabad-airport":
                        LocationID = Constant.Ahmedabad;
                        break;
                    case "lgbia-guwahati-airport":
                        LocationID = Constant.Guwahati;
                        break;
                    case "jaipur-airport":
                        LocationID = Constant.Jaipur;
                        break;
                    case "ccsia-lucknow-airport":
                        LocationID = Constant.Lucknow;
                        break;
                    case "thiruvananthapuram-airport":
                        LocationID = Constant.Thiruvananthapuram;
                        break;
                    case "mangaluru-airport":
                        LocationID = Constant.Mangaluru;
                        break;
                    case "csmia-mumbai-airport":
                        LocationID = Constant.Mumbai;
                        break;
                    case "adani-one-airport":
                        LocationID = Constant.adaniOne;
                        break;

                }
            }
            return LocationID;
        }

        private List<string> GetLocationCode(MultilistField multilistField, string terminalLocationType)
        {
            return multilistField != null ? multilistField.GetItems().Select(p => p.Fields[Constant.Title].Value).ToList() : new List<string>();
        }
    }
}