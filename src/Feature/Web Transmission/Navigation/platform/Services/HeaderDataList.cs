using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.BAU.Transmission.Feature.Navigation.Platform.Models;
using Sitecore.Data.Items;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data;
using Adani.BAU.Transmission.Foundation.Logging.Platform.Repositories;
using Sitecore.Resources.Media;
using Sitecore.Abstractions;
using Sitecore.DependencyInjection;
using Sitecore.Data.Fields;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Services
{
    public class HeaderDataList : IHeaderDataList
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        public HeaderDataList(ILogRepository logRepository,IHelper helper)
        {

            this._logRepository = logRepository;
            this._helper = helper;
        }

        public HeaderData GetHeaderData(Item datasource)
        {
            HeaderData listHeaderData = new HeaderData();
            HeaderDetails headerDetails = new HeaderDetails();
            HamburgerMenu hamburgerMenu = new HamburgerMenu();
            List<HeaderDetails> headDeatilslist = new List<HeaderDetails>();
            List<TopNavigation> topNavigationlist = new List<TopNavigation>();
            List<HamburgerMenu> hamburgerMenulist = new List<HamburgerMenu>();
            List<PrimaryHeaderMenu> primaryHeaderMenusList = new List<PrimaryHeaderMenu>();
            List<SearchSuggester> searchSuggestersList = new List<SearchSuggester>();
            List<UserAccount> userAccountslist = new List<UserAccount>();
            List<MyAccounts> myAccountslist = new List<MyAccounts>();
            List<HeaderLogoDropdown> headerLogoDropdowns = new List<HeaderLogoDropdown>();
            List<AirportList> airportLists = new List<AirportList>();
            List<HeaderLogos> headerLogosLists = new List<HeaderLogos>();

            try
            {

                IEnumerable<Item> listItems = datasource.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkFolderTemplateID).ToList();
                if (listItems != null && listItems.Count() > 0)
                {
                    foreach (Sitecore.Data.Items.Item item in listItems)
                    {
                        string branchID = item.Name;
                        if (branchID == Constant.TopNavigationID)
                        {

                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {

                                foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                {

                                    try
                                    {
                                        TopNavigation topNav = new TopNavigation();
                                        string childlinkname = childrenlink.Name;
                                        topNav.headerText = !string.IsNullOrEmpty(childrenlink.Fields[Constant.Link_Title].Value) ? childrenlink.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(childrenlink.Fields[Constant.Link_Url]);
                                        topNav.headerLink = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                        topNav.headerLeftIcon = childrenlink.Fields[Constant.Link_leftIcon].Value;
                                        topNav.headerRightIcon = childrenlink.Fields[Constant.Link_rightIcon].Value;
                                        if (childrenlink.Children?.Count > 0)
                                        {

                                            IEnumerable<Item> ChildlinkItemsL2 = childrenlink.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                            if (ChildlinkItemsL2 != null && ChildlinkItemsL2.Count() > 0)
                                            {
                                                List<ChildNavigation> toNavChildList = new List<ChildNavigation>();
                                                foreach (Sitecore.Data.Items.Item ChildlinkItemsL3 in ChildlinkItemsL2)
                                                {
                                                    ChildNavigation topNavchild = new ChildNavigation();
                                                    topNavchild.itemText = !string.IsNullOrEmpty(ChildlinkItemsL3.Fields[Constant.Link_Title].Value) ? ChildlinkItemsL3.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(ChildlinkItemsL3.Fields[Constant.Link_Url]);
                                                    topNavchild.itemLink = _helper.LinkUrl(ChildlinkItemsL3.Fields[Constant.Link_Url]);
                                                    topNavchild.itemLeftIcon = ChildlinkItemsL3.Fields[Constant.Link_leftIcon].Value;
                                                    topNavchild.itemRightIcon = ChildlinkItemsL3.Fields[Constant.Link_rightIcon].Value;
                                                    toNavChildList.Add(topNavchild);

                                                }
                                                topNav.items = toNavChildList;

                                            }
                                        }

                                        topNavigationlist.Add(topNav);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logRepository.Error("Top Navigation gives -> " + ex.Message);
                                    }

                                }

                                headerDetails.TopNavigation = topNavigationlist;

                            }
                        }
                        else if (branchID == Constant.HamburgerMenuID)
                        {

                            IEnumerable<Item> ChildbranchItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildbranchItems != null && ChildbranchItems.Count() > 0)
                            {
                                List<AdaniAirport> hamburgerMenuInformation = new List<AdaniAirport>();
                                List<Others> hamburgerMenuOthers = new List<Others>();
                                // List<AdaniElectricity> hamburgerMenuAdaniElectricity = new List<AdaniElectricity>();
                                List<AdaniBusinesses> hamburgerMenuAdaniBusinesses = new List<AdaniBusinesses>();
                                foreach (Sitecore.Data.Items.Item children in ChildbranchItems)
                                {

                                    string childbranchname = children.Name;
                                    if (childbranchname == Constant.AdaniAirportID)
                                    {
                                        AdaniAirport info = new AdaniAirport();
                                        info.headerText = !string.IsNullOrEmpty(children.Fields[Constant.Link_Title].Value) ? children.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(children.Fields[Constant.Link_Url]);
                                        info.headerLink = _helper.LinkUrl(children.Fields[Constant.Link_Url]);
                                        info.headerLeftIcon = children.Fields[Constant.Link_leftIcon].Value;
                                        info.headerRightIcon = children.Fields[Constant.Link_rightIcon].Value;

                                        IEnumerable<Item> ChildlinkItems = children.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                        if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                                        {
                                            List<ChildNavigation> toNavChildList = new List<ChildNavigation>();
                                            foreach (Item Informationitem in ChildlinkItems)
                                            {

                                                ChildNavigation infochild = new ChildNavigation();
                                                infochild.itemText = !string.IsNullOrEmpty(Informationitem.Fields[Constant.Link_Title].Value) ? Informationitem.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(Informationitem.Fields[Constant.Link_Url]);
                                                infochild.itemLink = _helper.LinkUrl(Informationitem.Fields[Constant.Link_Url]);
                                                infochild.itemLeftIcon = Informationitem.Fields[Constant.Link_leftIcon].Value;
                                                infochild.itemRightIcon = Informationitem.Fields[Constant.Link_rightIcon].Value;
                                                if (Informationitem.Children?.Count > 0)
                                                {
                                                    IEnumerable<Item> ChildlinkItemsL3 = Informationitem.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                                    if (ChildlinkItemsL3 != null && ChildlinkItemsL3.Count() > 0)
                                                    {
                                                        List<ChildNavigation> toNavChildList1 = new List<ChildNavigation>();
                                                        foreach (Sitecore.Data.Items.Item InformationitemL3 in ChildlinkItemsL3)
                                                        {
                                                            ChildNavigation topNavchild1 = new ChildNavigation();
                                                            topNavchild1.itemText = !string.IsNullOrEmpty(InformationitemL3.Fields[Constant.Link_Title].Value) ? InformationitemL3.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(InformationitemL3.Fields[Constant.Link_Url]);
                                                            topNavchild1.itemLink = _helper.LinkUrl(InformationitemL3.Fields[Constant.Link_Url]);
                                                            topNavchild1.itemLeftIcon = InformationitemL3.Fields[Constant.Link_leftIcon].Value;
                                                            topNavchild1.itemRightIcon = InformationitemL3.Fields[Constant.Link_rightIcon].Value;
                                                            toNavChildList1.Add(topNavchild1);
                                                        }
                                                        infochild.collapseItems = toNavChildList1;

                                                    }


                                                }
                                                toNavChildList.Add(infochild);


                                            }
                                            info.items = toNavChildList;
                                            hamburgerMenuInformation.Add(info);
                                            hamburgerMenu.AdaniAirport = hamburgerMenuInformation;

                                        }

                                    }
                                    else if (childbranchname == Constant.OtherID)
                                    {
                                        Others others = new Others();
                                        others.headerText = !string.IsNullOrEmpty(children.Fields[Constant.Link_Title].Value) ? children.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(children.Fields[Constant.Link_Url]);
                                        others.headerLink = _helper.LinkUrl(children.Fields[Constant.Link_Url]);
                                        others.headerLeftIcon = children.Fields[Constant.Link_leftIcon].Value;
                                        others.headerRightIcon = children.Fields[Constant.Link_rightIcon].Value;

                                        IEnumerable<Item> ChildlinkItems = children.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                        if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                                        {
                                            List<ChildNavigation> toNavChildList = new List<ChildNavigation>();
                                            foreach (Item Childlinkitem1 in ChildlinkItems)
                                            {
                                                ChildNavigation infochild = new ChildNavigation();
                                                infochild.itemText = !string.IsNullOrEmpty(Childlinkitem1.Fields[Constant.Link_Title].Value) ? Childlinkitem1.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(Childlinkitem1.Fields[Constant.Link_Url]);
                                                infochild.itemLink = _helper.LinkUrl(Childlinkitem1.Fields[Constant.Link_Url]);
                                                infochild.itemLeftIcon = Childlinkitem1.Fields[Constant.Link_leftIcon].Value;
                                                infochild.itemRightIcon = Childlinkitem1.Fields[Constant.Link_rightIcon].Value;
                                                toNavChildList.Add(infochild);


                                            }
                                            others.items = toNavChildList;
                                            hamburgerMenuOthers.Add(others);
                                            hamburgerMenu.Others = hamburgerMenuOthers;

                                        }


                                    }

                                    else if (childbranchname == Constant.AdaniBusinessesID)
                                    {
                                        AdaniBusinesses adaniBusinesses = new AdaniBusinesses();
                                        adaniBusinesses.headerText = !string.IsNullOrEmpty(children.Fields[Constant.Link_Title].Value) ? children.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(children.Fields[Constant.Link_Url]);
                                        adaniBusinesses.headerLink = _helper.LinkUrl(children.Fields[Constant.Link_Url]);
                                        adaniBusinesses.headerLeftIcon = children.Fields[Constant.Link_leftIcon].Value;
                                        adaniBusinesses.headerRightIcon = children.Fields[Constant.Link_rightIcon].Value;
                                        IEnumerable<Item> ChildlinkItems = children.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                        if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                                        {
                                            List<ChildNavigation> toNavChildList = new List<ChildNavigation>();
                                            foreach (Item Childlinkitem1 in ChildlinkItems)
                                            {
                                                ChildNavigation infochild = new ChildNavigation();
                                                infochild.itemText = !string.IsNullOrEmpty(Childlinkitem1.Fields[Constant.Link_Title].Value) ? Childlinkitem1.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(Childlinkitem1.Fields[Constant.Link_Url]);
                                                infochild.itemLink = _helper.LinkUrl(Childlinkitem1.Fields[Constant.Link_Url]);
                                                infochild.itemLeftIcon = Childlinkitem1.Fields[Constant.Link_leftIcon].Value;
                                                infochild.itemRightIcon = Childlinkitem1.Fields[Constant.Link_rightIcon].Value;
                                                toNavChildList.Add(infochild);


                                            }
                                            adaniBusinesses.collapseItems = toNavChildList;
                                            hamburgerMenuAdaniBusinesses.Add(adaniBusinesses);
                                            hamburgerMenu.AdaniBusinesses = hamburgerMenuAdaniBusinesses;

                                        }

                                    }


                                }
                                hamburgerMenulist.Add(hamburgerMenu);
                                headerDetails.HamburgerMenu = hamburgerMenulist;
                            }

                        }

                        else if (branchID == Constant.PrimaryHeaderMenuID)
                        {

                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {

                                foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                {
                                    PrimaryHeaderMenu topNav = new PrimaryHeaderMenu();
                                    string childlinkname = childrenlink.Name;
                                    topNav.headerText = !string.IsNullOrEmpty(childrenlink.Fields[Constant.Link_Title].Value) ? childrenlink.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(childrenlink.Fields[Constant.Link_Url]);
                                    topNav.headerLink = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                    topNav.headerLeftIcon = childrenlink.Fields[Constant.Link_leftIcon].Value;
                                    topNav.headerRightIcon = childrenlink.Fields[Constant.Link_rightIcon].Value;
                                    if (childrenlink.Children?.Count > 0)
                                    {

                                        IEnumerable<Item> ChildlinkItemsL2 = childrenlink.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                        if (ChildlinkItemsL2 != null && ChildlinkItemsL2.Count() > 0)
                                        {

                                            List<ChildNavigation> toNavChildList = new List<ChildNavigation>();
                                            foreach (Sitecore.Data.Items.Item ChildlinkitemL2 in ChildlinkItemsL2)
                                            {
                                                ChildNavigation topNavchild = new ChildNavigation();
                                                topNavchild.itemText = !string.IsNullOrEmpty(ChildlinkitemL2.Fields[Constant.Link_Title].Value) ? ChildlinkitemL2.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(ChildlinkitemL2.Fields[Constant.Link_Url]);
                                                topNavchild.itemLink = _helper.LinkUrl(ChildlinkitemL2.Fields[Constant.Link_Url]);
                                                topNavchild.itemLeftIcon = ChildlinkitemL2.Fields[Constant.Link_leftIcon].Value;
                                                topNavchild.itemRightIcon = ChildlinkitemL2.Fields[Constant.Link_rightIcon].Value;
                                                if (ChildlinkitemL2.Children?.Count > 0)
                                                {
                                                    IEnumerable<Item> ChildlinkItemsL3 = ChildlinkitemL2.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                                    if (ChildlinkItemsL3 != null && ChildlinkItemsL3.Count() > 0)
                                                    {
                                                        List<ChildNavigation> toNavChildList1 = new List<ChildNavigation>();
                                                        foreach (Sitecore.Data.Items.Item ChildlinkitemL3 in ChildlinkItemsL3)
                                                        {
                                                            ChildNavigation topNavchild1 = new ChildNavigation();
                                                            topNavchild1.itemText = !string.IsNullOrEmpty(ChildlinkitemL3.Fields[Constant.Link_Title].Value) ? ChildlinkitemL3.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(ChildlinkitemL3.Fields[Constant.Link_Url]);
                                                            topNavchild1.itemLink = _helper.LinkUrl(ChildlinkitemL3.Fields[Constant.Link_Url]);
                                                            topNavchild1.itemLeftIcon = ChildlinkitemL3.Fields[Constant.Link_leftIcon].Value;
                                                            topNavchild1.itemRightIcon = ChildlinkitemL3.Fields[Constant.Link_rightIcon].Value;
                                                            toNavChildList1.Add(topNavchild1);
                                                        }
                                                        topNavchild.collapseItems = toNavChildList1;

                                                    }


                                                }
                                                toNavChildList.Add(topNavchild);

                                            }
                                            topNav.items = toNavChildList;

                                        }
                                    }

                                    primaryHeaderMenusList.Add(topNav);
                                }

                                headerDetails.PrimaryHeaderMenus = primaryHeaderMenusList;

                            }
                        }

                        else if (branchID == Constant.SearchSuggesterID)
                        {

                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {

                                foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                {
                                    SearchSuggester topNav = new SearchSuggester();
                                    string childlinkname = childrenlink.Name;
                                    topNav.headerText = !string.IsNullOrEmpty(childrenlink.Fields[Constant.Link_Title].Value) ? childrenlink.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(childrenlink.Fields[Constant.Link_Url]);
                                    topNav.headerLink = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                    topNav.headerLeftIcon = childrenlink.Fields[Constant.Link_leftIcon].Value;
                                    topNav.headerRightIcon = childrenlink.Fields[Constant.Link_rightIcon].Value;
                                    if (childrenlink.Children?.Count > 0)
                                    {

                                        IEnumerable<Item> ChildlinkItemsL2 = childrenlink.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                        if (ChildlinkItemsL2 != null && ChildlinkItemsL2.Count() > 0)
                                        {
                                            List<ChildNavigation> toNavChildList = new List<ChildNavigation>();
                                            foreach (Sitecore.Data.Items.Item ChildlinkitemL2 in ChildlinkItemsL2)
                                            {

                                                ChildNavigation topNavchild = new ChildNavigation();
                                                topNavchild.itemText = !string.IsNullOrEmpty(ChildlinkitemL2.Fields[Constant.Link_Title].Value) ? ChildlinkitemL2.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(ChildlinkitemL2.Fields[Constant.Link_Url]);
                                                topNavchild.itemLink = _helper.LinkUrl(ChildlinkitemL2.Fields[Constant.Link_Url]);
                                                topNavchild.itemLeftIcon = ChildlinkitemL2.Fields[Constant.Link_leftIcon].Value;
                                                topNavchild.itemRightIcon = ChildlinkitemL2.Fields[Constant.Link_rightIcon].Value;
                                                if (ChildlinkitemL2.Children?.Count > 0)
                                                {
                                                    IEnumerable<Item> ChildlinkItemsL3 = ChildlinkitemL2.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                                    if (ChildlinkItemsL3 != null && ChildlinkItemsL3.Count() > 0)
                                                    {
                                                        List<ChildNavigation> toNavChildList1 = new List<ChildNavigation>();
                                                        foreach (Sitecore.Data.Items.Item ChildlinkitemL3 in ChildlinkItemsL3)
                                                        {
                                                            ChildNavigation topNavchild1 = new ChildNavigation();
                                                            topNavchild1.itemText = !string.IsNullOrEmpty(ChildlinkitemL3.Fields[Constant.Link_Title].Value) ? ChildlinkitemL3.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(ChildlinkitemL3.Fields[Constant.Link_Url]);
                                                            topNavchild1.itemLink = _helper.LinkUrl(ChildlinkitemL3.Fields[Constant.Link_Url]);
                                                            topNavchild1.itemLeftIcon = ChildlinkitemL3.Fields[Constant.Link_leftIcon].Value;
                                                            topNavchild1.itemRightIcon = ChildlinkitemL3.Fields[Constant.Link_rightIcon].Value;
                                                            toNavChildList1.Add(topNavchild1);
                                                        }
                                                        topNavchild.collapseItems = toNavChildList1;

                                                    }


                                                }
                                                toNavChildList.Add(topNavchild);

                                            }
                                            topNav.items = toNavChildList;

                                        }


                                    }

                                    searchSuggestersList.Add(topNav);

                                }
                                headerDetails.searchSuggesters = searchSuggestersList;
                            }
                        }

                        else if (branchID == Constant.UserAccountID)
                        {

                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {

                                foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                {
                                    UserAccount topNav = new UserAccount();
                                    string childlinkname = childrenlink.Name;
                                    topNav.headerText = !string.IsNullOrEmpty(childrenlink.Fields[Constant.Link_Title].Value) ? childrenlink.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(childrenlink.Fields[Constant.Link_Url]);
                                    topNav.headerLink = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                    topNav.headerLeftIcon = childrenlink.Fields[Constant.Link_leftIcon].Value;
                                    topNav.headerRightIcon = childrenlink.Fields[Constant.Link_rightIcon].Value;
                                    if (childrenlink.Children?.Count > 0)
                                    {

                                        IEnumerable<Item> ChildlinkItemsL2 = childrenlink.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                        if (ChildlinkItemsL2 != null && ChildlinkItemsL2.Count() > 0)
                                        {
                                            List<ChildNavigation> toNavChildList = new List<ChildNavigation>();
                                            foreach (Sitecore.Data.Items.Item ChildlinkitemL2 in ChildlinkItemsL2)
                                            {
                                                ChildNavigation topNavchild = new ChildNavigation();
                                                topNavchild.itemText = !string.IsNullOrEmpty(ChildlinkitemL2.Fields[Constant.Link_Title].Value) ? ChildlinkitemL2.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(ChildlinkitemL2.Fields[Constant.Link_Url]);
                                                topNavchild.itemLink = _helper.LinkUrl(ChildlinkitemL2.Fields[Constant.Link_Url]);
                                                topNavchild.itemLeftIcon = ChildlinkitemL2.Fields[Constant.Link_leftIcon].Value;
                                                topNavchild.itemRightIcon = ChildlinkitemL2.Fields[Constant.Link_rightIcon].Value;
                                                toNavChildList.Add(topNavchild);

                                            }
                                            topNav.items = toNavChildList;

                                        }
                                    }

                                    userAccountslist.Add(topNav);
                                }

                                headerDetails.userAccounts = userAccountslist;

                            }
                        }

                        else if (branchID == Constant.MyAccountsID)
                        {

                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {

                                foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                {
                                    MyAccounts topNav = new MyAccounts();
                                    string childlinkname = childrenlink.Name;
                                    topNav.headerText = !string.IsNullOrEmpty(childrenlink.Fields[Constant.Link_Title].Value) ? childrenlink.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(childrenlink.Fields[Constant.Link_Url]);
                                    topNav.headerLink = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                    topNav.headerLeftIcon = childrenlink.Fields[Constant.Link_leftIcon].Value;
                                    topNav.headerRightIcon = childrenlink.Fields[Constant.Link_rightIcon].Value;
                                    if (childrenlink.Children?.Count > 0)
                                    {

                                        IEnumerable<Item> ChildlinkItemsL2 = childrenlink.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                        if (ChildlinkItemsL2 != null && ChildlinkItemsL2.Count() > 0)
                                        {
                                            List<ChildNavigation> toNavChildList = new List<ChildNavigation>();
                                            foreach (Sitecore.Data.Items.Item ChildlinkitemL2 in ChildlinkItemsL2)
                                            {
                                                ChildNavigation topNavchild = new ChildNavigation();
                                                topNavchild.itemText = !string.IsNullOrEmpty(ChildlinkitemL2.Fields[Constant.Link_Title].Value) ? ChildlinkitemL2.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(ChildlinkitemL2.Fields[Constant.Link_Url]);
                                                topNavchild.itemLink = _helper.LinkUrl(ChildlinkitemL2.Fields[Constant.Link_Url]);
                                                topNavchild.itemLeftIcon = ChildlinkitemL2.Fields[Constant.Link_leftIcon].Value;
                                                topNavchild.itemRightIcon = ChildlinkitemL2.Fields[Constant.Link_rightIcon].Value;
                                                toNavChildList.Add(topNavchild);

                                            }
                                            topNav.items = toNavChildList;

                                        }
                                    }

                                    myAccountslist.Add(topNav);
                                }

                                headerDetails.MyAccounts = myAccountslist;

                            }
                        }
                        else if (branchID == Constant.LogoDropdownID)
                        {

                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {

                                foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)

                                {
                                    HeaderLogoDropdown topNav = new HeaderLogoDropdown();
                                    string childlinkname = childrenlink.Name;
                                    topNav.itemText = !string.IsNullOrEmpty(childrenlink.Fields[Constant.Link_Title].Value) ? childrenlink.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(childrenlink.Fields[Constant.Link_Url]);
                                    topNav.itemLink = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                    topNav.itemLeftIcon = childrenlink.Fields[Constant.Link_leftIcon].Value;
                                    topNav.itemRightIcon = childrenlink.Fields[Constant.Link_rightIcon].Value;
                                    if (childrenlink.Children?.Count > 0)
                                    {

                                        IEnumerable<Item> ChildlinkItemsL2 = childrenlink.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                        if (ChildlinkItemsL2 != null && ChildlinkItemsL2.Count() > 0)
                                        {
                                            List<ChildNavigation> toNavChildList = new List<ChildNavigation>();
                                            foreach (Sitecore.Data.Items.Item childrenitemL2 in ChildlinkItemsL2)
                                            {
                                                ChildNavigation topNavchild = new ChildNavigation();
                                                topNavchild.itemText = !string.IsNullOrEmpty(childrenitemL2.Fields[Constant.Link_Title].Value) ? childrenitemL2.Fields[Constant.Link_Title].Value : _helper.LinkUrlText(childrenitemL2.Fields[Constant.Link_Url]);
                                                topNavchild.itemLink = _helper.LinkUrl(childrenitemL2.Fields[Constant.Link_Url]);
                                                topNavchild.itemLeftIcon = childrenitemL2.Fields[Constant.Link_leftIcon].Value;
                                                topNavchild.itemRightIcon = childrenitemL2.Fields[Constant.Link_rightIcon].Value;
                                                toNavChildList.Add(topNavchild);

                                            }
                                            topNav.items = toNavChildList;

                                        }
                                    }

                                    headerLogoDropdowns.Add(topNav);
                                }

                                headerDetails.HeaderLogoDropdown = headerLogoDropdowns;

                            }
                        }


                    }

                }

                IEnumerable<Item> listItems1 = datasource.Children.Where(x => x.TemplateID == Templates.AirportNavigation.ImageWithLinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                if (listItems1 != null && listItems1.Count() > 0)
                {
                    foreach (Sitecore.Data.Items.Item item in listItems1)
                    {

                        if (item.Name == Constant.AirportListID)
                        {
                            AirportList airportList = new AirportList();
                            airportList.headerText = _helper.LinkUrlText(item.Fields[Constant.Link]);
                            airportList.headerLink = _helper.LinkUrl(item.Fields[Constant.Link]);
                            airportList.headerImage = _helper.GetImageURL(item, Constant.Image);
                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.ImageWithLinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {

                                List<ChildNavigationwithImage> toNavChildList = new List<ChildNavigationwithImage>();
                                foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                {

                                    ChildNavigationwithImage topNavchild = new ChildNavigationwithImage();
                                    topNavchild.itemText = _helper.LinkUrlText(childrenlink.Fields[Constant.Link]);
                                    topNavchild.itemLink = _helper.LinkUrl(childrenlink.Fields[Constant.Link]);
                                    topNavchild.itemImage = _helper.GetImageURL(childrenlink, Constant.Image);
                                    topNavchild.airportcode = !string.IsNullOrEmpty(childrenlink.Fields[Constant.AirportCode].Value.ToString()) ? childrenlink.Fields[Constant.AirportCode].Value.ToString() : ""; 
                                    topNavchild.airportLogo = _helper.GetImageURL(childrenlink, Constant.airportLogo); 
                                    topNavchild.brandLogo = _helper.GetImageURL(childrenlink, Constant.brandLogo); 
                                    topNavchild.brandLogoColored = _helper.GetImageURL(childrenlink, Constant.brandLogoColored); 
                                    topNavchild.airportURLName = !string.IsNullOrEmpty(childrenlink.Fields[Constant.airportURLName].Value.ToString()) ? childrenlink.Fields[Constant.airportURLName].Value.ToString() : ""; ;
                                    toNavChildList.Add(topNavchild);

                                }
                                airportList.items = toNavChildList;
                            }

                            airportLists.Add(airportList);
                            headerDetails.AirportList = airportLists;

                        }

                        else if (item.Name == Constant.HeaderLogoID)
                        {
                            HeaderLogos HeaderLogosList = new HeaderLogos();
                            HeaderLogosList.headerText = _helper.LinkUrlText(item.Fields[Constant.Link]);
                            HeaderLogosList.headerLink = _helper.LinkUrl(item.Fields[Constant.Link]);
                            HeaderLogosList.headerImage = _helper.GetImageURL(item, Constant.Image);
                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.ImageWithLinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {

                                List<ChildNavigationwithImage> toNavChildList = new List<ChildNavigationwithImage>();
                                foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                {

                                    ChildNavigationwithImage topNavchild = new ChildNavigationwithImage();
                                    topNavchild.itemText = _helper.LinkUrlText(childrenlink.Fields[Constant.Link]);
                                    topNavchild.itemLink = _helper.LinkUrl(childrenlink.Fields[Constant.Link]);
                                    topNavchild.itemImage = _helper.GetImageURL(childrenlink, Constant.Image);
                                    topNavchild.airportcode = !string.IsNullOrEmpty(childrenlink.Fields[Constant.AirportCode].Value.ToString()) ? childrenlink.Fields[Constant.AirportCode].Value.ToString() : ""; ;
                                    toNavChildList.Add(topNavchild);

                                }
                                HeaderLogosList.items = toNavChildList;
                            }

                            headerLogosLists.Add(HeaderLogosList);
                            headerDetails.HeaderLogos = headerLogosLists;

                        }
                    }
                }

                headDeatilslist.Add(headerDetails);

                listHeaderData.HeaderDetails = headDeatilslist;
            }
            catch (Exception ex)
            {
                _logRepository.Error("ChildlinkFolder gives -> " + ex.Message);
            }
            return listHeaderData;
        }


    }
}