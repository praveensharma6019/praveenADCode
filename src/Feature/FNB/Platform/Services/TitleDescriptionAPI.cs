using Adani.SuperApp.Airport.Feature.Carousel.Platform;
using Adani.SuperApp.Airport.Feature.FNB.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public class TitleDescriptionAPI : ITitleDescriptionAPI
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public TitleDescriptionAPI(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetTitleDescriptionAPIData(Sitecore.Mvc.Presentation.Rendering rendering, string storeType, string location, string terminaltype)
        {
            
            WidgetModel TitleDescription = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                TitleDescription.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                TitleDescription.widget.widgetItems = ParseTitleDescriptionApidata(rendering, storeType, location, terminaltype);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetTitleDescriptionAPIData gives -> " + ex.Message);
            }


            return TitleDescription;
        }

        private List<object> ParseTitleDescriptionApidata(Rendering rendering, string storeType, string location, string terminaltype)
        {
            List<Object> TitleDescriptionAPIData = new List<Object>();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? RenderingContext.Current.Rendering.Item
               : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Info("Datasource not selected");
                }
                else
                {
                    string Temp = TitleDescription.TitleDescriptionTemplateID.ToString();

                    List <Item> titleDescription_list = new List<Item>();
                    if (datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList() != null)
                    {
                        titleDescription_list = datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList();
                    }

                    List<Item> FilteredOfferData = GetTitleDescription(titleDescription_list, getstoreTypeID(storeType), getLocationID(location), getTerminalID(terminaltype));

                    TitleDescriptionModel titledescriptionApiModel = null;
                    foreach (Sitecore.Data.Items.Item titledescriptionItem in FilteredOfferData)
                    {
                        titledescriptionApiModel = new TitleDescriptionModel();
                        titledescriptionApiModel.Title = !string.IsNullOrEmpty(titledescriptionItem.Fields[TitleDescription.Title].Value.ToString()) ? titledescriptionItem.Fields[TitleDescription.Title].Value.ToString() : string.Empty;
                        titledescriptionApiModel.Description = !string.IsNullOrEmpty(titledescriptionItem.Fields[TitleDescription.Description].Value.ToString()) ? titledescriptionItem.Fields[TitleDescription.Description].Value.ToString() : string.Empty;

                        TitleDescriptionAPIData.Add(titledescriptionApiModel);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" ParseTitleDescriptionApidata gives -> " + ex.Message);
            }

            return TitleDescriptionAPIData;
        }


        private List<Item> GetTitleDescription(List<Sitecore.Data.Items.Item> childList, string storeType, string location, string terminaltype)
        {

            List<Item> childlst = new List<Item>();

            childlst = childList.Where(p => p[TitleDescription.LocationType].Contains(location)
                                            && p[TitleDescription.TerminalStoreType].Contains(storeType)
                                            && p[TitleDescription.TerminalType].Contains(terminaltype)).ToList();
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