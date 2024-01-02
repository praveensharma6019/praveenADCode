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
    public class FnBbankoffers : IFnBbankoffers
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public FnBbankoffers(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetFnBbankoffersData(Rendering rendering, string storeType, string location, string terminaltype, string outletcode)
        {

            WidgetModel fnBbankoffers = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                fnBbankoffers.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                fnBbankoffers.widget.widgetItems = ParseBankoffersData(rendering, storeType,location, terminaltype,outletcode);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetFnBbankoffersData gives -> " + ex.Message);
            }


            return fnBbankoffers;
        }

        private List<object> ParseBankoffersData(Rendering rendering, string storeType, string location, string terminaltype,string outletcode)
        {
            List<Object> FnBbankoffersData = new List<Object>();
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
                    string Temp = Bankoffers.FNBBankOfferTemplateID.ToString();

                    var BankOffers_list = new List<Item>();
                    if (datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList() != null)
                    {
                        BankOffers_list = datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList();
                    }

                    List<Item> FilteredOfferData = new List<Item>();

                    FNBbankoffersModel _FNBbankoffersModel = null;
                    if (string.IsNullOrEmpty(storeType) && string.IsNullOrEmpty(location) && string.IsNullOrEmpty(terminaltype) && string.IsNullOrEmpty(outletcode))
                    {
                        FilteredOfferData = BankOffers_list;
                    }
                    else 
                    {
                        FilteredOfferData = GetFNBbankoffers(BankOffers_list, getstoreTypeID(storeType), getLocationID(location), getTerminalID(terminaltype), outletcode);

                    }


                    foreach (Sitecore.Data.Items.Item bankoffersItem in FilteredOfferData)
                    {
                        _FNBbankoffersModel = new FNBbankoffersModel();
                        _FNBbankoffersModel.Title = !string.IsNullOrEmpty(bankoffersItem.Fields[Bankoffers.Title].Value.ToString()) ? bankoffersItem.Fields[Bankoffers.Title].Value.ToString() : string.Empty;
                        if ((Sitecore.Data.Fields.ImageField)bankoffersItem.Fields[Bankoffers.Icon] != null)
                            _FNBbankoffersModel.Icon = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)bankoffersItem.Fields[Bankoffers.Icon]);
                        _FNBbankoffersModel.Code = !string.IsNullOrEmpty(bankoffersItem.Fields[Bankoffers.Code].Value.ToString()) ? bankoffersItem.Fields[Bankoffers.Code].Value.ToString() : string.Empty;
                        _FNBbankoffersModel.DisplayID = !string.IsNullOrEmpty(bankoffersItem.Fields[Bankoffers.DisplayID].Value.ToString()) ? bankoffersItem.Fields[Bankoffers.DisplayID].Value.ToString() : string.Empty;
                        _FNBbankoffersModel.Information = !string.IsNullOrEmpty(bankoffersItem.Fields[Bankoffers.Information].Value.ToString()) ? bankoffersItem.Fields[Bankoffers.Information].Value.ToString() : string.Empty;
                        _FNBbankoffersModel.CTAlink = bankoffersItem.Fields[Bankoffers.CTALink] != null ? _helper.GetLinkURL(bankoffersItem, Bankoffers.CTALink) : String.Empty;
                        _FNBbankoffersModel.ExpiryDate = bankoffersItem.Fields[Bankoffers.ExpiryDate] != null ? bankoffersItem.Fields[Bankoffers.ExpiryDate].Value : string.Empty;
                        _FNBbankoffersModel.ErrorMessage = bankoffersItem.Fields[Bankoffers.ErrorMessage] != null ? bankoffersItem.Fields[Bankoffers.ErrorMessage].Value : string.Empty;
                        _FNBbankoffersModel.IsApply = bankoffersItem.Fields[Bankoffers.IsApply] != null ? _helper.GetCheckboxOption(bankoffersItem.Fields[Bankoffers.IsApply]) : false;
                        _FNBbankoffersModel.PotentialEarnMessage= bankoffersItem.Fields[Bankoffers.PotentialEarnMessage] !=null ? bankoffersItem.Fields[Bankoffers.PotentialEarnMessage].Value : string.Empty;

                        
                        List<TermsAndConditionItem> items = new List<TermsAndConditionItem>();
                        if (bankoffersItem != null && bankoffersItem.HasChildren)
                        {
                            foreach (Sitecore.Data.Items.Item item in bankoffersItem.GetChildren())
                            {
                                TermsAndConditionItem subItem = new TermsAndConditionItem();
                                subItem.Text = item.Fields["Description"].ToString();
                                items.Add(subItem);
                            }
                        }
                        _FNBbankoffersModel.TermsNConditions = items;
                        if (outletcode==null || outletcode ==String.Empty)
                        {
                            FnBbankoffersData.Add(_FNBbankoffersModel);
                        }
                        else {
                            Sitecore.Data.Fields.MultilistField multilistField = bankoffersItem.Fields[Bankoffers.ApplicableOutlets];
                            List<Item> applicableOutlets = multilistField.GetItems().ToList();
                            var list = applicableOutlets.Where(a => a.Fields[Bankoffers.OutletCode].Value.Equals(outletcode)).ToList();
                            if (list != null && list.Count != 0)
                            {
                                FnBbankoffersData.Add(_FNBbankoffersModel);
                            }
                        }
                       

                        
                        
                       
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" ParseBankoffersData gives -> " + ex.Message);
            }

            return FnBbankoffersData;
        }

        private List<Item> GetFNBbankoffers(List<Sitecore.Data.Items.Item> childList, string storeType, string location, string terminaltype, string outletcode)
        {

            List<Item> childlst = new List<Item>();

            childlst = childList.Where(p => p[Bankoffers.LocationType].Contains(location)
                                            && p[Bankoffers.TerminalStoreType].Contains(storeType)
                                            && p[Bankoffers.TerminalType].Contains(terminaltype)).ToList();
                                     
            
                                             
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