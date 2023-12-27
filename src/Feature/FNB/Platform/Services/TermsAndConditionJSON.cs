using Adani.SuperApp.Airport.Feature.Carousel.Platform;
using Adani.SuperApp.Airport.Feature.FNB.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Collections;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using Item = Sitecore.Data.Items.Item;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public class TermsAndConditionJSON : ITermsAndConditionJSON
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public TermsAndConditionJSON(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetTermsAndConditionData(Sitecore.Mvc.Presentation.Rendering rendering, string queryString, string storeType)
        {

            WidgetModel fnBSapApi = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                fnBSapApi.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                fnBSapApi.widget.widgetItems = GetTermsAndCondition(rendering,storeType);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetTermsAndConditionData gives -> " + ex.Message);
            }


            return fnBSapApi;
        }


        public List<object> GetTermsAndCondition(Sitecore.Mvc.Presentation.Rendering rendering,string storeType)
        {
            List<object> list = new List<object>();
            
            try
            {
                var datasource = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasource != null && datasource.GetChildren() != null && datasource.GetChildren().Count() > 0)
                {
                    List<Item> filteredData = new List<Item>();
                    if (String.IsNullOrEmpty(storeType)) {
                        filteredData =datasource.GetChildren().ToList();

                    }
                    else
                    {
                        filteredData = GetFilteredTermsAndCondition(datasource.GetChildren(), storeType);

                    }


                    foreach (Sitecore.Data.Items.Item termsCondition in filteredData) {
                        TermsAndConditionModel terms = new TermsAndConditionModel();
                        terms.Title = !string.IsNullOrEmpty(termsCondition.Fields["Title"].Value.ToString()) ? termsCondition.Fields["Title"].Value.ToString() : String.Empty;
                        terms.OfferTitle = !string.IsNullOrEmpty(termsCondition.Fields["OfferTitle"].Value.ToString()) ? termsCondition.Fields["OfferTitle"].Value.ToString() : String.Empty;
                        terms.LicenseCode= !string.IsNullOrEmpty(termsCondition.Fields["LicenseCode"].Value.ToString()) ? termsCondition.Fields["LicenseCode"].Value.ToString() : String.Empty;
                        terms.LicenseText= !string.IsNullOrEmpty(termsCondition.Fields["LicenseText"].Value.ToString()) ? termsCondition.Fields["LicenseText"].Value.ToString() : String.Empty;
                        terms.Image = termsCondition.Fields[Constant.Image] != null ? _helper.GetImageURL(termsCondition, Constant.Image) : String.Empty;
                        List<TermsAndConditionItems> items = new List<TermsAndConditionItems>();
                        if (termsCondition != null && termsCondition.HasChildren) {
                            foreach (Sitecore.Data.Items.Item item in termsCondition.GetChildren())
                            {
                                TermsAndConditionItems subItem = new TermsAndConditionItems();
                                subItem.list = item.Fields["Description"].ToString();
                                items.Add(subItem);
                            }
                        }  
                       
                        terms.WebDescription = items;

                        list.Add(terms);
                    }
                    

                }

                 
            }
            catch (Exception ex)
            {
                _logRepository.Error(" Get Terms and Condition gives -> " + ex.Message);
            }
            return list;
        }

        private List<Item> GetFilteredTermsAndCondition(ChildList childList, string storeType)
        {

            List<Item> childlst = new List<Item>();
            foreach (Sitecore.Data.Items.Item item in childList) {
                var itemID = item.Fields["StoreOutlet"].Value;
                Item targetItem = Sitecore.Context.Database.GetItem(itemID);
                if (targetItem != null && targetItem.Fields[Bankoffers.OutletCode].Value.Equals(storeType))
                {
                    childlst.Add(item);
                }

            }
            
            return childlst;
        }


    }
}