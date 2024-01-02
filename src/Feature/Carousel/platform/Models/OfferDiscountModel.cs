using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{


    public class OfferDiscountModel
    {
        public string Title { get; set; }
        public bool isAirportSelectNeeded { get; set; }
        public string PromotionDescription { get; set; }
        public string PromotionType { get; set; }
        public string PromotionCode { get; set; }
        public string SkuCode { get; set; }
        public string OfferType { get; set; }
        //public string OfferValue { get; set; }
        public string DisplayText { get; set; }
        public string EffectiveFrom { get; set; }
        public string EffectiveTo { get; set; }
        public string ItemPath { get; set; }
        public string AutoId { get; set; }
        public string DesktopImageSrc { get; set; }
        public string DesktopImageAlt { get; set; }
        public string MobileImageSrc { get; set; }
        public string MobileImageAlt { get; set; }
        public string ThumbnailImageSrc { get; set; }
        public string ThumbnailImageAlt { get; set; }
        public string DeepLinkUrl { get; set; }
        public string DeepLinkAlt { get; set; }
        public string OfferTitle { get; set; }
        //Removed for Ticket NO 18278
        //public string PromotionClaimType { get; set; }
        public bool ShowonHomepage { get; set; }
        public string savings { get; set; }
        public string Apptype { get; set; }
        public string BuyQuantity { get; set; }
        public string TCLinkUrl { get; set; }
        public string TCLinkAlt { get; set; }
        public string DisplayRank { get; set; }
        public string ExtraImageSrc { get; set; }
        public string ExtraImageAlt { get; set; }
        public string BannerImageDeskSrc { get; set; }
        public string BannerImageMobSrc { get; set; }
        public string BannerImageDeskAlt { get; set; }
        public string BannerImageMobAlt { get; set; }
        public string CategoryFilter { get; set; }
        public string bannerCondition { get; set; }
        public string ctaText { get; set; }
        public string offerUniqueID { get; set; }
        public string SitecoreOfferID { get; set; }
        public bool TCEnable { get; set; }
        public string PromotionTypeLabel { get; set; }
        public string TabTitle { get; set; }
        public bool IsBankOffer { get; set; }
        public string LOB { get; set; }
        public string DisplayedOn { get; set; }
        public string offerRedirectionLinkText { get; set; }
        public string offerRedirectionLinkURL { get; set; }
        public bool IsExclusive { get; set; }
        public bool IsOfferAndDiscount { get; set; }
        public string BankOfferText { get; set; }
        public List<string> TermCondition { get; set; }
        public List<string> TerminalLocationType { get; set; }
        //public List<OfferAirportModel> TerminalLocationType { get; set; }
        public List<string> TerminalStoreType { get; set; }
        public string offerServicesRedirectionLink { get; set; }
        // Roll Back Code Changes for New Offer Journey
        //public List<string> GlobalTermsAndCondition { get; set; }
        //public string seofriendlyUrl { get; set; }
        //public string bannerDisplayRank { get; set; }

    }

    //Roll Back Code changes for New Offer Journey 
    //public class OfferAirportModel
    //{
    //    public string title { get; set; }
    //    public string description { get; set; }
    //    public string descriptionApp { get; set; }
    //    public string airportCTAText { get; set; }
    //    public string airportCTALink { get; set; }
    //    public string airportDesktopImageSrc { get; set; }
    //    public string airportMobileImageSrc { get; set; }

    //}
}