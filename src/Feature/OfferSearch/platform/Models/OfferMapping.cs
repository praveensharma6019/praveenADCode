using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models
{
    public class OfferMapping
    {
        public string title { get; set; }
        public string promotionType { get; set; }
        public string promotionCode { get; set; }
        public string skuCode { get; set; }
        //public string offerType { get; set; }
        public string displayText { get; set; }
        public string effectiveFrom { get; set; }
        public string effectiveTo { get; set; }
        public string promotionDescription { get; set; }
        public string offerTitle { get; set; }
        //public string pcmClaimType { get; set; }
        //public string showonHomepage { get; set; }
        //public string expiryOption { get; set; }
        //public string validationType { get; set; }
        public string desktopImageSrc { get; set; }
        public string mobileImageSrc { get; set; }
        public string thumbnailImageSrc { get; set; }
        public string extraImage { get; set; }
        public string linkURL { get; set; }
        public string linkText { get; set; }
        public string displayRank { get; set; }
        //public string apptype { get; set; }
        public string tcLinkUrl { get; set; }
        public string tcLinkAlt { get; set; }
        public string savings { get; set; }
        public string bannerImageDeskSrc { get; set; }
        public string bannerImageMobSrc { get; set; }
        public string categoryFilter { get; set; }
        public string offerUniqueID { get; set; }
        public string sitecoreofferId { get; set; }
        public bool tcEnable { get; set; }
        public string autoId { get; set; }
        public string promotionTypeLabel { get; set; }
        public string tabTitle { get; set; }
        public string bankOfferText { get; set; }
        public bool isBankOffer { get; set; }
        public string OfferLOB { get; set; }
        public int OfferDiscountPrice { get; set; }
        public double OfferDiscountPercent { get; set; }
        public string displayedOn { get; set; }
        public string offerRedirectionLinkText { get; set; }
        public string offerRedirectionLinkURL { get; set; }
        public bool isTravelExclusive { get; set; }
        public List<OfferAirportModel> terminalLocationType { get; set; }
        public string terminalStoreType { get; set; }
        public List<string> termCondition { get; set; }
        //Added field In ref to ticket No 17128
        public List<string> howToUse { get; set; }
        public string bookingConfirmedOfferText { get; set; }
        public string bookingConfirmedOfferDescription { get; set; }
        public string offerLogoDesktop { get; set; }
        public string offerLogoMobile { get; set; }
        public string unlockOfferCTAText { get; set; }
        public string UnlockOfferCTALink { get; set; }
        public string unlockOfferCTAVisitWesiteText { get; set; }
        public string unlockOfferCTAVisitWesiteLink { get; set; }
        //Ticket No 19493
        public string unlockOfferTitle { get; set; }

        public List<string> globaltermCondition { get; set; }
        public string seofriendlyUrl { get; set; }
    }

    public class OfferAirportModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string descriptionApp { get; set; }
        public string airportCTAText { get; set; }
        public string airportCTALink { get; set; }
        public string airportDesktopImageSrc { get; set; }
        public string airportMobileImageSrc { get; set; }
    }
}