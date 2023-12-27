using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.Retail.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.Services
{
    public class RefundPolicy : IRefundPolicy
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public RefundPolicy(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetRefundPolicyData(Rendering rendering, string outletcode)
        {

            WidgetModel retailOutletData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constants.RenderingParamField]);
                retailOutletData.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                retailOutletData.widget.widgetItems = RefundPolicyData(rendering,outletcode);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetRefundPolicyData gives -> " + ex.Message);
            }


            return retailOutletData;
        }

        private List<Object> RefundPolicyData(Rendering rendering, string outletcode)
        {
            List<Object> refundDataList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Error(" GetRefundPolicyData RefundPolicyData  data source is empty ");
                }
                var filteredData = GetFilteredPolicyDetails(datasource.GetChildren().ToList(), outletcode);
                RefundPolicyDetails RefundData;
                foreach (Sitecore.Data.Items.Item item in filteredData)
                {
                    RefundData = new RefundPolicyDetails();
                    RefundData.Title = !string.IsNullOrEmpty(item.Fields[Constants.Title].Value.ToString()) ? item.Fields[Constants.Title].Value.ToString() : string.Empty;
                    List<PolicyData> policyDataList = new List<PolicyData>();
                    foreach (Sitecore.Data.Items.Item offersItem in item.Children)
                    {
                        PolicyData policyData = new PolicyData();
                        policyData.List = !string.IsNullOrEmpty(offersItem.Fields[Constants.Title].Value.ToString()) ? offersItem.Fields[Constants.Title].Value.ToString() : string.Empty;
                        policyDataList.Add(policyData);
                    }
                    RefundData.WebDescription = policyDataList;
                    refundDataList.Add(RefundData);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetRefundPolicyData RefundPolicyData gives -> " + ex.Message);
            }

            return refundDataList;
        }
        private List<Item> GetFilteredPolicyDetails(List<Sitecore.Data.Items.Item> childList, string outletCode)
        {

            List<Item> childlst = new List<Item>();
            foreach (Sitecore.Data.Items.Item offerItem in childList)
            {
                Sitecore.Data.Fields.MultilistField multilistField = offerItem.Fields[Constants.ApplicableOutlets];
                List<Item> applicableOutlets = multilistField.GetItems().ToList();

                var list = applicableOutlets.Where(a => a.Fields[Constants.OutletCode].Value.Equals(outletCode)).ToList();
                if (list != null && list.Count != 0)
                {
                    childlst.Add(offerItem);
                }
            }

            return childlst;
        }

    }
}