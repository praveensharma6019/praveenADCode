using Adani.SuperApp.Airport.Feature.Filters.Platform.Models;
using Sitecore.Mvc.Presentation;
using System;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.Filters.Platform.Services
{
    public class FilterProductsService : IFilterProductsService
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetService;

        public FilterProductsService(ILogRepository _logRepository,IWidgetService widgetService)
        {
            this._logRepository = _logRepository;
            this._widgetService = widgetService;
        }
        public FilterProductsWidgets GetProductFilters(Rendering rendering)
        {
            FilterProductsWidgets filterProductsWidgets = new FilterProductsWidgets();

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
                    //WidgetService widgetService = new WidgetService();
                    filterProductsWidgets.widget = _widgetService.GetWidgetItem(widget);
                }
                else
                {
                    filterProductsWidgets.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                filterProductsWidgets.widget.widgetItems = GetProductFilterList(rendering);
            }
            catch(Exception ex)
            {
                _logRepository.Error(" FilterProductsService GetProductFilters gives -> " + ex.Message);
            }

            return filterProductsWidgets;
        }

        private List<Object> GetProductFilterList(Rendering rendering)
        {
            List<Object> productFilterList = new List<Object>();

            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                FilterProducts filterProducts;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    filterProducts = new FilterProducts();
                    filterProducts.title = !string.IsNullOrEmpty(item.Fields[Constant.displayName].Value.ToString()) ? item.Fields[Constant.displayName].Value.ToString() : "";
                    filterProducts.apiUrl = !string.IsNullOrEmpty(item.Fields[Constant.apiUrl].Value.ToString()) ? item.Fields[Constant.apiUrl].Value.ToString() : "";
                    
                    filterProducts.materialGroup = !string.IsNullOrEmpty(item.Fields[Constant.MaterialGroup].Value.ToString()) ? item.Fields[Constant.MaterialGroup].Value.ToString() : "";
                    filterProducts.category = !string.IsNullOrEmpty(item.Fields[Constant.Category].Value.ToString()) ? item.Fields[Constant.Category].Value.ToString() : "";
                    filterProducts.subCategory = !string.IsNullOrEmpty(item.Fields[Constant.SubCategory].Value.ToString()) ? item.Fields[Constant.SubCategory].Value.ToString() : "";
                    filterProducts.brand = !string.IsNullOrEmpty(item.Fields[Constant.Brand].Value.ToString()) ? item.Fields[Constant.Brand].Value.ToString() : "";
                    filterProducts.showOnHomepage = (item.Fields[Constant.ShowOnHomepage].Value.ToString() == "1") ? true : false;
                    filterProducts.newArrival = (item.Fields[Constant.NewArrival].Value.ToString() == "1") ? true : false;
                    filterProducts.popular = (item.Fields[Constant.Popular].Value.ToString() == "1") ? true : false;
                    filterProducts.skuCode = !string.IsNullOrEmpty(Convert.ToString(item.Fields[Constant.SKUCode])) ? item.Fields[Constant.SKUCode].Value.ToString() : "";


                    productFilterList.Add(filterProducts);
                }

            }
            catch (Exception ex)
            {
                 _logRepository.Error(" FilterProductsService GetProductFilterList gives -> " + ex.Message);
            }
            return productFilterList;
        }
    }
}