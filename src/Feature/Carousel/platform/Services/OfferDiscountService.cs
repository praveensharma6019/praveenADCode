using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Sitecore.Data.Fields;
using Sitecore.Collections;
using System.Globalization;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class OfferDiscountService : IOfferDiscount
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public OfferDiscountService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetOfferList(Rendering rendering, bool flag, string location, string storeType, string isExclusive, string moduleType, string isOfferAndDiscount, string appType)
        {
            WidgetModel OfferListData = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                if (widget != null)
                {

                    OfferListData.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    OfferListData.widget = new WidgetItem();
                }

                OfferListData.widget.widgetItems = GetOfferListData(rendering, flag, location, storeType, isExclusive, moduleType, isOfferAndDiscount, appType);

            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetOfferList gives -> " + ex.Message);
            }


            return OfferListData;
        }


        private List<Object> GetOfferListData(Rendering rendering, bool flag, string location, string storeType, string isExclusive, string moduleType, string isOfferAndDiscount, string appType)
        {
            List<Object> OfferDiscountDataList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasourceItem = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasourceItem == null)
                {
                    _logRepository.Error(" OfferDiscountService GetOfferListData data source is empty");
                }
                OfferDiscountModel offerDiscountModelList = null;
              
                var FilteredOfferData = GetAppspecificOffers(datasourceItem.Axes.GetDescendants().Where(x => x.TemplateID.ToString().Equals(Constant.OfferTemplateID)).ToList(), appType, getLocationID(location), getstoreTypeID(storeType), isExclusive, moduleType, isOfferAndDiscount, flag);
                foreach (Sitecore.Data.Items.Item offerItem in FilteredOfferData.OrderBy(x=> Convert.ToInt32(x.Fields[Constant.BannerDisplayRank].Value)))
                {
                    if (ValidateOfferExpiry(offerItem.Fields[Constant.EffectiveTo], offerItem.Fields[Constant.EffectiveFrom]))
                    {
                            #region Parsing Code
                            offerDiscountModelList = new OfferDiscountModel();
                            offerDiscountModelList.isAirportSelectNeeded = _helper.GetCheckboxOption(offerItem.Fields[Constant.isAirportSelectNeeded]);
                            offerDiscountModelList.Title = !string.IsNullOrEmpty(offerItem.Fields[Constant.Title].Value) ? offerItem.Fields[Constant.Title].Value : string.Empty;
                            offerDiscountModelList.PromotionType = !string.IsNullOrEmpty(offerItem.Fields[Constant.PromotionType].Value) ? offerItem.Fields[Constant.PromotionType].Value : string.Empty;
                            offerDiscountModelList.PromotionCode = !string.IsNullOrEmpty(offerItem.Fields[Constant.PromotionCode].Value) ? offerItem.Fields[Constant.PromotionCode].Value : string.Empty;
                            offerDiscountModelList.PromotionDescription = !string.IsNullOrEmpty(offerItem.Fields[Constant.PromotionDescription].Value) ? offerItem.Fields[Constant.PromotionDescription].Value : string.Empty;
                            offerDiscountModelList.OfferType = !string.IsNullOrEmpty(offerItem.Fields[Constant.OfferType].Value) ? offerItem.Fields[Constant.OfferType].Value : string.Empty;
                            offerDiscountModelList.DisplayText = !string.IsNullOrEmpty(offerItem.Fields[Constant.DisplayText].Value) ? offerItem.Fields[Constant.DisplayText].Value : string.Empty;
                            Sitecore.Data.Fields.DateField EffFrom = offerItem.Fields[Constant.EffectiveFrom];
                            offerDiscountModelList.EffectiveFrom = EffFrom != null ? EffFrom.DateTime.ToString() : string.Empty;
                            Sitecore.Data.Fields.DateField EffTo = offerItem.Fields[Constant.EffectiveTo];
                            offerDiscountModelList.EffectiveTo = EffTo != null ? EffTo.DateTime.ToString() : string.Empty;
                            offerDiscountModelList.TerminalLocationType = GetLocationCode((Sitecore.Data.Fields.MultilistField)offerItem.Fields[Constant.TerminalLocationType], Constant.TerminalLocationType);
                            offerDiscountModelList.TerminalStoreType = GetLocationCode((Sitecore.Data.Fields.MultilistField)offerItem.Fields[Constant.TerminalStoreType], Constant.TerminalLocationType);
                            offerDiscountModelList.AutoId = !string.IsNullOrEmpty(offerItem.Fields[Constant.AppID].Value) ? offerItem.Fields[Constant.AppID].Value : string.Empty;
                            offerDiscountModelList.OfferTitle = !string.IsNullOrEmpty(offerItem.Fields[Constant.OfferTitle].Value) ? offerItem.Fields[Constant.OfferTitle].Value : string.Empty;
                            //Removed for Ticket NO 18278 
                            //offerDiscountModelList.PromotionClaimType = !string.IsNullOrEmpty(offerItem.Fields[Constant.PromotionClaimType].Value) ? offerItem.Fields[Constant.PromotionClaimType].Value : string.Empty;
                            offerDiscountModelList.DesktopImageSrc = _helper.GetImageURL(offerItem, Constant.DesktopImage);
                            offerDiscountModelList.DesktopImageAlt = _helper.GetImageAlt(offerItem, Constant.DesktopImage);
                            offerDiscountModelList.MobileImageSrc = _helper.GetImageURL(offerItem, Constant.OfferMobileImage);
                            offerDiscountModelList.MobileImageAlt = _helper.GetImageAlt(offerItem, Constant.OfferMobileImage);
                            offerDiscountModelList.ThumbnailImageSrc = _helper.GetImageURL(offerItem, Constant.OfferThumbnailImage);
                            offerDiscountModelList.ThumbnailImageAlt = _helper.GetImageAlt(offerItem, Constant.OfferThumbnailImage);
                            offerDiscountModelList.DeepLinkUrl = _helper.GetLinkURL(offerItem, Constant.DeepLink);
                            offerDiscountModelList.DeepLinkAlt = _helper.GetLinkText(offerItem, Constant.DeepLink);
                            var TC = !string.IsNullOrEmpty(offerItem.Fields[Constant.Termandcondition].Value) ? offerItem.Fields[Constant.Termandcondition].Value : string.Empty;
                            offerDiscountModelList.TermCondition = gettermsconditionlistofstring(TC);
                            offerDiscountModelList.savings = !string.IsNullOrEmpty(offerItem.Fields[Constant.Savings].Value) ? offerItem.Fields[Constant.Savings].Value : string.Empty;
                            offerDiscountModelList.TCLinkUrl = _helper.GetLinkURL(offerItem, Constant.TCLink);
                            offerDiscountModelList.TCLinkAlt = _helper.GetLinkText(offerItem, Constant.TCLink);
                            offerDiscountModelList.BuyQuantity = !string.IsNullOrEmpty(offerItem.Fields[Constant.BuyQuantity].Value) ? offerItem.Fields[Constant.BuyQuantity].Value : string.Empty;
                            Sitecore.Data.Fields.CheckboxField SOHP = offerItem.Fields[Constant.ShowonHomepage];
                            offerDiscountModelList.ShowonHomepage = SOHP.Checked;
                            offerDiscountModelList.ItemPath = offerItem.Paths.FullPath;
                            offerDiscountModelList.ExtraImageSrc = _helper.GetImageURL(offerItem, Constant.OfferImage);
                            offerDiscountModelList.ExtraImageAlt = _helper.GetImageAlt(offerItem, Constant.OfferImage);
                            offerDiscountModelList.DisplayRank = !string.IsNullOrEmpty(offerItem.Fields[Constant.DisplayRank].Value) ? offerItem.Fields[Constant.DisplayRank].Value : string.Empty;
                            offerDiscountModelList.Apptype = !string.IsNullOrEmpty(offerItem.Fields[Constant.AppType].Value) ? offerItem.Fields[Constant.AppType].Value : string.Empty;
                            offerDiscountModelList.SkuCode = !string.IsNullOrEmpty(offerItem.Fields[Constant.OfferSkuCode].Value) ? offerItem.Fields[Constant.OfferSkuCode].Value : string.Empty;
                            offerDiscountModelList.BannerImageDeskSrc = _helper.GetImageURL(offerItem, Constant.BannerImageDesk);
                            offerDiscountModelList.BannerImageDeskAlt = _helper.GetImageAlt(offerItem, Constant.BannerImageDesk);
                            offerDiscountModelList.BannerImageMobSrc = _helper.GetImageURL(offerItem, Constant.BannerImageMob);
                            offerDiscountModelList.BannerImageMobAlt = _helper.GetImageAlt(offerItem, Constant.BannerImageMob);
                            offerDiscountModelList.CategoryFilter = !string.IsNullOrEmpty(offerItem.Fields[Constant.CategoryFilter].Value) ? offerItem.Fields[Constant.CategoryFilter].Value : string.Empty;
                            offerDiscountModelList.offerUniqueID = !string.IsNullOrEmpty(offerItem.Fields[Constant.OfferUinqueID].Value) ? offerItem.Fields[Constant.OfferUinqueID].Value : string.Empty;
                            offerDiscountModelList.bannerCondition = getDictionaryKeyValue(Constant.TermsandconditionfieldID);
                            offerDiscountModelList.ctaText = !string.IsNullOrEmpty(offerItem.Fields[Constant.OfferSpecificCTAText].Value) ? offerItem.Fields[Constant.OfferSpecificCTAText].Value : string.Empty;
                            //Removed for Ticket NO 18278 
                            //getDictionaryKeyValue(Constant.CtaText);
                            offerDiscountModelList.SitecoreOfferID = offerItem.ID.ToString();
                            Sitecore.Data.Fields.CheckboxField TCEnable = offerItem.Fields[Constant.TCEnabled];
                            Sitecore.Data.Fields.CheckboxField isbankEnable = offerItem.Fields[Constant.isBankOffer];
                            Sitecore.Data.Fields.CheckboxField isTravelExclusive = offerItem.Fields[Constant.isExclusive];
                            Sitecore.Data.Fields.CheckboxField isOfferAndDiscountChkbox = offerItem.Fields[Constant.isOfferAndDiscount];
                            offerDiscountModelList.TabTitle = !string.IsNullOrEmpty(offerItem.Fields[Constant.TabTitle].Value) ? offerItem.Fields[Constant.TabTitle].Value : string.Empty;
                            offerDiscountModelList.PromotionTypeLabel = !string.IsNullOrEmpty(offerItem.Fields[Constant.PromotionTypeLabel].Value) ? offerItem.Fields[Constant.PromotionTypeLabel].Value : string.Empty;
                            offerDiscountModelList.TCEnable = TCEnable.Checked;
                            offerDiscountModelList.IsBankOffer = isbankEnable.Checked;
                            offerDiscountModelList.IsOfferAndDiscount = isOfferAndDiscountChkbox.Checked;
                            offerDiscountModelList.BankOfferText = !string.IsNullOrEmpty(offerItem.Fields[Constant.isBankOfferText].Value) ? offerItem.Fields[Constant.isBankOfferText].Value : string.Empty;
                            offerDiscountModelList.LOB = !string.IsNullOrEmpty(offerItem.Fields[Constant.LOB].Value) ? offerItem.Fields[Constant.LOB].Value : string.Empty;
                            offerDiscountModelList.DisplayedOn = !string.IsNullOrEmpty(offerItem.Fields[Constant.displayedon].Value) ? offerItem.Fields[Constant.displayedon].Value : string.Empty;
                            offerDiscountModelList.offerRedirectionLinkURL = _helper.GetLinkURL(offerItem, Constant.Offerredirection);
                            offerDiscountModelList.offerRedirectionLinkText = _helper.GetLinkText(offerItem, Constant.Offerredirection);
                            offerDiscountModelList.IsExclusive = isTravelExclusive.Checked;
                            offerDiscountModelList.offerServicesRedirectionLink = _helper.GetLinkURL(offerItem, Constant.OfferServicesRedirectionLink);
                            // New Offer Journey Code Commited 
                            //var GTC = !string.IsNullOrEmpty(offerItem.Fields[Constant.GlobalTermsandconditionFieldID].Value) ? offerItem.Fields[Constant.GlobalTermsandconditionFieldID].Value : string.Empty;
                            //offerDiscountModelList.GlobalTermsAndCondition = gettermsconditionlistofstring(GTC);
                            //offerDiscountModelList.seofriendlyUrl = offerItem.Parent.Name + "/" + offerDiscountModelList.offerUniqueID;
                            //offerDiscountModelList.bannerDisplayRank = !string.IsNullOrEmpty(offerItem.Fields[Constant.BannerDisplayRank].Value) ? offerItem.Fields[Constant.BannerDisplayRank].Value : string.Empty;


                            OfferDiscountDataList.Add(offerDiscountModelList);
                            #endregion
                        
                    }



                }


            }
            catch (Exception ex)
            {
                _logRepository.Error(" OfferDiscountService GetOfferListData gives -> " + ex.Message);
            }
            return OfferDiscountDataList;

        }

        private string getstoreTypeID(string storeType)
        {
            string stroreID = string.Empty;
            if (!string.IsNullOrEmpty(storeType))
            {
                switch (storeType.ToLower())
                {
                    case "arrival":
                        stroreID = Constant.Arrival;
                        break;
                    case "departure":
                        stroreID = Constant.Departure;
                        break;
                }
            }
            return stroreID;
        }
        /// <summary>
        /// Unused code need to be removed after testing
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private string getLocationID(string location)
        {
            string LocationID = string.Empty;
            if (!string.IsNullOrEmpty(location))
            {
                switch (location.ToLower())
                {
                    case "svpia-ahmedabad-airport":
                        LocationID = Constant.Ahmedabad;
                        break;
                    case "lgbia-guwahati-airport":
                        LocationID = Constant.Guwahati;
                        break;
                    case "jaipur-airport":
                        LocationID = Constant.Jaipur;
                        break;
                    case "ccsia-lucknow-airport":
                        LocationID = Constant.Lucknow;
                        break;
                    case "thiruvananthapuram-airport":
                        LocationID = Constant.Thiruvananthapuram;
                        break;
                    case "mangaluru-airport":
                        LocationID = Constant.Mangaluru;
                        break;
                    case "csmia-mumbai-airport":
                        LocationID = Constant.Mumbai;
                        break;
                    case "adani-one-airport":
                        LocationID = Constant.adaniOne;
                        break;

                }
            }
            return LocationID;
        }

        private List<Item> GetAppspecificOffers(List<Sitecore.Data.Items.Item> childList, string apptype, string LocationID, string storeType, string isExclusive, string moduleType, string isOfferAndDiscount, bool flag)
        {
            
            List<Item> childlst = new List<Item>();
            
                var filterstring = apptype == Constant.ApptypeFilter ? Constant.WebType : Constant.AppType;
                //Added filter 18831
                childlst = flag ? childList.Where(p => ((Sitecore.Data.Fields.CheckboxField)(p.Fields[filterstring])).Checked &&
                                                    ((Sitecore.Data.Fields.CheckboxField)(p.Fields[Constant.Active])).Checked &&
                                                    ((Sitecore.Data.Fields.CheckboxField)(p.Fields[Constant.CarouselEnable])).Checked &&
                                                    p[Constant.TerminalLocationType].Contains(LocationID)).ToList() :
                                                  childList.Where(p => ((Sitecore.Data.Fields.CheckboxField)(p.Fields[filterstring])).Checked &&
                                                 ((Sitecore.Data.Fields.CheckboxField)(p.Fields[Constant.Active])).Checked).ToList();
                
                if (!string.IsNullOrEmpty(storeType) && childlst != null && childlst.Count() > 0)
                {
                    childlst = childlst.Where(r => r[Constant.TerminalStoreType].Contains(storeType)).ToList();
                }

            //}
            return childlst;
        }

        //New offer Journey Code Roll back
        //private List<OfferAirportModel> GetLocationDetails(MultilistField multilistField, string terminalLocationType)
        //{
        //    List<OfferAirportModel> TerminalLocationType = new List<OfferAirportModel>();
        //    OfferAirportModel offerAirportModel = null;
        //    if (multilistField != null)
        //    {
        //        foreach (Sitecore.Data.Items.Item TLT in multilistField.GetItems())
        //        {
        //            offerAirportModel = new OfferAirportModel
        //            {
        //                title = TLT.Fields[Constant.AirportFieldConstant.Title].Value,
        //                description = TLT.Fields[Constant.AirportFieldConstant.Description].Value,
        //                descriptionApp = TLT.Fields[Constant.AirportFieldConstant.DescriptionApp].Value

        //            };
        //            offerAirportModel.airportCTALink = _helper.GetLinkURL(TLT, Constant.AirportFieldConstant.CTALink);
        //            offerAirportModel.airportCTAText = _helper.GetLinkText(TLT, Constant.AirportFieldConstant.CTALink);
        //            offerAirportModel.airportDesktopImageSrc = _helper.GetImageURL(TLT, Constant.AirportFieldConstant.StanderedImage);
        //            offerAirportModel.airportMobileImageSrc = _helper.GetImageURL(TLT, Constant.AirportFieldConstant.Image);
        //            TerminalLocationType.Add(offerAirportModel);
        //        }
        //    }
        //    return TerminalLocationType;
        //}

        private List<string> GetLocationCode(MultilistField multilistField, string terminalLocationType)
        {
            List<string> TerminalLocationType = new List<string>();
            if (multilistField != null)
            {
                foreach (Sitecore.Data.Items.Item TLT in multilistField.GetItems())
                {
                    TerminalLocationType.Add(TLT.Fields[Constant.Title].Value);
                }
            }
            return TerminalLocationType;
        }
        private string getDictionaryKeyValue(string ID)
        {

            return Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(ID)) != null ? Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(ID)).Fields["Phrase"].Value : String.Empty;

        }
        private bool ValidateOfferExpiry(Sitecore.Data.Fields.DateField field, Sitecore.Data.Fields.DateField effectiveFromfield)
        {
            bool flag = false;
            DateTime effectiveTO;
            DateTime effectiveFrom;
            if (field != null && effectiveFromfield != null)
            {
                effectiveTO = field.DateTime;
                effectiveFrom = effectiveFromfield.DateTime;
                if (_helper.getISTDateTime((System.DateTime.Now).ToString()) >= Convert.ToDateTime(effectiveFrom) && Convert.ToDateTime(effectiveTO) >= _helper.getISTDateTime((System.DateTime.Now).ToString()))
                {
                    flag = true;
                }
            }



            return flag;
        }
        private List<string> gettermsconditionlistofstring(string tC)
        {
            List<string> tcStringList = new List<string>();
            if (!string.IsNullOrEmpty(tC))
            {
                foreach (var tc in tC.Split('$'))
                {
                    tcStringList.Add(tc);
                }
            }
            return tcStringList;
        }

    }
}
   