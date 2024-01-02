using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.DutyFreeProductImport.Platform.Models;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using static Adani.SuperApp.Airport.Feature.DutyFreeProductImport.Platform.Models.DBOfferModel;

namespace Adani.SuperApp.Airport.Feature.DutyFreeProductImport.Platform.Command
{
    public class OfferImports
    {
        LogRepository _logRepository = new LogRepository();
        int index = 1;
        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
           // _logRepository.Info("Offer scheduled task is being run!");
             ReadOfferJson();
        }

        private void ReadOfferJson()
        {
            #region Remove After Testing
            // StreamReader sr = new StreamReader("D:\\OneDrive - Adani\\Desktop\\OfferJSON.json");
            // string jsonString = sr.ReadToEnd();
            //sr.Close();
            //var jsonObject = JsonConvert.DeserializeObject<OfferModelList>(jsonString);

            #endregion
            APIResponse aPIResponse = new APIResponse();
            _logRepository.Info("get Offers form OfferImportAPI");
            var CIresponse = aPIResponse.GetAPIResponse("GET", Sitecore.Configuration.Settings.GetSetting("AllPromotionsAPI"), null, null, "");
            var CIaPIresult = JsonConvert.DeserializeObject<OfferModelList>(CIresponse);
            var DBresponse = aPIResponse.GetAPIResponse("GET", Sitecore.Configuration.Settings.GetSetting("AllPromotionsDBAPI"), null, null, "");
            var DBaPIresult = JsonConvert.DeserializeObject<DBOfferDataModelResponse>(DBresponse);
            
            CreateCIPromotion(CIaPIresult.data,"CI");
            CreateDBPromotion(DBaPIresult.data, "DB");

        }
        private void CreateCIPromotion(List<CIPromotionOfferModel> data,string contentFrom)
        {
            try
            {
                Sitecore.Data.Database contextDB = GetContextDatabase();
                _logRepository.Info("Gets the context database->" + contextDB.ToString());
                Sitecore.Data.Items.Item parentItem = contextDB.GetItem(Constant.OfferListFolderID);
                string parentItemPath = parentItem.Paths.FullPath;
                _logRepository.Info("Gets the Parent Item under which Promotion need to be created->" + parentItem.ID.ToString());
                var offerTemplate = contextDB.GetTemplate(Constant.PromoOfferTemplateID);
                _logRepository.Info("Gets the Template for Promotion creation->" + offerTemplate.ID.ToString());
                _logRepository.Info("Checks for the null of Parent folder and the template");
                if (parentItem != null && offerTemplate != null)
                {
                    _logRepository.Info("Checks passed for the Parent folder and the template");
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        _logRepository.Info("Disabeling Sitecore Security for content creation");
                        _logRepository.Info("Number of Promotions to be imported -> " + data.Count);
                        foreach (var promotion in data)
                        {
                           _logRepository.Info("Offer with "+ promotion.pcmStaticPromoCode +" importing start ->" + promotion.uid);
                            Item newItem = null;
                            bool isExist = false;
                            try
                            {
                                newItem = GetExistingItemBasedOnLanguage(parentItemPath + "/" + contentFrom + "-" + promotion.pcmStaticPromoCode);
                                _logRepository.Info("Checks if the item already exists ->" + promotion.uid);
                                if (newItem == null)
                                {
                                    newItem = parentItem.Add(contentFrom + "-" + promotion.pcmStaticPromoCode, offerTemplate);
                                }
                                else
                                {
                                    isExist = true;
                                }
                                _logRepository.Info("Item Id of the item to be added -> " + newItem.ID.ToString());
                                if (newItem != null)
                                {
                                    try
                                    {
                                        if (isExist)
                                            newItem = newItem.Versions.AddVersion();
                                        newItem.Editing.BeginEdit();
                                        newItem.Appearance.DisplayName = contentFrom + "-" + promotion.pcmStaticPromoCode;
                                        newItem.Fields[Constant.PromoOffers.Title].Value = !string.IsNullOrEmpty(promotion.pcmName.ToString()) ? promotion.pcmName.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.PromotionDescription].Value = !string.IsNullOrEmpty(promotion.pcmDescription.ToString()) ? promotion.pcmDescription.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.PromotionType].Value = !string.IsNullOrEmpty(promotion.pcmEligibleCategories.ToString()) ? promotion.pcmEligibleCategories.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.PromotionCode].Value = !string.IsNullOrEmpty(promotion.pcmStaticPromoCode.ToString()) ? promotion.pcmStaticPromoCode.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.OfferType].Value = !string.IsNullOrEmpty(promotion.pcmValueType.ToString()) ? promotion.pcmValueType.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.DisplayText].Value = !string.IsNullOrEmpty(promotion.pcmValueType.ToString()) && !string.IsNullOrEmpty(promotion.pcmPromoValue.ToString()) ? getDisplayText(promotion.pcmValueType.ToString(), promotion.pcmPromoValue.ToString()) : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.OfferId].Value = !string.IsNullOrEmpty(promotion.resourceRef.ToString()) ? promotion.resourceRef.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.PromotionClaimType].Value = !string.IsNullOrEmpty(promotion.pcmClaimType.ToString()) ? promotion.pcmClaimType.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.Currency].Value = !string.IsNullOrEmpty(promotion.pcmCurrency.ToString()) ? promotion.pcmCurrency.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.TerminalStoreType].Value = promotion.pcmEligibleLocations != null && !string.IsNullOrEmpty(promotion.pcmEligibleLocations.ToString()) ? promotion.pcmEligibleLocations.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.TermsandCondition].Value = !string.IsNullOrEmpty(promotion.pcmTermsAndCondition.ToString()) ? promotion.pcmTermsAndCondition.ToString() : string.Empty;
                                        Sitecore.Data.Fields.DateField efd = newItem.Fields[Constant.PromoOffers.EffectiveFrom];
                                        efd.Value = promotion.pcmPromoEffectiveFrom != null ? getSitecoreDate(promotion.pcmPromoEffectiveFrom).ToString() : System.DateTime.Now.ToString();
                                        Sitecore.Data.Fields.DateField eftd = newItem.Fields[Constant.PromoOffers.EffectiveTo];
                                        eftd.Value = promotion.pcmExpiryDate != null ? getSitecoreDate(promotion.pcmExpiryDate).ToString() : System.DateTime.Now.ToString();

                                        newItem.Fields[Constant.PromoOffers.EffectiveTo].Value = !string.IsNullOrEmpty(promotion.pcmTermsAndCondition.ToString()) ? promotion.pcmTermsAndCondition.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.ExpiryOption].Value = !string.IsNullOrEmpty(promotion.pcmExpiryOption.ToString()) ? promotion.pcmExpiryOption.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.ValidationType].Value = !string.IsNullOrEmpty(promotion.pcmNameValidationType.ToString()) ? promotion.pcmNameValidationType.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.DisplayRank].Value = index.ToString();
                                        newItem.Fields[Constant.PromoOffers.promoDesktopImage].Value = "/sitecore/media library/Project/Adani/Offers/noImage";
                                        newItem.Fields[Constant.PromoOffers.promoMobileImage].Value = "/sitecore/media library/Project/Adani/Offers/noImage";
                                        newItem.Fields[Constant.PromoOffers.promoThumbnailImage].Value = "/sitecore/media library/Project/Adani/Offers/noImage";
                                        newItem.Editing.EndEdit();
                                        _logRepository.Info("Offer Imported Successfully -> " + newItem.ID.ToString());
                                        index = index + 1;
                                    }
                                    catch (Exception ex)
                                    {

                                        _logRepository.Error(promotion.pcmStaticPromoCode + " Item creation failed ->  " + ex.Message);
                                    }
                                    
                                }
                            }
                            catch (Exception ex)
                            {
                                _logRepository.Error("Item creation failed due to -> " + ex.Message);
                            }
                        }
                    }
                }

                PublishOffers(parentItem);

            }
            catch (Exception ex)
            {
                _logRepository.Error("CreatePromotion method failed due to -> " + ex.Message);
            }
        }

        private void CreateDBPromotion(List<DBOfferDataModel> data, string contentFrom)
        {
            try
            {
                Sitecore.Data.Database contextDB = GetContextDatabase();
                _logRepository.Info("Gets the context database->" + contextDB.ToString());
                Sitecore.Data.Items.Item parentItem = contextDB.GetItem(Constant.OfferListFolderID);
                string parentItemPath = parentItem.Paths.FullPath;
                _logRepository.Info("Gets the Parent Item under which Promotion need to be created->" + parentItem.ID.ToString());
                var offerTemplate = contextDB.GetTemplate(Constant.PromoOfferTemplateID);
                _logRepository.Info("Gets the Template for Promotion creation->" + offerTemplate.ID.ToString());
                _logRepository.Info("Checks for the null of Parent folder and the template");
                if (parentItem != null && offerTemplate != null)
                {
                    _logRepository.Info("Checks passed for the Parent folder and the template");
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        _logRepository.Info("Disabeling Sitecore Security for content creation");
                        _logRepository.Info("Number of Promotions to be imported -> " + data.Count);
                        foreach (var promotion in data)
                        {
                            _logRepository.Info("Offer with " + promotion.promotionCode + " importing start ->" + promotion.id);
                            Item newItem = null;
                            bool isExist = false;
                            try
                            {
                                newItem = GetExistingItemBasedOnLanguage(parentItemPath + "/" + contentFrom + "-" + promotion.promotionCode);
                                _logRepository.Info("Checks if the item already exists ->" + promotion.id);
                                if (newItem == null)
                                {
                                    newItem = parentItem.Add(contentFrom + "-" + promotion.promotionCode, offerTemplate);
                                }
                                else
                                {
                                    isExist = true;
                                }
                                _logRepository.Info("Item Id of the item to be added -> " + newItem.ID.ToString());
                                if (newItem != null)
                                {
                                    try
                                    {
                                        if (isExist)
                                            newItem = newItem.Versions.AddVersion();
                                        newItem.Editing.BeginEdit();
                                        newItem.Appearance.DisplayName = contentFrom + "-" + promotion.promotionCode;
                                        newItem.Fields[Constant.PromoOffers.Title].Value = !string.IsNullOrEmpty(promotion.offerDisplayText.ToString()) ? promotion.offerDisplayText.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.PromotionDescription].Value = !string.IsNullOrEmpty(promotion.offerDisplayText.ToString()) ? promotion.offerDisplayText.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.PromotionType].Value = "dutyfree";
                                        newItem.Fields[Constant.PromoOffers.PromotionCode].Value = !string.IsNullOrEmpty(promotion.promotionCode.ToString()) ? promotion.promotionCode.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.OfferType].Value = !string.IsNullOrEmpty(promotion.offerType.ToString()) ? promotion.offerType.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.DisplayText].Value = !string.IsNullOrEmpty(promotion.offerDisplayText.ToString()) && !string.IsNullOrEmpty(promotion.offerDisplayText.ToString()) ? getDisplayText(promotion.offerType.ToUpper().ToString(), promotion.offer.ToString()) : string.Empty;
                                        //newItem.Fields[Constant.PromoOffers.OfferId].Value = !string.IsNullOrEmpty(promotion.resourceRef.ToString()) ? promotion.resourceRef.ToString() : string.Empty;
                                        //newItem.Fields[Constant.PromoOffers.PromotionClaimType].Value = !string.IsNullOrEmpty(promotion.pcmClaimType.ToString()) ? promotion.pcmClaimType.ToString() : string.Empty;
                                        //newItem.Fields[Constant.PromoOffers.Currency].Value = !string.IsNullOrEmpty(promotion.pcmCurrency.ToString()) ? promotion.pcmCurrency.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.TerminalStoreType].Value = promotion.operatorType != null && !string.IsNullOrEmpty(promotion.operatorType.ToString()) ? promotion.operatorType.ToString() : string.Empty;
                                        //newItem.Fields[Constant.PromoOffers.TermsandCondition].Value = !string.IsNullOrEmpty(promotion.pcmTermsAndCondition.ToString()) ? promotion.pcmTermsAndCondition.ToString() : string.Empty;
                                        Sitecore.Data.Fields.DateField efd = newItem.Fields[Constant.PromoOffers.EffectiveFrom];
                                        efd.Value = promotion.effectiveFrom != null ? promotion.effectiveFrom.ToString() : System.DateTime.Now.ToString();
                                        Sitecore.Data.Fields.DateField eftd = newItem.Fields[Constant.PromoOffers.EffectiveTo];
                                        eftd.Value = promotion.effectiveTo != null ? promotion.effectiveTo.ToString() : System.DateTime.Now.ToString();

                                        //newItem.Fields[Constant.PromoOffers.EffectiveTo].Value = !string.IsNullOrEmpty(promotion.pcmTermsAndCondition.ToString()) ? promotion.pcmTermsAndCondition.ToString() : string.Empty;
                                        //newItem.Fields[Constant.PromoOffers.ExpiryOption].Value = !string.IsNullOrEmpty(promotion.pcmExpiryOption.ToString()) ? promotion.pcmExpiryOption.ToString() : string.Empty;
                                        //newItem.Fields[Constant.PromoOffers.ValidationType].Value = !string.IsNullOrEmpty(promotion.pcmNameValidationType.ToString()) ? promotion.pcmNameValidationType.ToString() : string.Empty;
                                        newItem.Fields[Constant.PromoOffers.DisplayRank].Value = index.ToString();
                                        //newItem.Fields[Constant.PromoOffers.promoDesktopImage].Value = "/sitecore/media library/Project/Adani/Offers/noImage";
                                        //newItem.Fields[Constant.PromoOffers.promoMobileImage].Value = "/sitecore/media library/Project/Adani/Offers/noImage";
                                        //newItem.Fields[Constant.PromoOffers.promoThumbnailImage].Value = "/sitecore/media library/Project/Adani/Offers/noImage";

                                        newItem.Editing.EndEdit();
                                        _logRepository.Info("Offer Imported Successfully -> " + newItem.ID.ToString());
                                        index = index + 1;
                                    }
                                    catch (Exception ex)
                                    {

                                        _logRepository.Error(promotion.promotionCode + " Item creation failed ->  " + ex.Message);
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                _logRepository.Error("Item creation failed due to -> " + ex.Message);
                            }
                        }
                    }
                }

                PublishOffers(parentItem);

            }
            catch (Exception ex)
            {
                _logRepository.Error("CreateDBPromotion method failed due to -> " + ex.Message);
            }
        }
        private DateTime getSitecoreDate(string v)
        {
            return Sitecore.DateUtil.IsoDateToDateTime(v);
        }

        private Sitecore.Data.Database GetContextDatabase()
        {
            return Sitecore.Context.ContentDatabase;
        }
        private Sitecore.Data.Items.Item GetExistingItemBasedOnLanguage(string ItemPath)
        {
            return GetContextDatabase().GetItem(ItemPath, Sitecore.Context.Language);
        }
        private string getDisplayText(string type,string value)
        {
            string displaytext = string.Empty;
            switch(type)
            {
                case "PERCENTAGE":
                        displaytext = "Flat " + value + "% OFF";
                    break;
                case "'ABSOLUTE'":
                    displaytext = "Flat " + value + " OFF";
                    break;


            }
            return displaytext;


        }
        private void PublishOffers(Sitecore.Data.Items.Item item)
        {
            _logRepository.Info("Offer Publish Started ");
            try
            {
                Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase(Constant.master);
                _logRepository.Info("Gets the master/Source database ");
                Sitecore.Data.Database web = Sitecore.Configuration.Factory.GetDatabase(Constant.web);
                _logRepository.Info("Gets the Web/Target database ");
                Sitecore.Publishing.PublishOptions publishOptions = new Sitecore.Publishing.PublishOptions(master,
                                        web,
                                        Sitecore.Publishing.PublishMode.SingleItem,
                                        item.Language,
                                        System.DateTime.Now);
                _logRepository.Info("Publish started -> " + System.DateTime.Now.ToString());
                Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);
                _logRepository.Info("Set publish Options -> ");
                publisher.Options.RootItem = item;
                _logRepository.Info("Set Root Item ");
                publisher.Options.Deep = true;

                publisher.Publish();
                _logRepository.Info("Item Published ");
            }
            catch (Exception ex)
            {

                _logRepository.Error(" Offers Published Failed due to -> " + ex.Message);
            }

        }
    }
}