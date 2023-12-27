using Adani.SuperApp.Airport.Feature.CabVendor.Platform;
using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Services
{
    public class CabServiceTitleDescription : ICabServiceTitleDescription
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public CabServiceTitleDescription(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetServiceDetails(Rendering rendering,string location)
        {

            WidgetModel CarouselOutletData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[VendorConstant.RenderingParamField]);
                CarouselOutletData.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                CarouselOutletData.widget.widgetItems = GetDetails(rendering,  location);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetCarouselOutletData gives -> " + ex.Message);
            }


            return CarouselOutletData;
        }
        public List<object> GetDetails(Rendering rendering,string location)
        {
            List<Object> _obj = new List<Object>();
            try
            {
                var datasourceItem = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (datasourceItem != null)
                {
                    List<Item> FilteredData = GetOutletsData(datasourceItem.GetChildren().ToList(), getLocationID(location));
                    foreach (Sitecore.Data.Items.Item item in FilteredData)
                    {
                        TitleWithDescription details = new TitleWithDescription();
                        details.Title = !string.IsNullOrEmpty(item.Fields[VendorConstant.TitleCab].Value.ToString()) ? item.Fields[VendorConstant.TitleCab].Value.ToString() : string.Empty;
                        details.Description = !string.IsNullOrEmpty(item.Fields[VendorConstant.Description].Value.ToString()) ? item.Fields[VendorConstant.Description].Value.ToString() : string.Empty;
                        details.ReadMoreText = !string.IsNullOrEmpty(item.Fields[VendorConstant.ReadMoreText].Value.ToString()) ? item.Fields[VendorConstant.ReadMoreText].Value.ToString() : string.Empty;
                        _obj.Add(details);
                    }

                  
                }
                else return null;
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetDetails throws Exception -> " + ex.Message);
            }
            return _obj;
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
                        LocationID = VendorConstant.Ahmedabad;
                        break;

                    case Airport_Location.GAU:
                        LocationID = VendorConstant.Guwahati;
                        break;
                    case Airport_Location.JAI:
                        LocationID = VendorConstant.Jaipur;
                        break;
                    case Airport_Location.LKO:
                        LocationID = VendorConstant.Lucknow;
                        break;
                    case Airport_Location.TRV:
                        LocationID = VendorConstant.Thiruvananthapuram;
                        break;
                    case Airport_Location.IXE:
                        LocationID = VendorConstant.Mangaluru;
                        break;
                    case Airport_Location.BOM:
                        LocationID = VendorConstant.Mumbai;
                        break;
                    case Airport_Location.ADLONE:
                        LocationID = VendorConstant.adaniOne;
                        break;

                }
            }
            return LocationID;
        }


        private List<Item> GetOutletsData(List<Sitecore.Data.Items.Item> childList, string location)
        {

            List<Item> childlst = new List<Item>();

            childlst = childList.Where(p => p[VendorConstant.AirportLocation].Contains(location)).ToList();

            return childlst;
        }
    }
}