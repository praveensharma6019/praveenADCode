using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Controllers
{
    public class VendorSearchController : ApiController
    {
        private readonly IAPIResponse offersAPI;
        private ILogRepository logRepository;
        private readonly IHelper _helper;
        public VendorSearchController(ILogRepository _logRepository, IHelper helper)
        {
            this.logRepository = _logRepository;
            this._helper = helper;
        }

        [HttpPost]
        [Route("api/GetVendorSearchData")]
        public IHttpActionResult GetVendors([FromBody] VendorFilters filter)
        {
            ResponseFilters responseData = new ResponseFilters();
            ResultFilters resultData = new ResultFilters();

            resultData.result = GetVendorSearchDataResult(ref filter);
            ///ar jsonResult = ParseProduct(results);
            if (resultData.result != null)
            {
                responseData.status = true;
                responseData.data = resultData;
            }
            return Json(responseData);
        }



        private Object GetVendorSearchDataResult(ref VendorFilters filter)
        {

            List<Vendor> result = new List<Vendor>();
            Sitecore.Globalization.Language language = Sitecore.Globalization.Language.Parse(filter.language);
            Sitecore.Data.Database contextDB = Sitecore.Context.Database;

            Item CabVendorsFolder = contextDB.GetItem(VendorConstant.CabVendorsFolderID.ToString(), language);
            if (CabVendorsFolder.HasChildren)
            {
                string cabCode = filter.cabCode;
                var vendorItemList = CabVendorsFolder.GetChildren().Where(a => a.TemplateID.Guid == VendorConstant.VendorTemplateID || a.Fields[VendorConstant.Code].Value.ToLower().ToString() == cabCode.ToLower());

                if (vendorItemList != null && vendorItemList.Any())
                {
                    foreach (Item vendorItem in vendorItemList)
                    {
                        Vendor vendor = new Vendor();
                        vendor.title = !string.IsNullOrEmpty(vendorItem.Fields[VendorConstant.ItemTitle].Value) ? vendorItem.Fields[VendorConstant.ItemTitle].Value : string.Empty;
                        vendor.code = !string.IsNullOrEmpty(vendorItem.Fields[VendorConstant.Code].Value) ? vendorItem.Fields[VendorConstant.Code].Value : string.Empty;
                        vendor.description = !string.IsNullOrEmpty(vendorItem.Fields[VendorConstant.Description].Value) ? vendorItem.Fields[VendorConstant.Description].Value : string.Empty;
                        vendor.image = _helper.GetImageURLByFieldId(vendorItem, VendorConstant.Image);
                        vendor.mobileImage = _helper.GetImageURLByFieldId(vendorItem, VendorConstant.MobileImage);
                        Sitecore.Data.Fields.MultilistField multilistField = vendorItem.Fields[VendorConstant.Airport];
                        // Add Airports served by Vender
                        //List<Item> airportItems = new List<Item>();

                        //foreach (Item item in multilistField.GetItems())
                        //{
                        //    Airports airports = new Airports();
                        //    Item airportItem = item.Children.First(a => a.TemplateID.Guid == VenderConstant.AirportTemplateID);
                        //    airportItems.Add(airportItem);
                        //}
                        // Add Cab Services
                        multilistField = vendorItem.Fields[VendorConstant.Cabs];
                        if (multilistField != null && multilistField.GetItems() != null && multilistField.GetItems().Any())
                        {
                            foreach (Item item in multilistField.GetItems())
                            {
                                ServiceCategory serviceCategory = new ServiceCategory();
                                serviceCategory.title = !string.IsNullOrEmpty(item.Fields[VendorConstant.ItemTitle].Value) ? item.Fields[VendorConstant.ItemTitle].Value : string.Empty;
                                serviceCategory.subTitle = !string.IsNullOrEmpty(item.Fields[VendorConstant.SubTitle].Value) ? item.Fields[VendorConstant.SubTitle].Value : string.Empty;
                                serviceCategory.description = !string.IsNullOrEmpty(item.Fields[VendorConstant.Description].Value) ? item.Fields[VendorConstant.Description].Value : string.Empty;
                                serviceCategory.image = _helper.GetImageURLByFieldId(item, VendorConstant.Image);
                                serviceCategory.mobileImage = _helper.GetImageURLByFieldId(item, VendorConstant.MobileImage);
                                vendor.services.Add(serviceCategory);
                            }

                        }

                        if (vendorItem.HasChildren)
                        {
                            Item informationItem = vendorItem.Children.FirstOrDefault(a => a.TemplateID.Guid == VendorConstant.InfoTemplateID);
                            if (informationItem != null)
                            {
                                vendor.information.title = !string.IsNullOrEmpty(informationItem.Fields[VendorConstant.InfoValue].Value) ? informationItem.Fields[VendorConstant.InfoValue].Value : string.Empty;
                                if (informationItem.HasChildren)
                                {
                                    foreach (Item infoItem in informationItem.Children.Where(a => a.TemplateID.Guid == VendorConstant.InfoTemplateID))
                                    {
                                        if (!string.IsNullOrEmpty(infoItem.Fields[VendorConstant.InfoValue].Value))
                                        {
                                            vendor.information.info.Add(infoItem.Fields[VendorConstant.InfoValue].Value);
                                        }
                                    }
                                }

                            }
                        }
                        result.Add(vendor);
                    }
                }

            }

            return result;
        }
    }
}