using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Extensions;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
    public class RewardsList : IRewardsList
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        private readonly ILoyalty loyalty;

        public RewardsList(ILogRepository logRepository, IWidgetService widgetService, ILoyalty loyalty)
        {
            this._logRepository = logRepository;
            this._widgetservice = widgetService;
            this.loyalty = loyalty;
        }

        public WidgetModel GetRewards(Rendering rendering)
        {
            WidgetModel loyaltyWidgets = new WidgetModel();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;

                if (widget != null)
                {
                    
                    loyaltyWidgets.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    loyaltyWidgets.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                loyaltyWidgets.widget.widgetItems = GetRewardsList(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetRewards throws Exception -> " + ex.Message);
            }


            return loyaltyWidgets;
        }

        public List<Object> GetRewardsList(Rendering rendering)
        {
            List<Object> rewardsList = new List<Object>();
            try
            {
                Item datasource = rendering.Item;
                if (datasource != null)
                {
                    RewardsListModel rewards = new RewardsListModel();
                    MultilistField listOfRewards = datasource.Fields[Templates.RewardsFields.RewardsList];
                    if (listOfRewards != null && listOfRewards.GetItems() != null)
                    {
                        rewards.rewardsList = loyalty.getRewardsList(listOfRewards);
                        rewardsList.Add(rewards);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetRewardsList throws Exception -> " + ex.Message);   
            }
            return rewardsList;
        }
    }
}