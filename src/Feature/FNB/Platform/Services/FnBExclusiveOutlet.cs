using Adani.SuperApp.Airport.Feature.Carousel.Platform;
using Adani.SuperApp.Airport.Feature.FNB.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using Item = Sitecore.Data.Items.Item;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public class FnBExclusiveOutlet : IFnBExclusiveOutlet
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public FnBExclusiveOutlet(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetExclusiveOutletdata(Sitecore.Mvc.Presentation.Rendering rendering, string storeType, string location, string terminaltype, string isApp)
        {

            //FnBExclusiveOutlet fnBExclusiveOutlet = new FnBExclusiveOutlet();
            WidgetModel fnBExclusiveOutlet = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                fnBExclusiveOutlet.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                fnBExclusiveOutlet.widget.widgetItems = GetOutletdata(rendering, storeType, location, terminaltype, isApp);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" SliderWithImageAndDetailService GetSliderWithImageAndDetail gives -> " + ex.Message);
            }


            return fnBExclusiveOutlet;
        }

        private List<Object> GetOutletdata(Rendering rendering, string storeType, string location, string terminaltype, string isApp)
        {
            List<Object> OutletDataList = new List<Object>();
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
                    string Temp = FnbExclusiveOutlet.ExclusiveOutletTemplateID.ToString();

                    var exclusive_list = new List<Item>();
                    if (datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList() != null)
                    {
                        exclusive_list = datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList();
                    }



                    ExclusiveOutlets exclusiveOutlets = null;
                    var FilteredOfferData = GetExclusiveOutlets(exclusive_list, getstoreTypeID(storeType), getLocationID(location), getTerminalID(terminaltype));

                    foreach (Sitecore.Data.Items.Item ExclusiveItem in FilteredOfferData)
                    {
                        exclusiveOutlets = new ExclusiveOutlets();
                        exclusiveOutlets.Title = !string.IsNullOrEmpty(ExclusiveItem.Fields[FnbExclusiveOutlet.Title].Value.ToString()) ? ExclusiveItem.Fields[FnbExclusiveOutlet.Title].Value.ToString() : string.Empty;
                        if ((Sitecore.Data.Fields.ImageField)ExclusiveItem.Fields[FnbExclusiveOutlet.ImageSrc] != null)
                            exclusiveOutlets.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)ExclusiveItem.Fields[FnbExclusiveOutlet.ImageSrc]);
                        exclusiveOutlets.Storecode = !string.IsNullOrEmpty(ExclusiveItem.Fields[FnbExclusiveOutlet.StoreCode].Value.ToString()) ? ExclusiveItem.Fields[FnbExclusiveOutlet.StoreCode].Value.ToString() : string.Empty;
                        if ((Sitecore.Data.Fields.ImageField)ExclusiveItem.Fields[FnbExclusiveOutlet.ThumbnailImage] != null)
                            exclusiveOutlets.ThumbnailImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)ExclusiveItem.Fields[FnbExclusiveOutlet.ThumbnailImage]);
                        exclusiveOutlets.Openingtime = !string.IsNullOrEmpty(ExclusiveItem.Fields[FnbExclusiveOutlet.OpeningTime].Value.ToString()) ? ExclusiveItem.Fields[FnbExclusiveOutlet.OpeningTime].Value.ToString() : string.Empty;
                        exclusiveOutlets.Closingtime = !string.IsNullOrEmpty(ExclusiveItem.Fields[FnbExclusiveOutlet.ClosingTime].Value.ToString()) ? ExclusiveItem.Fields[FnbExclusiveOutlet.ClosingTime].Value.ToString() : string.Empty;
                        exclusiveOutlets.PreparationTime = !string.IsNullOrEmpty(ExclusiveItem.Fields[FnbExclusiveOutlet.PreparationTime].Value.ToString()) ? ExclusiveItem.Fields[FnbExclusiveOutlet.PreparationTime].Value.ToString() : string.Empty;
                        exclusiveOutlets.StoreStatus = !string.IsNullOrEmpty(ExclusiveItem.Fields[FnbExclusiveOutlet.StoreStatus].Value.ToString()) ? ExclusiveItem.Fields[FnbExclusiveOutlet.StoreStatus].Value.ToString() : string.Empty;
                        
                        if ((Sitecore.Data.Fields.ImageField)ExclusiveItem.Fields[FnbExclusiveOutlet.MobileImage] != null)
                            exclusiveOutlets.MobileImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)ExclusiveItem.Fields[FnbExclusiveOutlet.MobileImage]);
                        if (isApp == "true")
                        {
                            if ((Sitecore.Data.Fields.ImageField)ExclusiveItem.Fields[FnbExclusiveOutlet.MobileImage] != null)
                            { exclusiveOutlets.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)ExclusiveItem.Fields[FnbExclusiveOutlet.MobileImage]); }
                            else { exclusiveOutlets.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)ExclusiveItem.Fields[FnbExclusiveOutlet.ImageSrc]); }
                        }
                        OutletDataList.Add(exclusiveOutlets);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetOutletdata gives -> " + ex.Message);

            }
            return OutletDataList;
        }

        private List<Item> GetExclusiveOutlets(List<Sitecore.Data.Items.Item> childList, string storeType, string location, string terminaltype)
        {

            List<Item> childlst = new List<Item>();

            childlst = childList.Where(p => p[Constants.LocationType].Contains(location)
                                            && p[Constants.TerminalStoreType].Contains(storeType)
                                            && p[Constants.TerminalType].Contains(terminaltype)).ToList();
            return childlst;
        }

        private string getstoreTypeID(string storeType)
        {
            string storeID = string.Empty;
            if (!string.IsNullOrEmpty(storeType))
            {
                switch (storeType.ToLower())
                {
                    case "arrival-dom":
                        storeID = Constants.ArrivalDomestic;
                        break;
                    case "arrival-int":
                        storeID = Constants.ArrivalInternational;
                        break;
                    case "departure-dom":
                        storeID = Constants.DepartureDomestic;
                        break;
                    case "departure-int":
                        storeID = Constants.DepartureInternational;
                        break;
                }
            }
            return storeID;
        }

        private string getLocationID(string location)
        {
          Airport_Location parsedLocation=  (Airport_Location)Enum.Parse(typeof(Airport_Location), location);
           
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
 