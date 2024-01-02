using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.BrandListing.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Repositories;
using Adani.SuperApp.Airport.Foundation.Widget.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.BrandListing.Services
{
    public class BrandListing : IBrandListing
    {
        private readonly ILogRepository _logRepository;
        public BrandListing(ILogRepository logRepository)
        {

            this._logRepository = logRepository;
        }
        public BrandListingWidget GetBrandListing(Rendering rendering)
        {
            BrandListingWidget brandListingWidgits = new BrandListingWidget();
            try
            {
                Item widget = null;
                IDictionary<string, string> paramDictionary = rendering.Parameters.ToDictionary(pair => pair.Key, pair => pair.Value);
                foreach (string key in paramDictionary.Keys.ToList())
                {
                    if (Sitecore.Data.ID.TryParse(paramDictionary[key], out var itemId))
                    {
                        widget = rendering.RenderingItem.Database.GetItem(itemId);
                    }
                }
                if (widget != null)
                {
                    WidgetService widgetService = new WidgetService();
                    brandListingWidgits.widget = widgetService.GetWidgetItem(widget);
                }
                else
                {
                    brandListingWidgits.widget = new  Foundation.Widget.Models.WidgetItem();
                }
                brandListingWidgits.widget.widgetItems = GetBranddata(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(" BrandListing GetBrandListing gives -> " + ex.Message);

            }
            
            return brandListingWidgits;
        }

        private List<Object> GetBranddata(Rendering rendering)
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
                    Sitecore.Data.Fields.CheckboxField chkvivible = item.Fields[Constant.Visibleonbrandyoulove];
                    brandListing = new Models.BrandListing();
                    brandListing.BrandName = !string.IsNullOrEmpty(item.Fields[Constant.BrandName].Value.ToString()) ? item.Fields[Constant.BrandName].Value.ToString() : "";
                    brandListing.Image = Foundation.SitecoreHelper.Helper.Helper.GetImageURL(item, Constant.Image);
                    brandListing.BrandDescription = !string.IsNullOrEmpty(item.Fields[Constant.BrandDescription].Value.ToString()) ? item.Fields[Constant.BrandDescription].Value.ToString() : "";
                    brandListing.BrandCode = !string.IsNullOrEmpty(item.Fields[Constant.BrandCode].Value.ToString()) ? item.Fields[Constant.BrandCode].Value.ToString() : "";
                    brandListing.Visibleonbrandyoulove = chkvivible.Checked;
                    BrandList.Add(brandListing);
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