using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.Models
{
    public class HeaderData
    {
        public List<HeaderDetails> HeaderDetails { get; set; }

    }

    public class HeaderDetails
    {
        // public string HeaderName { get; set; }
        public List<TopNavigation> TopNavigation { get; set; }

        public List<TopNavigation> TopNavigationLeftIcons { get; set; }
        public List<HamburgerMenu> HamburgerMenu { get; set; }
        public List<HeaderLogoDropdown> HeaderLogoDropdown { get; set; }
        public List<PrimaryHeaderMenu> PrimaryHeaderMenus { get; set; }
        public List<SearchSuggester> searchSuggesters { get; set; }
        public List<UserAccount> userAccounts { get; set; }
        public List<MyAccounts> MyAccounts { get; set; }

        public List<AirportList> AirportList { get; set; }
        public List<HeaderLogos> HeaderLogos { get; set; }


    }
    public class TopNavigation
    {
        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }
        public bool IsAgePopup { get; set; }
        public string LinkTarget { get; set; }
        public List<ChildNavigation> items { get; set; }
    }

    public class HamburgerMenu
    {
        //  public List<Information> Information { get; set; }

        //  public List<AdaniElectricity> AdaniElectricity { get; set; }
        public List<AdaniAirport> AdaniAirport { get; set; }
        public List<AdaniBusinesses> AdaniBusinesses { get; set; }
        public List<Others> Others { get; set; }
        

    }
    public class Information
    {
        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }
        public string LinkTarget { get; set; }

        public List<ChildNavigation> items { get; set; }
    }
    public class MyAccounts
    {

        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }
        public string LinkTarget { get; set; }

        public List<ChildNavigation> items { get; set; }
    }

    public class AdaniAirport
    {

        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }
        public bool IsAgePopup { get; set; }
        public string LinkTarget { get; set; }

        public List<ChildNavigation> items { get; set; }
    }

    public class Others
    {

        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }
        public bool IsAgePopup { get; set; }
        public string LinkTarget { get; set; }

        public List<ChildNavigation> items { get; set; }
    }
    public class AdaniElectricity
    {

        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }

        public List<ChildNavigation> items { get; set; }
    }
    public class AdaniBusinesses
    {

        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }
        public bool IsAgePopup { get; set; }
        public string LinkTarget { get; set; }

        public List<ChildNavigation> collapseItems { get; set; }
    }
    public class PrimaryHeaderMenu
    {

        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }
        public bool IsAgePopup { get; set; }
        public string LinkTarget { get; set; }
        public List<ChildNavigation> items { get; set; }
    }
    public class SearchSuggester
    {

        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }
        public string LinkTarget { get; set; }

        public List<ChildNavigation> items { get; set; }
    }

    public class UserAccount
    {

        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerLeftIcon { get; set; }
        public string headerRightIcon { get; set; }
        public string LinkTarget { get; set; }

        public List<ChildNavigation> items { get; set; }
    }

    public class ChildNavigation
    {

        public string itemText { get; set; }
        public string itemLink { get; set; }
        public string itemLeftIcon { get; set; }
        public string itemRightIcon { get; set; }
        public bool IsAgePopup { get; set; }
        public string LinkTarget { get; set; }
        public string tagName { get; set; }

        public List<ChildNavigation> collapseItems { get; set; }

    }

    public class HeaderLogoDropdown
    {
        public string itemText { get; set; }
        public string itemLink { get; set; }
        public string itemLeftIcon { get; set; }
        public string itemRightIcon { get; set; }
        public string LinkTarget { get; set; }

        public List<ChildNavigation> items { get; set; }


    }

    public class AirportList
    {
        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerImage { get; set; }       
        public List<ChildNavigationwithImage> items { get; set; }


    }

    public class HeaderLogos
    {
        public string headerText { get; set; }
        public string headerLink { get; set; }
        public string headerImage { get; set; }
        public List<ChildNavigationwithImage> items { get; set; }


    }
    public class ChildNavigationwithImage
    {
        public string itemText { get; set; }
        public string itemLink { get; set; }
        public string itemImage { get; set; }
        public string airportcode { get; set; }
        public string airportLogo { get; set; }
        public string brandLogo { get; set; }
        public string brandLogoColored { get; set; }
        public string airportURLName { get; set; }
        public string airportName { get; set; }


    }





}