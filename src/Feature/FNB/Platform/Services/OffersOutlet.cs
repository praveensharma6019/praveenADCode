using Adani.SuperApp.Airport.Feature.Carousel.Platform;
using Adani.SuperApp.Airport.Feature.FNB.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Syndication;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public class OffersOutlet : IOffersOutlet
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public OffersOutlet(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetOffersOutletData(Rendering rendering, string storeType, string location, string terminaltype)
        {

            WidgetModel OffersOutletData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                OffersOutletData.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                OffersOutletData.widget.widgetItems = ParseOutletData(storeType,location, terminaltype);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetOffersOutletData gives -> " + ex.Message);
            }


            return OffersOutletData;
        }

        private List<object> ParseOutletData(string storeType, string location, string terminaltype)
        {
            List<Object> outletsData = new List<Object>();
            try
            {
              
                    List<Item> allItems = new List<Item>();

                   var carouselItem= Sitecore.Context.Database.GetItem(Carousels.HeroCarouselFolderID);
                    if (carouselItem!=null && carouselItem.GetChildren() != null) {
                       allItems.AddRange(carouselItem.GetChildren());
                    }

                    var allOffers = Sitecore.Context.Database.GetItem(Offers.AlloffersFolderID);
                    if (allOffers!=null && allOffers.GetChildren() != null)
                    {
                        allItems.AddRange(allOffers.GetChildren());
                    }
                    
                    OffersModel _offersModel = null;
                    _offersModel = new OffersModel();
                    List<Item> FilteredOfferData = GetOutletsData(allItems, getstoreTypeID(storeType), getLocationID(location), getTerminalID(terminaltype));

                    List<string> outletIDs = new List<string>();
                    foreach (Sitecore.Data.Items.Item outletItem in FilteredOfferData)
                    {
                        Sitecore.Data.Fields.MultilistField multilistField = outletItem.Fields[Bankoffers.ApplicableOutlets];
                        List<Item> applicableOutlets = multilistField.GetItems().ToList();
 
                        foreach (Sitecore.Data.Items.Item item in applicableOutlets)
                        {
                            string outletID = item.Fields[Bankoffers.OutletCode].Value;
                            outletIDs.Add(outletID);

                        }

                    }
                        _offersModel = new OffersModel();
                        List<string> distinctIDs = outletIDs.Distinct().ToList();
                        var JoinedID = string.Join(",", distinctIDs);
                        _offersModel.outletID = JoinedID;
                    
                    outletsData.Add(_offersModel);
                }
            
            catch (Exception ex)
            {

                _logRepository.Error(" ParseOutletData gives -> " + ex.Message);
            }

            return outletsData;
        }

        private List<Item> GetOutletsData(List<Sitecore.Data.Items.Item> childList, string storeType, string location, string terminaltype)
        {

            List<Item> childlst = new List<Item>();

            childlst = childList.Where(p => p[Carousels.LocationType].Contains(location)
                                            && p[Carousels.TerminalStoreType].Contains(storeType)
                                            && p[Carousels.TerminalType].Contains(terminaltype)).ToList();
                                                                      
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