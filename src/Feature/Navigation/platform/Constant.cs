using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform
{
    public class Constant
    {
        public static string RenderingParamField = "Widget";
        public static readonly Guid CityToCityTemplateId = new Guid("{F30C68F5-3783-4F1E-BDCB-C6538BB2D9AA}");
        public static readonly Guid SubCategoryTemplate = new Guid("{2F8490CE-ABB5-4676-8194-5430203E57F1}");
        public static readonly Guid CategoryTemplate = new Guid("{5B0D0497-19EC-44CC-B5AB-9E0245933BEC}");
        public static readonly Guid MaterialGroupTemplate = new Guid("{F6E33521-E0A2-48D3-8D7C-AC33FC301E5C}");
        public static readonly Guid BannerTemplate = new Guid("{F48CCC70-AF9E-4300-AA81-6A3CFD2FD07D}");
        public static readonly string restrictedGroup = "liquor";

        public static readonly string Title = "Title";
        public static readonly string Image = "Image";
        public static readonly string Link = "Link";
        public static readonly string LogoClass = "LogoClass";
        public static readonly string CDNPath = "CDNPath";
        public static readonly string Age = "Age";
        public static readonly string AirportCode = "AirportCode";
        public static readonly string airportLogo = "airportLogo";
        public static readonly string brandLogo = "brandLogo";
        public static readonly string brandLogoColored = "brandLogoColored";
        public static readonly string airportURLName = "airportURLName";
        public static readonly string airportName = "airportName";

        public static readonly string Thumbnailimage = "Thumbnailimage";
        public static readonly string MainImage = "MainImage";
        public static readonly string IconImages = "IconImages";

        public static readonly string DesktopImage = "Desktop Image";
        public static readonly string MobileImage = "Mobile Image";

        public static readonly string MaterialGroupCode = "Material Group Code";

        public static readonly string Code = "Code";
        public static readonly string Name = "Name";

        public static readonly string Brands = "Brands";
        public static readonly string Brand_Name = "Brand Name";
        public static readonly string Brand_Code = "Brand Code";
        public static readonly string Brand_Material_Group = "Material Group";
        public static readonly string Brand_Description = "Brand Description";
        public static readonly string Brand_CDN_Image = "Brand CDN Image";

        public static readonly string SortOrder = "Sort Order";

        public static readonly string TagName = "name";

        /// <summary>
        ///Navigation constants
        /// </summary>
        public static readonly string Link_Title = "LinkTitle";
        public static readonly string Link_Url = "LinkUrl";
        public static readonly string Link_leftIcon = "LeftIconClass";
        public static readonly string Link_rightIcon = "RightIconClass";
        public static readonly string Link_isAgePopup = "IsAgePopup";

        public static readonly string Active_Image = "ActiveImage";
        public static readonly string Image_Path = "ImagePath";

        public static readonly string LogoutRendering = "{39DCD06C-B5F0-41A5-863B-39F3BDA94551}";
        public static readonly string LoyaltyPopupRendering = "{B3F8A1D5-762B-42EA-98DF-074C3EE12E86}";
        public static readonly string TerminalPopupRendering = "{7D9550A3-12CF-44FD-AF44-F073BD44911D}";
        public static readonly string CookieConsentRendering = "{7365FFAA-AE2E-4F03-A6B6-FD67C895BFCB}";
        public static readonly string TerminalChangePopupRendering = "{F6AFBCAF-B5E0-439F-B4C0-D442782DFB3F}";
        public static readonly string PranaamCartClearPopupRendering = "{15EA9497-150B-49F6-97B0-0A0B81DD7842}";


        public static readonly string seoContentItemName = "Seo Contents";
        public static readonly string servicesItemName = "Services";
        public static readonly string brandItemName = "Brands";
        public static readonly string MainNavigationItemName = "Main Navigations";
        public static readonly string TravelHelpItemName = "TravelHelp";
        public static readonly string FlightInformationItemName = "FlightInformation";
        public static readonly string AdaniBusinessesItemName = "AdaniBusinesses";
        public static readonly string CompanyItemName = "Company";
        public static readonly string HelpAndSupportItemName = "HelpAndSupport";
        public static readonly string paymentItemName = "Payments";
        public static readonly string FooterSocialLinksItemName = "Footer Social Links";
        public static readonly string DownloadItemName = "Download";
        public static readonly string FooterCopyRightItemName = "Footer copyright";
        public static readonly string FooterContactUsItemName = "FooterContactUs";
        public static readonly string BottomNavItemName = "BottomNav";
        public static readonly string footerBottomNavName = "footerBottomNav";
        public static readonly string Routes = "Routes";

        /// <summary>
        /// Header navigation items
        /// </summary>
        public static readonly string TopNavLeftIcons = "TopNavLeftIcons";
        public static readonly string TopNavigationID = "TopNavigation";
        public static readonly string HamburgerMenuID = "HamburgerMenu";
        public static readonly string AdaniAirportID = "AdaniAirport";
        public static readonly string OtherID = "Other";
        public static readonly string AdaniBusinessesID = "AdaniBusinesses";
        public static readonly string PrimaryHeaderMenuID = "PrimaryHeaderMenu";
        public static readonly string SearchSuggesterID = "SearchSuggester";
        public static readonly string UserAccountID = "UserAccount";
        public static readonly string MyAccountsID = "MyAccounts";
        public static readonly string LogoDropdownID = "LogoDropdown";
        public static readonly string AirportListID = "AirportList";
        public static readonly string HeaderLogoID = "HeaderLogos";
        public static readonly string QuickLinksID = "QuickLinks";
        public static readonly string AdaniRewards = "adani rewards";

        //Adani AIrports name
        public const string ahmedabad = "svpia-ahmedabad-airport";
        public const string guwahati = "lgbia-guwahati-airport";
        public const string thiruvananthapuram = "thiruvananthapuram-airport";
        public const string jaipur = "jaipur-airport";
        public const string lucknow = "ccsia-lucknow-airport";
        public const string mangaluru = "mangaluru-airport";
        public const string mumbai = "csmia-mumbai-airport";
    }
}