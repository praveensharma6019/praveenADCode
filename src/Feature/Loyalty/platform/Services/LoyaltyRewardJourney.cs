using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
    public class LoyaltyRewardJourney : ILoyaltyRewardJourney
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        private readonly ILoyalty _loyalty;
        public LoyaltyRewardJourney(ILogRepository logRepository, IWidgetService widgetService, IHelper helper, ILoyalty loyalty)
        {

            this._logRepository = logRepository;
            this._widgetservice = widgetService;
            this._helper = helper;
            this._loyalty = loyalty;
        }

        public LoyaltyRewardsJourneyWidget getLoyaltyRewardJourneyData(Rendering rendering)
        {
            LoyaltyRewardsJourneyWidget loyaltyRewardJourneyWidgets = new LoyaltyRewardsJourneyWidget();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;

                if (widget != null)
                {
                    
                    loyaltyRewardJourneyWidgets.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    loyaltyRewardJourneyWidgets.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                loyaltyRewardJourneyWidgets.widget.widgetItems = GetLoyaltyRewardJourneyBannerData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("getLoyaltyData throws Exception -> " + ex.Message);
            }


            return loyaltyRewardJourneyWidgets;
        }

        private List<object> GetLoyaltyRewardJourneyBannerData(Rendering rendering)
        {
            List<object> _loyaltyrewardsJourneyObj = new List<object>();
            try
            {
                var loyaltyrewardJourneyDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (loyaltyrewardJourneyDatasource != null)
                {
                    LoyaltyJourneyModel loyaltyRewardJourneyModel = new LoyaltyJourneyModel
                    {
                        title = !string.IsNullOrEmpty(loyaltyrewardJourneyDatasource.Fields[Templates.LoyaltyRewardsJourneyBanner.Title].Value.ToString()) ? loyaltyrewardJourneyDatasource.Fields[Templates.LoyaltyRewardsJourneyBanner.Title].Value.ToString() : string.Empty,
                        subTitle = !string.IsNullOrEmpty(loyaltyrewardJourneyDatasource.Fields[Templates.LoyaltyRewardsJourneyBanner.SubTitle].Value.ToString()) ? loyaltyrewardJourneyDatasource.Fields[Templates.LoyaltyRewardsJourneyBanner.SubTitle].Value.ToString() : string.Empty,
                    };
                    loyaltyRewardJourneyModel.imageSrc = _helper.GetImageURL(loyaltyrewardJourneyDatasource, Templates.LoyaltyBannerFields.Image);
                    loyaltyRewardJourneyModel.imageAlt = _helper.GetImageAlt(loyaltyrewardJourneyDatasource, Templates.LoyaltyBannerFields.Image);
                  //  Loyalty objLoyalty = new Loyalty(_logRepository);
                    loyaltyRewardJourneyModel.rewardList = _loyalty.getRewardsList((Sitecore.Data.Fields.MultilistField)loyaltyrewardJourneyDatasource.Fields[Templates.LoyaltyBannerFields.RewardList]);
                    _loyaltyrewardsJourneyObj.Add(loyaltyRewardJourneyModel);

                }
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetLoyaltyBannerData throws Exception -> " + ex.Message);
            }
            return _loyaltyrewardsJourneyObj;
        }
    }
}