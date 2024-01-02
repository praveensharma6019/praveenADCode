using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
    public class EarnRewards : IEarnReward
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservices;
        public EarnRewards(ILogRepository logRepository, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._widgetservices = widgetService;
        }
        public LoyaltyWidgets getEarrewardData(Rendering rendering)
        {
            LoyaltyWidgets loyaltyWidgets = new LoyaltyWidgets();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;
                loyaltyWidgets.widget = widget != null ? loyaltyWidgets.widget = _widgetservices.GetWidgetItem(widget): new Foundation.Theming.Platform.Models.WidgetItem();
                loyaltyWidgets.widget.widgetItems = GetEarnRewardData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("getLoyaltyData throws Exception -> " + ex.Message);
            }


            return loyaltyWidgets;
        }

        private List<object> GetEarnRewardData(Rendering rendering)
        {
            List<object> _loyaltyObj = new List<object>();
            try
            {
                var earnrewardDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                EarnRewardModel earnRewardModel = null;
                if (earnrewardDatasource != null)
                {
                   foreach(Sitecore.Data.Items.Item earnrewarditem in earnrewardDatasource.Children)
                    {
                        
                        earnRewardModel = new EarnRewardModel
                        {
                            Title = !string.IsNullOrEmpty(earnrewarditem.Fields[Templates.LoyaltyBannerFields.Title].Value.ToString()) ? earnrewarditem.Fields[Templates.LoyaltyBannerFields.Title].Value.ToString() : string.Empty
                        };
                        
                        earnRewardModel.rewardStepList = getRewardStep((Sitecore.Data.Fields.MultilistField)earnrewarditem.Fields["Rewards Steps"]);
                        _loyaltyObj.Add(earnRewardModel);
                    }
                     
                    

                }
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetLoyaltyBannerData throws Exception -> " + ex.Message);
            }
            return _loyaltyObj;
        }

        private List<Object> getRewardStep(MultilistField multilistField)
        {
            List<Object> _rewardStep = new List<object>();
            
            try
            {
                RewardStep rewardStep1 = null;
                if (multilistField!=null)
                {
                    foreach(Sitecore.Data.Items.Item rewardStep in multilistField.GetItems())
                    {
                        rewardStep1 = new RewardStep
                        {
                            rewardsSteps = rewardStep.Fields[Templates.LoyaltyBannerFields.Title].Value
                        };
                        _rewardStep.Add(rewardStep1);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error("getRewardStep throws Exception -> " + ex.Message);
            }
            return _rewardStep;
        }
    }
}