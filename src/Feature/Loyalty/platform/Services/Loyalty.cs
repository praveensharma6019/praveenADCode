using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
    public class Loyalty : ILoyalty
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public Loyalty(ILogRepository logRepository, IWidgetService widgetService,IHelper helper)
        {

            this._logRepository = logRepository;
            this._widgetservice = widgetService;
            this._helper = helper;
        }
        public LoyaltyWidgets getLoyaltyData(Rendering rendering)
        {
            LoyaltyWidgets loyaltyWidgets = new LoyaltyWidgets();
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
                loyaltyWidgets.widget.widgetItems = GetLoyaltyBannerData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("getLoyaltyData throws Exception -> " + ex.Message);
            }


            return loyaltyWidgets;
        }

        private List<object> GetLoyaltyBannerData(Rendering rendering)
        {
            List<object> _loyaltyObj = new List<object>();
            try
            {
                var loyaltyDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if(loyaltyDatasource!=null)
                {
                    LoyaltyModel loyaltyModel = new LoyaltyModel
                    {
                        title = !string.IsNullOrEmpty(loyaltyDatasource.Fields[Templates.LoyaltyBannerFields.Title].Value.ToString()) ? loyaltyDatasource.Fields[Templates.LoyaltyBannerFields.Title].Value.ToString() : string.Empty
                        
                    };
                    loyaltyModel.imageSrc = _helper.GetImageURL(loyaltyDatasource, Templates.LoyaltyBannerFields.Image);
                    loyaltyModel.imageAlt= _helper.GetImageAlt(loyaltyDatasource, Templates.LoyaltyBannerFields.Image);
                    loyaltyModel.ctaText = _helper.GetLinkText(loyaltyDatasource, Templates.LoyaltyBannerFields.Link);
                    loyaltyModel.ctaLink = _helper.GetLinkURL(loyaltyDatasource, Templates.LoyaltyBannerFields.Link);
                    loyaltyModel.appImageSrc= _helper.GetImageURL(loyaltyDatasource, Templates.LoyaltyBannerFields.AppImage);
                    loyaltyModel.appImageAlt = _helper.GetImageAlt(loyaltyDatasource, Templates.LoyaltyBannerFields.AppImage);
                    loyaltyModel.rewardList = getRewardsList((Sitecore.Data.Fields.MultilistField)loyaltyDatasource.Fields[Templates.LoyaltyBannerFields.RewardList]);
                    loyaltyModel.loyaltyWelcomeMessage = Convert.ToBoolean(rendering.Parameters["UseDictionary"]) ? Sitecore.Globalization.Translate.Text(Templates.DictionaryKey.newUserloyaltyMessage) : string.Empty;
                    _loyaltyObj.Add(loyaltyModel);

                }
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetLoyaltyBannerData throws Exception -> " + ex.Message);
            }
            return _loyaltyObj;
        }

        public List<Rewards> getRewardsList(MultilistField multilistField)
        {
            List<Rewards> rewardList = new List<Rewards>();
            Rewards objRewards = null;
            try
            {
                if(multilistField!=null)
                {
                    foreach(Sitecore.Data.Items.Item item in multilistField.GetItems())
                    {
                        objRewards = new Rewards();
                        objRewards.title =!string.IsNullOrEmpty(item.Fields[Templates.RewardsFields.Title].Value)? item.Fields[Templates.RewardsFields.Title].Value:string.Empty;
                        objRewards.description = !string.IsNullOrEmpty(item.Fields[Templates.RewardsFields.Descriptions].Value) ? item.Fields[Templates.RewardsFields.Descriptions].Value : string.Empty;
                        objRewards.standerdImageUrl = _helper.GetImageURL(item, Templates.RewardsFields.Image);
                        objRewards.standerdImageAlt = _helper.GetImageAlt(item, Templates.RewardsFields.Image);
                        objRewards.ctaLink = _helper.GetLinkURL(item, Templates.RewardsFields.Link);
                        objRewards.ctaText = _helper.GetLinkText(item, Templates.RewardsFields.Link);
                        objRewards.active = ((Sitecore.Data.Fields.CheckboxField)item.Fields[Templates.RewardsFields.Active]).Checked;
                        rewardList.Add(objRewards);

                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("getRewardsList throws Exception -> " + ex.Message);   
            }
            return rewardList;
        }
    }
}