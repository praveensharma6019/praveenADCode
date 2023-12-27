using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.DutyFreeProductImport.Platform.Models
{
    public class CIPromotionOfferModel
    {
        public DateTime createdAt { get; set; }
        public string createdBy { get; set; }
        public DateTime? updatedAt { get; set; }
        public string updatedBy { get; set; }
        public object eventId { get; set; }
        public int ownerRef { get; set; }
        public int uid { get; set; }
        public string resourceRef { get; set; }
        public string pcmName { get; set; }
        public string pcmDescription { get; set; }
        public string pcmType { get; set; }
        public object pcmRewardCurrencyId { get; set; }
        public bool pcmIsOTPRequired { get; set; }
        public string pcmCurrency { get; set; }
        public string pcmStatus { get; set; }
        public string pcmClaimType { get; set; }
        public string pcmAssignType { get; set; }
        public int pcmAllowDupCusPromo { get; set; }
        public string pcmValueType { get; set; }
        public string pcmNameValidationType { get; set; }
        public int pcmExpiryMin { get; set; }
        public double pcmPercentage { get; set; }
        public int pcmCappingEnabled { get; set; }
        public double pcmCappingValue { get; set; }
        public int pcmRetLowestVal { get; set; }
        public int pcmAllowDiffCurrency { get; set; }
        public string pcmStaticPromoCode { get; set; }
        public double pcmPromoValue { get; set; }
        public int pcmPromoCodeStockQty { get; set; }
        public int pcmRestrictCusClaim { get; set; }
        public string pcmCusClaimLimitPeriod { get; set; }
        public int pcmCusClaimPerDay { get; set; }
        public int pcmMaxClaimPerCustomer { get; set; }
        public string pcmExpiryOption { get; set; }
        public string pcmExpiryDate { get; set; }
        public int pcmExpiryValue { get; set; }
        public string pcmExpiryPeriodAdjType { get; set; }
        public int pcmPromoAutogenRangeEnabled { get; set; }
        public int pcmEnableCustomSpiel { get; set; }
        public string pcmSpielName { get; set; }
        public string pcmAutogenPromoPrefix { get; set; }
        public int? pcmAutogenPromoFromIndex { get; set; }
        public int? pcmAutogenPromoCurrIndex { get; set; }
        public int pcmEnablePadding { get; set; }
        public object pcmPaddingCharacter { get; set; }
        public object pcmPaddingLength { get; set; }
        public double pcmAmountRangeFrom { get; set; }
        public double pcmAmountRangeTo { get; set; }
        public int pcmEnableExternalLink { get; set; }
        public int pcmEnableShortenLink { get; set; }
        public object pcmExternalBaseUrl { get; set; }
        public object pcmShortenLinkProvider { get; set; }
        public object pcmPromoImgUrl { get; set; }
        public string pcmTermsAndCondition { get; set; }
        public int? pcmEnableSkuRule { get; set; }
        public int? pcmSwitchSkuPrice { get; set; }
        public int? pcmSkuDiscountQty { get; set; }
        public string pcmSkuMatchType { get; set; }
        public List<PromoSkuRuleList> promoSkuRuleList { get; set; }
        public object pcmDiscountSkuType { get; set; }
        public string pcmDiscountSkuValue { get; set; }
        public bool? pcmIsDiscountSkuPercentage { get; set; }
        public bool? pcmEnableBinFilter { get; set; }
        public string pcmBinType { get; set; }
        public string pcmBinValue { get; set; }
        public object pcmEligibleChannels { get; set; }
        public object pcmEligibleLocations { get; set; }
        public string pcmEligibleCategories { get; set; }
        public string pcmLocFilterType { get; set; }
        public int pcmEnableAutoApply { get; set; }
        public int version { get; set; }
        public string pcmMasterExpiryDate { get; set; }
        public object channelName { get; set; }
        public object resourceClass { get; set; }
        public string pcmPromoEffectiveFrom { get; set; }

    }
   

    public class PromoSkuRuleList
    {
        public object createdAt { get; set; }
        public object createdBy { get; set; }
        public object updatedAt { get; set; }
        public object updatedBy { get; set; }
        public object eventId { get; set; }
        public int ownerRef { get; set; }
        public int psrId { get; set; }
        public int psrPromoSku { get; set; }
        public string psrSkuType { get; set; }
        public string psrSkuValue { get; set; }
        public int psrSkuQty { get; set; }
        public string psrSkuComparator { get; set; }
        public object psrSkuPriceFrom { get; set; }
        public object psrSkuPriceTo { get; set; }
        
    }

    public class OfferModelList
    {
        public List<CIPromotionOfferModel> data { get; set; }
        public string status { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<OfferModelList>(myJsonResponse);

}