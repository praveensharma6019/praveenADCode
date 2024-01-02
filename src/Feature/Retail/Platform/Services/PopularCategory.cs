using Adani.SuperApp.Airport.Feature.Retail.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Newtonsoft.Json;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Query;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Item = Sitecore.Data.Items.Item;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.Services
{
    public class PopularCategory : IPopularCategory
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public PopularCategory(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetPopularCategories(Sitecore.Mvc.Presentation.Rendering rendering, string storeType, string location, string terminaltype, string outletcode)
        {

            WidgetModel retailBrandsDetails = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constants.RenderingParamField]);
                retailBrandsDetails.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                retailBrandsDetails.widget.widgetItems = GetAllPopularCategories(rendering, storeType, location, terminaltype, outletcode);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetPopularCategories  gives -> " + ex.Message);
            }


            return retailBrandsDetails;
        }

        private List<Object> GetAllPopularCategories(Rendering rendering, string storeType, string location, string terminaltype,string storeOutletCode)
        {
            List<Object> popularCategories = new List<Object>();
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
                    string Temp = PopularCategoryConstants.PopularCategoryTemplateID.ToString();
                    List<Item> filteredTemplateItems = datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList();


                    List<Item> FilteredPopularList = GetFilteredPopularCategories(filteredTemplateItems, getstoreTypeID(storeType), getLocationID(location), getTerminalID(terminaltype), storeOutletCode);


                    foreach(Sitecore.Data.Items.Item categories in FilteredPopularList)
                    {
                        PopularCategoryModel category = new PopularCategoryModel();
                        category.Title= !string.IsNullOrEmpty(categories.Fields[Constants.Title].Value.ToString()) ? categories.Fields[Constants.Title].Value.ToString() : string.Empty;
                       List<Categories> categoriesList = new List<Categories>();
                        if (categories.Children != null && categories.Children.Count() > 0)
                        {
                            foreach (Sitecore.Data.Items.Item Subcategory in categories.GetChildren())
                            {
                                Categories categoryModel = new Categories();
                                categoryModel.CategoryTitle = !string.IsNullOrEmpty(Subcategory.Fields[Constants.Title].Value.ToString()) ? Subcategory.Fields[Constants.Title].Value.ToString() : string.Empty;
                                List<CategoryDetails> details = new List<CategoryDetails>();
                                if (Subcategory.Children != null && Subcategory.Children.Count() > 0)
                                {
                                    foreach (Sitecore.Data.Items.Item categoryDetails in Subcategory.GetChildren())
                                    {
                                        CategoryDetails categoryDetailsModel = new CategoryDetails();
                                        categoryDetailsModel.Title = !string.IsNullOrEmpty(categoryDetails.Fields[Constants.Title].Value.ToString()) ? categoryDetails.Fields[Constants.Title].Value.ToString() : string.Empty;
                                        categoryDetailsModel.Description = !string.IsNullOrEmpty(categoryDetails.Fields[Constants.Description].Value.ToString()) ? categoryDetails.Fields[Constants.Description].Value.ToString() : string.Empty;
                                        categoryDetailsModel.Image = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)categoryDetails.Fields[Constants.Image]);
                                        categoryDetailsModel.CTA = _helper.GetLinkText(categoryDetails, Constants.CTA);
                                        details.Add(categoryDetailsModel);
                                    }
                                }
                                categoryModel.CategoryDetails = details;

                                categoriesList.Add(categoryModel);
                            }
                        
                        }
                        category.Categories= categoriesList;
                        popularCategories.Add(category);
                    }
                   

                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetAllPopularCategories gives -> " + ex.Message);

            }
            return popularCategories;
        }

        private List<Item> GetFilteredPopularCategories(List<Sitecore.Data.Items.Item> childList, string storeType, string location, string terminaltype,string outletCode)
        {

            List<Item> childlst = new List<Item>();
            List<Item> filteredList = new List<Item>();

            childlst = childList.Where(p => p[PopularCategoryConstants.LocationType].Contains(location)
                                            && p[PopularCategoryConstants.TerminalStoreType].Contains(storeType)
                                            && p[PopularCategoryConstants.TerminalType].Contains(terminaltype)).ToList();

            foreach (Sitecore.Data.Items.Item filteredOutlets in childlst) {
                Sitecore.Data.Fields.MultilistField multilistField = filteredOutlets.Fields[PopularCategoryConstants.ApplicableOutlets];
                List<Item> applicableOutlets = multilistField.GetItems().ToList();

                var list = applicableOutlets.Where(a => a.Fields["Outlet Code"].Value.Equals(outletCode)).ToList().First();
                if (list != null )
                {
                    filteredList.Add(list);
                }
            }
            return childlst;
        }

        private string getstoreTypeID(string storeType)
        {
            string storeID = string.Empty;
            if (!string.IsNullOrEmpty(storeType))
            {
                switch (storeType.ToLower())
                {
                    case "arrival":
                        storeID = Constants.Arrival;
                        break;
                    case "departure":
                        storeID = Constants.Departure;
                        break;
                }
            }
            return storeID;
        }

        private string getLocationID(string location)
        {
            Airport_Location parsedLocation = (Airport_Location)Enum.Parse(typeof(Airport_Location), location);

            string LocationID = string.Empty;
            if (!string.IsNullOrEmpty(location))
            {
                switch (parsedLocation)
                {
                    case Airport_Location.AMD:
                        LocationID = Constants.Ahmedabad;
                        break;

                    case Airport_Location.GAU:
                        LocationID = Constants.Guwahati;
                        break;
                    case Airport_Location.JAI:
                        LocationID = Constants.Jaipur;
                        break;
                    case Airport_Location.LKO:
                        LocationID = Constants.Lucknow;
                        break;
                    case Airport_Location.TRV:
                        LocationID = Constants.Thiruvananthapuram;
                        break;
                    case Airport_Location.IXE:
                        LocationID = Constants.Mangaluru;
                        break;
                    case Airport_Location.BOM:
                        LocationID = Constants.Mumbai;
                        break;
                    case Airport_Location.ADLONE:
                        LocationID = Constants.adaniOne;
                        break;

                }
            }
            return LocationID;
        }


        private string getTerminalID(string terminaltype)
        {
            string TerminalID = string.Empty;
            if (!string.IsNullOrEmpty(terminaltype))
            {
                switch (terminaltype.ToLower())
                {
                    case "terminal1":
                        TerminalID = Constants.Terminal1;
                        break;
                    case "terminal2":
                        TerminalID = Constants.Terminal2;
                        break;

                }
            }
            return TerminalID;
        }
    }
}