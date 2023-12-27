using Adani.SuperApp.Airport.Feature.Retail.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Newtonsoft.Json;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Item = Sitecore.Data.Items.Item;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.Services
{
    public class RetailBrandsDetails : IRetailBrandsDetails
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public RetailBrandsDetails(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetBrandsData(Sitecore.Mvc.Presentation.Rendering rendering, string storeType, string location, string terminaltype, string isApp)
        {

            WidgetModel retailBrandsDetails = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constants.RenderingParamField]);
                retailBrandsDetails.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                retailBrandsDetails.widget.widgetItems = GetAllBrandsData(rendering, storeType, location, terminaltype, isApp);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" RetailBrandsDetailsService GetBrandsData gives -> " + ex.Message);
            }


            return retailBrandsDetails;
        }

        private List<Object> GetAllBrandsData(Rendering rendering, string storeType, string location, string terminaltype, string isApp)
        {
            List<Object> BrandsDataList = new List<Object>();
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
                    string Temp = Constants.BrandsDetailsTemplateID.ToString();

                    BrandData brandDetails = null;
                    var FilteredBrandsData = GetExclusiveBrands(datasource.GetChildren().Where(x => x.TemplateID.ToString().Equals(Temp)).ToList(), getstoreTypeID(storeType), getLocationID(location), getTerminalID(terminaltype));

                    foreach(Sitecore.Data.Items.Item BrandItem in FilteredBrandsData)
                    {
                        brandDetails = new BrandData();
                        brandDetails.Title= !string.IsNullOrEmpty(BrandItem.Fields[Constants.Title].Value.ToString()) ? BrandItem.Fields[Constants.Title].Value.ToString() : string.Empty;
                        if((Sitecore.Data.Fields.ImageField)BrandItem.Fields[Constants.ImageSrc] != null)
                            brandDetails.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)BrandItem.Fields[Constants.ImageSrc]);
                        brandDetails.CTALink = _helper.GetLinkURL(BrandItem,Constants.CTALink);
                        brandDetails.CTALinkText = _helper.GetLinkText(BrandItem, Constants.CTALink);
                        brandDetails.Description = !string.IsNullOrEmpty(BrandItem.Fields[Constants.Description].Value.ToString()) ? BrandItem.Fields[Constants.Description].Value.ToString() : string.Empty;
                        brandDetails.Storecode= !string.IsNullOrEmpty(BrandItem.Fields[Constants.Storecode].Value.ToString()) ? BrandItem.Fields[Constants.Storecode].Value.ToString() : string.Empty;
                        brandDetails.UniqueId = !string.IsNullOrEmpty(BrandItem.Fields[Constants.UniqueId].Value.ToString()) ? BrandItem.Fields[Constants.UniqueId].Value.ToString() : string.Empty;
                        if ((Sitecore.Data.Fields.ImageField)BrandItem.Fields[Constants.ThumbnailImage] != null)
                            brandDetails.ThumbnailImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)BrandItem.Fields[Constants.ThumbnailImage]);
                        if ((Sitecore.Data.Fields.ImageField)BrandItem.Fields[Constants.MobileImage] != null)
                            brandDetails.MobileImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)BrandItem.Fields[Constants.MobileImage]);

                        if (isApp == "true")
                        {
                            if ((Sitecore.Data.Fields.ImageField)BrandItem.Fields[Constants.MobileImage] != null)
                            { brandDetails.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)BrandItem.Fields[Constants.MobileImage]); }
                            else { brandDetails.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)BrandItem.Fields[Constants.ImageSrc]); }
                        }
                        BrandsDataList.Add(brandDetails);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetAllBrandsData gives -> " + ex.Message);

            }
            return BrandsDataList;
        }

        private List<Item> GetExclusiveBrands(List<Sitecore.Data.Items.Item> childList, string storeType, string location, string terminaltype)
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