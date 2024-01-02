using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Http.Cors;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Models
{

    public class SandBoxModel
    {
        public string Firstname { get; set; }
        public string FormType { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Budget { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Remarks { get; set; }
        public string Project { get; set; }
        public string Projectintrested { get; set; }
        public string AssignmentCity { get; set; }
        public string configuration { get; set; }
        public string Message { get; set; }
        public string timeSlot { get; set; }
        public string purpose { get; set; }
        public bool OTPStatus { get; set; }
        public bool userblocked { get; set; }
        public string RecordType { get; set; }
    }
    public class MobileMenu
    {
        public List<object> data { get; set; }
    }
    public class LocatinAboutAdani
    {
        public string Location { get; set; }
        public string heading { get; set; }
        public string about { get; set; }
        public string readMore { get; set; }
        public string Links { get; set; }

        public string terms { get; set; }
        public string detailLink { get; set; }
        public string extrCharges { get; set; }
        public string readMoreLabel { get; set; }
        public string readLessLabel { get; set; }

    }
    public class MobileMenuItem
    {
        public string icon { get; set; }
        public string iconsrc { get; set; }
        public string gifsrc { get; set; }
        public string alt { get; set; }

        public string link { get; set; }
        public string desc { get; set; }

    }
    public class Headerairportlist
    {
        public List<AirportList> airportList { get; set; }
    }
    public class AirportList
    {
        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerImage { get; set; }
        public string headerLogoLink { get; set; }

        public string headerLogoAltText { get; set; }
        public string headerBackText { get; set; }
        public string headerBackLink { get; set; }
        public List<AirportItem> items { get; set; }
    }
    public class AirportItem
    {
        public string itemText { get; set; }
        public string itemLink { get; set; }
        public string itemImage { get; set; }
        public string airportcode { get; set; }
    }

    public class HeaderMenuList
    {
        public List<HamburgerItem> hamburgerMenu { get; set; }
    }
    public class NewHeaderMenuList
    {
        public List<object> data { get; set; }
    }
    public class BreadCrumbModel
    {
        public bool active { get; set; }
        public string href { get; set; }
        public object linkProps { get; set; }
        public string label { get; set; }
    }
    public class ItemPropclass
    {
        public string itemProp { get; set; }
    }
    public class SEOData
    {
        public string pageTitle { get; set; }
        public string metaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaTitle { get; set; }
        public string ogTitle { get; set; }
        public string robotsTags { get; set; }
        public string browserTitle { get; set; }
        public string ogImage { get; set; }
        public string ogDescription { get; set; }
        public string ogKeyword { get; set; }
        public string canonicalUrl { get; set; }
        public string googleSiteVerification { get; set; }
        public string msValidate { get; set; }
        public orgSchema orgSchema { get; set; }
    }
    public class orgSchema
    {
        public orgSchema()
        {
            sameAs = new List<string>();
        }
        public string telephone { get; set; }
        public string contactType { get; set; }
        public string areaServed { get; set; }
        public string streetAddress { get; set; }
        public string addressLocality { get; set; }
        public string addressRegion { get; set; }
        public string postalCode { get; set; }
        public List<string> sameAs { get; set; }
        public string contactOption { get; set; }
        public string logo { get; set; }
        public string url { get; set; }

    }

    public class EnquireForm
    {
        public string enquireNow { get; set; }
        public string popupTitle { get; set; }
        public string popupSubTitle { get; set; }
        public string planAVsiit { get; set; }
        public string ContactUsTitle { get; set; }
        public string PropertyLabel { get; set; }
        public string shareContact { get; set; }
        public string sendusQuery { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string projectType { get; set; }
        public List<Dropdown> ProjectTypeOptions { get; set; }
        public string selectPropertyType { get; set; }
        public List<Dropdown> PropertyTypeOptions { get; set; }
        public string agreeToConnect { get; set; }
        public string overrideRegistry { get; set; }
        public string submitDetail { get; set; }
        public string selectProjectProperty { get; set; }
        public string configuration { get; set; }
        public string StartDate { get; set; }
        public string homeLoanInterested { get; set; }
        public string timeSlots { get; set; }
        public List<Dropdown> TimeSlotsOptions { get; set; }
        public string date { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string heading { get; set; }
        public string para { get; set; }
        public string cancelLabel { get; set; }
        public string continueLabel { get; set; }
        public string description { get; set; }
        public string PrintLabel { get; set; }
        public string saveaspdfLabel { get; set; }
        public string DoneButtonLabel { get; set; }
        public string MobileLabel { get; set; }
        public string SubmitButtonText { get; set; }
        public string EnterOTPLabel { get; set; }
        public string WehavesentviaSMStoLabel { get; set; }
        public string HavenotreceivedaOTPLabel { get; set; }
        public string editButtonLable { get; set; }
        public string ResendButtonLabel { get; set; }
        public string PurposeLabel { get; set; }
        public string errorMessageTitle { get; set; }
        public string errorMessageDesription { get; set; }
        public List<Dropdown> PurposeList { get; set; }
        public ErrorData errorData { get; set; }
        public string BrochureHeading { get; set; }
        public string messageLabel { get; set; }
        public string messageMaxLength { get; set; }
        public string BrochureFormDescription { get; set; }
        public string BrochureThankyouDescription { get; set; }
    }
    public class Dropdown
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string controllerName { get; set; }
    }
    public class ErrorData
    {
        public string name { get; set; }
        public string planAVsiit { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string email { get; set; }
        public string messageError { get; set; }
        public string phoneNo { get; set; }
        public string projectType { get; set; }
        public string projectName { get; set; }
        public string configuration { get; set; }
        public string timeslot { get; set; }
        public string contactAdaniRealty { get; set; }
        public string purpose { get; set; }
    }
    public class NewHeaderMenuItem
    {
        public NewHeaderMenuItem()
        {
            items = new List<NewcollapseItemslist>();
        }
        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }
        public bool fatNav { get; set; }
        public string defaultImage { get; set; }
        public string defaultLogo { get; set; }
        public bool headerCallback { get; set; }
        public List<NewcollapseItemslist> items { get; set; }
    }
    public class NewcollapseItemslist
    {
        public string sectionTitle { get; set; }
        public string fatNavSectionLink { get; set; }
        public List<SectionPromoImages> sectionPromoImages { get; set; }
        public string itemText { get; set; }
        public string itemSubText { get; set; }
        public string itemLink { get; set; }
        public string itemLeftIcon { get; set; }
        public string itemRightIcon { get; set; }
        public List<InnerSection> items { get; set; }
        public List<NewcollapseItemslist> sectionItems { get; set; }
    }
    public class SectionPromoImages
    {
        public string promoImage { get; set; }
        public string promoLogo { get; set; }
        public string promoLink { get; set; }
        public string promoAltText { get; set; }

    }
    public class InnerSection
    {
        public string sectionTitle { get; set; }
        public string sectionTitleLink { get; set; }
        public string sectionImage { get; set; }
        public string sectionLogo { get; set; }
        public List<sampaleTemplate> sectionItems { get; set; }
    }
    public class sampaleTemplate
    {
        public string itemText { get; set; }
        public string itemLink { get; set; }
        public string target { get; set; }
        public string itemLogo { get; set; }
        public string itemImage { get; set; }
        public bool linkHeading { get; set; }
    }
    public class HamburgerItem
    {
        public string logosrc { get; set; }
        public string logoAlt { get; set; }
        public List<HeaderMenuItem> adaniRealty { get; set; }
        public List<HeaderMenuItem> information { get; set; }
        public List<AdaniBusinessItem> adaniBusinesses { get; set; }
        public List<HeaderMenuItem> helpandsupport { get; set; }
        //public List<HeaderMenuItem> others { get; set; }
    }
    public class topNavigationModel
    {
        public List<HeaderMenuItem> topNavigation { get; set; }
    }
    public class HeaderMenuItem
    {
        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }
        public List<collapseItemslist> items { get; set; }
    }
    public class AdaniBusinessItem
    {
        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }
        public List<collapseItemslist> collapseItems { get; set; }
    }
    public class collapseItemslist
    {
        public string itemText { get; set; }
        public string itemSubText { get; set; }
        public string itemLink { get; set; }
        public string target { get; set; }
        public string itemLeftIcon { get; set; }
        public string itemRightIcon { get; set; }
        public string itemImage { get; set; }
        public bool linkHeading { get; set; }
        public List<collapseItemslist> collapseItems { get; set; }
    }
    public class otherProjects
    {
        public paramdata param { get; set; }
        public List<projectdata> data { get; set; }

    }
    public class paramdata
    {
        public string ressOffer { get; set; }
    }

    public class projectdata
    {
        public string propertyID { get; set; }
        public string link { get; set; }
        public string linktarget { get; set; }
        public string logo { get; set; }
        public string logotitle { get; set; }
        public string logoalt { get; set; }
        public string src { get; set; }
        public string mobileimage { get; set; }
        public string imgalt { get; set; }
        public string title { get; set; }
        public string imgtitle { get; set; }
        public string location { get; set; }
        public string type { get; set; }
        public string imgtype { get; set; }
        public string propertyType { get; set; }
        public string latitude { get; set; }
        public string logitude { get; set; }
        public string city { get; set; }
    }
}