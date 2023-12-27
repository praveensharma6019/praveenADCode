using System.Collections.Generic;


namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models
{
    public class OfferMapping_old
    {
        public string title { get; set; }
        public string promotionType { get; set; }
        public string promotionCode { get; set; }
        public string skuCode { get; set; }
        public string offerType { get; set; }
        public string displayText { get; set; }
        public string effectiveFrom { get; set; }
        public string effectiveTo { get; set; }
        public string promotionDescription { get; set; }
        public string offerTitle { get; set; }
        public string pcmClaimType { get; set; }
        public string showonHomepage { get; set; }
        public string desktopImageSrc { get; set; }
        public string mobileImageSrc { get; set; }
        public string thumbnailImageSrc { get; set; }
        public string extraImage { get; set; }
        public string linkURL { get; set; }
        public string linkText { get; set; }
        public string displayRank { get; set; }
        public string apptype { get; set; }
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
        public string displayedOn { get; set; }
        public string offerRedirectionLinkText { get; set; }
        public string offerRedirectionLinkURL { get; set; }
        public bool isTravelExclusive { get; set; }
        public string terminalLocationType { get; set; }
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

        public string unlockOfferTitle { get; set; }
        //Ticket No 23293
        public string tabsubtitle { get; set; }
        public string offerServicesRedirectionLink { get; set; }
        public bool isInternational { get; set; }
        public string offerFullScreenImage { get; set; }
        public bool isExpired { get; set; } = false;
        public bool isInstoreOffer { get; set; } = false;
        public List<string> similarOffers { get; set; }
        public string tncViewAllLink { get; set; }
        public bool notClickable { get; set; }
        public MetaTags metaTags { get; set; }
        public StoreInfo storeInfo { get; set; }

        public class MetaTags
        {
            public string metaTitle { get; set; }
            public string metaDescription { get; set; }
            public string keywords { get; set; }
            public string breadcrumbTitle { get; set; }
            public string canonical { get; set; }
        }

        public class StoreInfo
        {
            public string terminalCode { get; set; }
            public string terminalGate { get; set; }
            public string shopId { get; set; }
        }

    }
}