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
    public class RetailOutlet : IRetailOutlet
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public RetailOutlet(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetRetailOutletData(Rendering rendering, string storeType, string location, string terminaltype, string outletcode,string isApp)
        {

            WidgetModel retailOutletData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constants.RenderingParamField]);
                retailOutletData.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                retailOutletData.widget.widgetItems = ParseOutletData(rendering, storeType,location, terminaltype,outletcode,isApp);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetRetailOutletData gives -> " + ex.Message);
            }


            return retailOutletData;
        }

        private List<object> ParseOutletData(Rendering rendering, string storeType, string location, string terminaltype,string outletcode,string isApp)
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
                    string Temp = Constants.OutletDetailsTemplateID.ToString();

                    var outlets_list = new List<Item>();
                    if (datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList() != null)
                    {
                        outlets_list = datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList();
                    }

                    BrandData _outletModel = null;
                    var FilteredOfferData = GetOutletsData(outlets_list, getstoreTypeID(storeType), getLocationID(location), getTerminalID(terminaltype), outletcode);


                    foreach (Sitecore.Data.Items.Item outletItem in FilteredOfferData)
                    {
                        _outletModel = new BrandData();
                        _outletModel.Title = !string.IsNullOrEmpty(outletItem.Fields[Constants.Title].Value.ToString()) ? outletItem.Fields[Constants.Title].Value.ToString() : string.Empty;
                        if ((Sitecore.Data.Fields.ImageField)outletItem.Fields[Constants.ImageSrc] != null)
                            _outletModel.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)outletItem.Fields[Constants.ImageSrc]);
                        _outletModel.CTALink = _helper.GetLinkURL(outletItem, Constants.CTALink);
                        _outletModel.CTALinkText = _helper.GetLinkText(outletItem, Constants.CTALink);
                        _outletModel.Description = !string.IsNullOrEmpty(outletItem.Fields[Constants.Description].Value.ToString()) ? outletItem.Fields[Constants.Description].Value.ToString() : string.Empty;
                        _outletModel.Storecode = !string.IsNullOrEmpty(outletItem.Fields[Constants.Storecode].Value.ToString()) ? outletItem.Fields[Constants.Storecode].Value.ToString() : string.Empty;
                        _outletModel.UniqueId = !string.IsNullOrEmpty(outletItem.Fields[Constants.UniqueId].Value.ToString()) ? outletItem.Fields[Constants.UniqueId].Value.ToString() : string.Empty;
                        if ((Sitecore.Data.Fields.ImageField)outletItem.Fields[Constants.ThumbnailImage] != null)
                            _outletModel.ThumbnailImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)outletItem.Fields[Constants.ThumbnailImage]);
                        if ((Sitecore.Data.Fields.ImageField)outletItem.Fields[Constants.MobileImage] != null)
                            _outletModel.MobileImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)outletItem.Fields[Constants.MobileImage]);
                        if (isApp == "true")
                        {
                            if ((Sitecore.Data.Fields.ImageField)outletItem.Fields[Constants.MobileImage] != null)
                            { _outletModel.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)outletItem.Fields[Constants.MobileImage]); }
                            else { _outletModel.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)outletItem.Fields[Constants.ImageSrc]); }
                        }

                        Sitecore.Data.Fields.MultilistField multilistField = outletItem.Fields[Constants.ApplicableOutlets];
                        List<Item> applicableOutlets = multilistField.GetItems().ToList();

                        var list = applicableOutlets.Where(a => a.Fields[Constants.OutletCode].Value.Equals(outletcode)).ToList();
                        if (list != null && list.Count!=0) {
                            outletsData.Add(_outletModel);
                        }

                        
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" ParseOutletData gives -> " + ex.Message);
            }

            return outletsData;
        }

        private List<Item> GetOutletsData(List<Sitecore.Data.Items.Item> childList, string storeType, string location, string terminaltype, string outletcode)
        {

            List<Item> childlst = new List<Item>();

            childlst = childList.Where(p => p[Constants.OutletLocationType].Contains(location)
                                            && p[Constants.OutletTerminalStoreType].Contains(storeType)
                                            && p[Constants.OutletTerminalType].Contains(terminaltype)).ToList();
                                     
            
                                             
            return childlst;
        }

        private string getstoreTypeID(string storeType)
        {
            string storeID = string.Empty;
            if (!string.IsNullOrEmpty(storeType))
            {
                switch (storeType.ToLower())
                {
                    case "arrival":
                        storeID = Constants.Arrival;
                        break;
                    case "departure":
                        storeID = Constants.Departure;
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