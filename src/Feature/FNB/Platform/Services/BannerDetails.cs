using Adani.SuperApp.Airport.Feature.Carousel.Platform;
using Adani.SuperApp.Airport.Feature.FNB.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.Portal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public class BannerDetails : IBannerDetails
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public BannerDetails(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetBannerDetails(Rendering rendering, string bannerCode)
        {
           
            WidgetModel _outletstatus = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                _outletstatus.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                _outletstatus.widget.widgetItems = GetDetails(rendering, bannerCode);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetBannerDetails gives -> " + ex.Message);
            }


            return _outletstatus;
        }

     

        private List<object> GetDetails(Rendering rendering, string bannerCode)
        {
            List<Object> bannerD = new List<Object>();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Info("Datasource not selected");
                }
                else
                {
                    BannerDetail details = null;
                    foreach (Sitecore.Data.Items.Item Item in datasource.Children)
                    {
                        details = new BannerDetail();
                        details.BannerCode = !string.IsNullOrEmpty(Item.Fields[Carousels.BannerCode].Value.ToString()) ? Item.Fields[Carousels.BannerCode].Value.ToString() : string.Empty;
                        details.Title = !string.IsNullOrEmpty(Item.Fields[Carousels.Title].Value.ToString()) ? Item.Fields[Carousels.Title].Value.ToString() : string.Empty;
                        if (details.BannerCode == bannerCode) {
                         details.Location= GetItemName(Item.Fields[Carousels.LocationType].Value.ToString());
                         details.Terminal= GetItemName(Item.Fields[Carousels.TerminalType].Value.ToString());
                         details.StoreType= GetItemName(Item.Fields[Carousels.TerminalStoreType].Value.ToString());
                            if ((Sitecore.Data.Fields.ImageField)Item.Fields[Carousels.ImageSrc] != null)
                                details.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)Item.Fields[Carousels.ImageSrc]);
                            details.CTALink = _helper.GetLinkURL(Item, Carousels.CTALink);
                            details.CTALinkText = _helper.GetLinkText(Item, Carousels.CTALink);
                            details.Description = !string.IsNullOrEmpty(Item.Fields[Carousels.Description].Value.ToString()) ? Item.Fields[Carousels.Description].Value.ToString() : string.Empty;
                            if ((Sitecore.Data.Fields.ImageField)Item.Fields[Carousels.ThumbnailImage] != null)
                                details.ThumbnailImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)Item.Fields[Carousels.ThumbnailImage]);
                            if ((Sitecore.Data.Fields.ImageField)Item.Fields[Carousels.MobileImage] != null)
                                details.MobileImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)Item.Fields[Carousels.MobileImage]);
                            Sitecore.Data.Fields.MultilistField multilistField = Item.Fields[Bankoffers.ApplicableOutlets];
                            List<Item> applicableOutlets = multilistField.GetItems().ToList();

                            List<string> outletIDs = new List<string>();
                            foreach (Sitecore.Data.Items.Item item in applicableOutlets)
                            {
                                string outletID = item.Fields[Bankoffers.OutletCode].Value;
                                outletIDs.Add(outletID);

                            }
                            var JoinedID = string.Join(",", outletIDs);
                            if (outletIDs.Count > 1)
                            {
                                details.UniqueId = "101";
                            }
                            else
                            {
                                if (outletIDs.Count == 1)
                                    details.UniqueId = "100";
                            }

                            details.OutletID = JoinedID;
                            details.Storecode = JoinedID;
                            bannerD.Add(details);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetDetails gives -> " + ex.Message);
            }

            return bannerD;
        }

        public string GetItemName(string itemID) {
            string itemName = string.Empty;
            if (itemID.Contains('|')) {
                var splitID = itemID.Split('|');
                List<string> addedName= new List<string>();
                foreach (string splitItemID in splitID) { 
                var name= Sitecore.Context.Database.GetItem(splitItemID).Name;
                    addedName.Add(name);

                }

                string CombinedName= string.Join(",", addedName);
                return CombinedName;
            }
            itemName=Sitecore.Context.Database.GetItem(itemID).Name;
            return itemName;
        }
    }
}