using Adani.SuperApp.Airport.Feature.CabVendor.Platform;
using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Services
{
    public class CabCancellation : ICabCancellation
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;

        public CabCancellation(ILogRepository logRepository, IHelper helper)
        {
            this._logRepository = logRepository;
            this._helper = helper;
        }

        public CancellationData GetCancellationData(Rendering rendering)
        {
            CancellationData _obj = new CancellationData();
            try
            {
                var datasourceItem = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (datasourceItem != null)
                {
                    _obj.headerTitle = !string.IsNullOrEmpty(datasourceItem.Fields[VendorConstant.CancellationTitle].Value.ToString()) ? datasourceItem.Fields[VendorConstant.CancellationTitle].Value.ToString() : string.Empty;
                    _obj.headerDescription = !string.IsNullOrEmpty(datasourceItem.Fields[VendorConstant.CancellationDescription].Value.ToString()) ? datasourceItem.Fields[VendorConstant.CancellationDescription].Value.ToString() : string.Empty;
                    _obj.buttonUrl = _helper.GetLinkURL(datasourceItem, VendorConstant.CancellationCTALink.ToString());
                    _obj.buttonText = _helper.GetLinkText(datasourceItem, VendorConstant.CancellationCTALink.ToString());
                    _obj.title = !string.IsNullOrEmpty(datasourceItem.Fields[VendorConstant.ReasonsTitleFieldName].Value.ToString()) ? datasourceItem.Fields[VendorConstant.ReasonsTitleFieldName].Value.ToString() : string.Empty;
                    List<Reasons> _reasonsList = new List<Reasons>();
                    if (((Sitecore.Data.Fields.MultilistField)datasourceItem.Fields[VendorConstant.ReasonsList]).Count > 0)
                    {
                        Sitecore.Data.Fields.MultilistField reasonsList = datasourceItem.Fields[VendorConstant.ReasonsList];
                        if (reasonsList != null && reasonsList.GetItems() != null && reasonsList.GetItems().Any())
                        {
                            foreach (Sitecore.Data.Items.Item item in reasonsList.GetItems())
                            {
                                Reasons reasonItems = new Reasons();
                                reasonItems.reason = !string.IsNullOrEmpty(item.Fields[VendorConstant.Reason].Value.ToString()) ? item.Fields[VendorConstant.Reason].Value.ToString() : string.Empty;
                                reasonItems.descriptionHint = !string.IsNullOrEmpty(item.Fields[VendorConstant.HintText].Value.ToString()) ? item.Fields[VendorConstant.HintText].Value.ToString() : string.Empty;
                                reasonItems.descriptionLength = !string.IsNullOrEmpty(item.Fields[VendorConstant.DescriptionLength].Value.ToString()) ? item.Fields[VendorConstant.DescriptionLength].Value.ToString() : string.Empty;
                                _reasonsList.Add(reasonItems);
                            }
                            _obj.reasons = _reasonsList;
                        }
                    }
                }
                else return null;
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetCancellationData throws Exception -> " + ex.Message);
            }
            return _obj;
        }
    }
}