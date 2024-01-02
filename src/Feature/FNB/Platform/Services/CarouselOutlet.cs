using Adani.SuperApp.Airport.Feature.Carousel.Platform;
using Adani.SuperApp.Airport.Feature.FNB.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Extensions;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public class CarouselOutlet : ICarouselOutlet
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public CarouselOutlet(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetCarouselOutletData(Rendering rendering, string storeType, string location, string terminaltype,string isApp)
        {

            WidgetModel CarouselOutletData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                CarouselOutletData.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                CarouselOutletData.widget.widgetItems = ParseOutletData(rendering, storeType,location, terminaltype,isApp);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetFNBOutletData gives -> " + ex.Message);
            }


            return CarouselOutletData;
        }

        private List<object> ParseOutletData(Rendering rendering, string storeType, string location, string terminaltype,string isApp)
        {
            List<Object> outletsData = new List<Object>();
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
                    string Temp = Carousels.CarouselDetailsTemplateID.ToString();

                    List<Item> carousels_list = new List<Item>();
                    if (datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList() != null)
                    {
                        carousels_list = datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList();
                    }

                    CarouselModel _carouselModel = null;
                    List<Item> FilteredOfferData = GetOutletsData(carousels_list, getstoreTypeID(storeType), getLocationID(location), getTerminalID(terminaltype));


                    foreach (Sitecore.Data.Items.Item outletItem in FilteredOfferData)
                    {
                        _carouselModel = new Models.CarouselModel();
                        _carouselModel.Title = !string.IsNullOrEmpty(outletItem.Fields[Carousels.Title].Value.ToString()) ? outletItem.Fields[Carousels.Title].Value.ToString() : string.Empty;
                        _carouselModel.BannerCode = !string.IsNullOrEmpty(outletItem.Fields[Carousels.BannerCode].Value.ToString()) ? outletItem.Fields[Carousels.BannerCode].Value.ToString() : string.Empty;
                        _carouselModel.SubTitle = !string.IsNullOrEmpty(outletItem.Fields[Carousels.SubTitle].Value.ToString()) ? outletItem.Fields[Carousels.SubTitle].Value.ToString() : string.Empty;
                        _carouselModel.BannerCondition= !string.IsNullOrEmpty(outletItem.Fields[Carousels.BannerCondition].Value.ToString()) ? outletItem.Fields[Carousels.BannerCondition].Value.ToString() : string.Empty;
                        _carouselModel.OfferUniqueID= !string.IsNullOrEmpty(outletItem.Fields[Carousels.offerUniqueID].Value.ToString()) ? outletItem.Fields[Carousels.offerUniqueID].Value.ToString() : string.Empty;
                        if ((Sitecore.Data.Fields.ImageField)outletItem.Fields[Carousels.ImageSrc] != null)
                            _carouselModel.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)outletItem.Fields[Carousels.ImageSrc]);
                        _carouselModel.CTALink = _helper.GetLinkURL(outletItem, Carousels.CTALink);
                        _carouselModel.CTALinkText = _helper.GetLinkText(outletItem, Carousels.CTALink);
                        _carouselModel.Description = !string.IsNullOrEmpty(outletItem.Fields[Carousels.Description].Value.ToString()) ? outletItem.Fields[Carousels.Description].Value.ToString() : string.Empty;
                        _carouselModel.UniqueId = !string.IsNullOrEmpty(outletItem.Fields[Carousels.UniqueID].Value.ToString()) ? outletItem.Fields[Carousels.UniqueID].Value.ToString() : string.Empty;
                        if ((Sitecore.Data.Fields.ImageField)outletItem.Fields[Carousels.ThumbnailImage] != null)
                            _carouselModel.ThumbnailImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)outletItem.Fields[Carousels.ThumbnailImage]);
                        if ((Sitecore.Data.Fields.ImageField)outletItem.Fields[Carousels.MobileImage] != null)
                            _carouselModel.MobileImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)outletItem.Fields[Carousels.MobileImage]);
                        Sitecore.Data.Fields.MultilistField multilistField = outletItem.Fields[Bankoffers.ApplicableOutlets];

                        if(isApp == "true")
                        {
                            if ((Sitecore.Data.Fields.ImageField)outletItem.Fields[Carousels.MobileImage] != null)
                            { _carouselModel.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)outletItem.Fields[Carousels.MobileImage]); }
                            else { _carouselModel.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)outletItem.Fields[Carousels.ImageSrc]); }
                        }

                        List<Item> applicableOutlets = multilistField.GetItems().ToList();

                        List<string> outletIDs = new List<string>();
                        List<string> restaurantName = new List<string>();
                        foreach (Sitecore.Data.Items.Item item in applicableOutlets)
                        {
                            string outletID = item.Fields[Bankoffers.OutletCode].Value;
                            outletIDs.Add(outletID);
                            string name = item.Fields[Carousels.Title].Value;
                            restaurantName.Add(name);

                        }
                        var JoinedID = string.Join(",", outletIDs);
                        var JoinedNames = string.Join(",", restaurantName);
                        //if (outletIDs.Count > 1)
                        //{
                        //    _carouselModel.UniqueId = "101";
                        //}
                        //else {
                        //    if (outletIDs.Count ==1)
                        //    _carouselModel.UniqueId = "100";
                        //}
                        _carouselModel.RestaurantName=JoinedNames;
                        _carouselModel.OutletID = JoinedID;
                        _carouselModel.Storecode = JoinedID;
                        outletsData.Add(_carouselModel);

                    }
                }
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