using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services
{
    public class BrandListing : IBrandListing
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public BrandListing(ILogRepository logRepository, IWidgetService widgetService, IHelper helper)
        {
            this._logRepository = logRepository;
            this._widgetservice = widgetService;
            this._helper = helper;
        }
        public BrandListingWidget GetBrandListing(Rendering rendering, string restricted, string storeType , string airport)
        {
            BrandListingWidget brandListingWidgits = new BrandListingWidget();
            try
            {
                Item widget = null;
                widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                brandListingWidgits.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();               
                brandListingWidgits.widget.widgetItems = GetBranddata(rendering, restricted, storeType, airport);
            }
            catch (Exception ex)
            {
                _logRepository.Error(" BrandListing GetBrandListing gives -> " + ex.Message);

            }
            
            return brandListingWidgits;
        }

        private List<Object> GetBranddata(Rendering rendering, string restricted, string storeType, string airport)
        {
            List<Object> BrandList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    Sitecore.Diagnostics.Log.Error("Datasource not attached in brands", this);
                }
                
                Models.BrandListing brandListing;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    Sitecore.Data.Fields.CheckboxField chkRestricted = item.Fields[BrandListConstant.Restricted];

                    if (chkRestricted.Checked.ToString().ToLower().Equals(restricted))
                    {
                        string brandStoreType = !string.IsNullOrEmpty(item.Fields[BrandListConstant.StoreType].Value.ToString()) ? item.Fields[BrandListConstant.StoreType].Value.ToString().ToLower().Trim() : "";
                        if (brandStoreType.Equals(storeType))
                        {
                            if (!String.IsNullOrEmpty(item.Fields[BrandListConstant.BrandsList].ToString()))
                            {
                                Sitecore.Data.Fields.MultilistField multiselectField = item.Fields[BrandListConstant.BrandsList];
                               
                                foreach (Sitecore.Data.Items.Item brand in multiselectField.GetItems())
                                {
                                    brandListing = new Models.BrandListing();
                                    brandListing.title = !string.IsNullOrEmpty(brand.Fields[BrandListConstant.BrandName].Value.ToString()) ? _helper.ToTitleCase(brand.Fields[BrandListConstant.BrandName].Value.ToString()) : "";
                                    brandListing.cdnPath = !string.IsNullOrEmpty(brand.Fields[BrandListConstant.BrandCDNImage].Value.ToString()) ? brand.Fields[BrandListConstant.BrandCDNImage].Value.ToString() : "";
                                    brandListing.imageSrc = _helper.GetImageURL(brand, BrandListConstant.Image);
                                    brandListing.description = !string.IsNullOrEmpty(brand.Fields[BrandListConstant.BrandDescription].Value.ToString()) ? brand.Fields[BrandListConstant.BrandDescription].Value.ToString() : "";
                                    brandListing.code = !string.IsNullOrEmpty(brand.Fields[BrandListConstant.BrandCode].Value.ToString()) ? brand.Fields[BrandListConstant.BrandCode].Value.ToString() : "";
                                    brandListing.materialGroup = !string.IsNullOrEmpty(brand.Fields[BrandListConstant.BrandMaterialGroup].Value.ToString()) ? brand.Fields[BrandListConstant.BrandMaterialGroup].Value.ToString() : "";
                                    brandListing.brand = brandListing.code;
                                    brandListing.restricted = restricted.ToLower().Trim().Equals("true") ? true : false;
                                    brandListing.storeType = storeType;
                                    brandListing.disableForAirport = _helper.GetAvaialbilityOnAirport(brand, airport, storeType);                                    
                                    
                                    BrandList.Add(brandListing);
                                }
                            }
                        }
                        
                    }                    
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" BrandListing GetBranddata gives -> " + ex.Message);
            }
           
            return BrandList;
        }

    }
}