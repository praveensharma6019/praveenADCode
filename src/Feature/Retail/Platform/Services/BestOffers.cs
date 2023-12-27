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
    public class BestOffers : IBestOffers
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;
        public BestOffers(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetBestOffersData(Rendering rendering, string outletcode, string isApp)
        {

            WidgetModel retailOutletData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constants.RenderingParamField]);
                retailOutletData.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                retailOutletData.widget.widgetItems = BestOffersData(rendering, outletcode,isApp);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetBestOffersData gives -> " + ex.Message);
            }


            return retailOutletData;
        }
        private List<Object> BestOffersData(Rendering rendering, string outletcode, string isApp)
        {
            List<Object> offersDataList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasource == null)
                {
                    _logRepository.Error(" GetBestOffersData BestOffersData  data source is empty ");
                }
                var filteredData = GetExclusiveOffers(datasource.GetChildren().ToList(), outletcode);
                BankOffers offers;
                foreach (Sitecore.Data.Items.Item item in filteredData)
                {
                    offers = new BankOffers();
                    offers.Title = !string.IsNullOrEmpty(item.Fields[Constants.Title].Value.ToString()) ? item.Fields[Constants.Title].Value.ToString() : string.Empty;
                    offers.CTALinkText = _helper.GetLinkText(item, Constants.CTALink);
                    offers.CTALink = _helper.GetLinkURL(item, Constants.CTALink);
                    List<Offers> offersData = new List<Offers>();
                    foreach (Sitecore.Data.Items.Item offersItem in item.Children)
                    {
                        Offers offerData = new Offers();
                        offerData.Title = !string.IsNullOrEmpty(offersItem.Fields[Constants.Title].Value.ToString()) ? offersItem.Fields[Constants.Title].Value.ToString() : string.Empty;
                        if ((Sitecore.Data.Fields.ImageField)offersItem.Fields[Constants.ImageSrc] != null)
                            offerData.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)offersItem.Fields[Constants.ImageSrc]);
                        offerData.CTALink = _helper.GetLinkURL(offersItem, Constants.CTALink);
                        offerData.CTALinkText = _helper.GetLinkText(offersItem, Constants.CTALink);
                        if ((Sitecore.Data.Fields.ImageField)offersItem.Fields[Constants.ThumbnailImage] != null)
                            offerData.ThumbnailImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)offersItem.Fields[Constants.ThumbnailImage]);
                        if ((Sitecore.Data.Fields.ImageField)offersItem.Fields[Constants.MobileImage] != null)
                            offerData.MobileImage = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)offersItem.Fields[Constants.MobileImage]);
                        if (isApp=="true")
                        {
                            if ((Sitecore.Data.Fields.ImageField)offersItem.Fields[Constants.MobileImage] != null)
                            { offerData.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)offersItem.Fields[Constants.MobileImage]); }
                            else { offerData.ImageSrc = _helper.GetImageURL((Sitecore.Data.Fields.ImageField)offersItem.Fields[Constants.ImageSrc]); }
                        }
                        offersData.Add(offerData);
                    }
                    offers.Offers = offersData;
                    offersDataList.Add(offers);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" BestOffersData BestOffersData gives -> " + ex.Message);
            }

            return offersDataList;
        }
        private List<Item> GetExclusiveOffers(List<Sitecore.Data.Items.Item> childList, string outletCode)
        {

            List<Item> childlst = new List<Item>();
            foreach (Sitecore.Data.Items.Item offerItem in childList)
            {
                Sitecore.Data.Fields.MultilistField multilistField = offerItem.Fields[Constants.ApplicableOutlets];
                List<Item> applicableOutlets = multilistField.GetItems().ToList();

                var list = applicableOutlets.Where(a => a.Fields[Constants.OutletCode].Value.Equals(outletCode)).ToList();
                if (list != null && list.Count != 0)
                {
                    childlst.Add(offerItem);
                }
            }

            return childlst;
        }
    }
}