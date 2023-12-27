using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{

    public class SocialMediaLoyalty : ISocialMediaLoyalty
    {
        private readonly ILogRepository _logRepository;
        private readonly Foundation.Theming.Platform.Services.IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public SocialMediaLoyalty(ILogRepository logRepository, Foundation.Theming.Platform.Services.IWidgetService widgetService, IHelper helper)
        {
            this._logRepository = logRepository;
            this._widgetservice = widgetService;
            this._helper = helper;
        }
        public LoyaltyWidgets getMediaItems(Rendering rendering)
        {
            LoyaltyWidgets loyaltyWidgets = new LoyaltyWidgets();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;
                loyaltyWidgets.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new Foundation.Theming.Platform.Models.WidgetItem();
                loyaltyWidgets.widget.widgetItems = getSocialMediaData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("getMediaItems throws Exception -> " + ex.Message);
            }
            return loyaltyWidgets;
        }

        private List<object> getSocialMediaData(Rendering rendering)
        {
            List<object> objMedia = new List<object>();
            SocialMediaModel socialMediaModel =null;
            try
            {
                var socialMediaDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (socialMediaDatasource != null)
                {
                    foreach (Sitecore.Data.Items.Item socialMediaitem in socialMediaDatasource.Children)
                    {

                        socialMediaModel = new SocialMediaModel();
                        socialMediaModel.ctaLink = _helper.GetLinkURL(socialMediaitem, Templates.LoyaltyBannerFields.Link);
                        socialMediaModel.ctaText = _helper.GetLinkText(socialMediaitem, Templates.LoyaltyBannerFields.Link);
                        socialMediaModel.Media = getSocialMediaData((Sitecore.Data.Fields.MultilistField)socialMediaitem.Fields[Templates.Media.MediaField]);

                        objMedia.Add(socialMediaModel);
                    }



                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("getSocialMediaData throws Exception -> " + ex.Message);
            }
            return objMedia;
        }

        private List<Media> getSocialMediaData(MultilistField multilistField)
        {
            List<Media> objMediaList = new List<Media>();
            Media objMedia = null;
            try
            {
                if(multilistField!=null)
                {
                    foreach(Sitecore.Data.Items.Item item in multilistField.GetItems())
                    {
                        objMedia = new Media();
                        objMedia.Title =!string.IsNullOrEmpty(item.Fields[Templates.LoyaltyBannerFields.Title].Value)? item.Fields[Templates.LoyaltyBannerFields.Title].Value:string.Empty;
                        objMedia.MediaText= !string.IsNullOrEmpty(item.Fields[Templates.Media.RichText].Value) ? item.Fields[Templates.Media.RichText].Value : string.Empty;
                        objMediaList.Add(objMedia);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("getSocialMediaData throws Exception -> " + ex.Message);
            }
            return objMediaList;
        }
    }
}