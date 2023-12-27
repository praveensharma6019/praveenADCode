using Adani.SuperApp.Airport.Feature.Carousel.Platform;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.StringExtensions;
using Sitecore.Web.UI.WebControls.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Item = Sitecore.Data.Items.Item;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
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
                fnBSapApi.widget.widgetItems = GetTermsAndCondition(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetSapAPIData gives -> " + ex.Message);
            }


            return fnBSapApi;
        }


        public List<object> GetTermsAndCondition(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            List<object> list = new List<object>();
            
            try
            {
                var datasource = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasource != null && datasource.GetChildren() != null && datasource.GetChildren().Count() > 0)
                {

                    foreach (Sitecore.Data.Items.Item termsCondition in datasource.GetChildren()) {
                        TermsAndConditionModel terms = new TermsAndConditionModel();
                        terms.Title = !string.IsNullOrEmpty(termsCondition.Fields["Title"].Value.ToString()) ? termsCondition.Fields["Title"].Value.ToString() : String.Empty;
                        terms.OfferTitle = !string.IsNullOrEmpty(termsCondition.Fields["OfferTitle"].Value.ToString()) ? termsCondition.Fields["OfferTitle"].Value.ToString() : String.Empty;
                        terms.SubTitle = !string.IsNullOrEmpty(termsCondition.Fields["SubTitle"].Value.ToString()) ? termsCondition.Fields["SubTitle"].Value.ToString() : String.Empty;
                        terms.PromoCodeTitle = !string.IsNullOrEmpty(termsCondition.Fields["PromoCodeTitle"].Value.ToString()) ? termsCondition.Fields["PromoCodeTitle"].Value.ToString() : String.Empty;
                        terms.PromoCodeValue = !string.IsNullOrEmpty(termsCondition.Fields["PromoCodeValue"].Value.ToString()) ? termsCondition.Fields["PromoCodeValue"].Value.ToString() : String.Empty;
                        terms.Description = !string.IsNullOrEmpty(termsCondition.Fields["Description"].Value.ToString()) ? termsCondition.Fields["Description"].Value.ToString() : String.Empty;
                        terms.SubDescription = !string.IsNullOrEmpty(termsCondition.Fields["SubDescription"].Value.ToString()) ? termsCondition.Fields["SubDescription"].Value.ToString() : String.Empty;
                        terms.ValidityPeriod = !string.IsNullOrEmpty(termsCondition.Fields["ValidityPeriod"].Value.ToString()) ? termsCondition.Fields["ValidityPeriod"].Value.ToString() : String.Empty;
                        terms.MinimumTransaction = !string.IsNullOrEmpty(termsCondition.Fields["MinimumTransaction"].Value.ToString()) ? termsCondition.Fields["MinimumTransaction"].Value.ToString() : String.Empty;

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


    }
}