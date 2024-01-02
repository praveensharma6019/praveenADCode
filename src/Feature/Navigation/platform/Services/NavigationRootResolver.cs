using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using Adani.SuperApp.Realty.Feature.Navigation.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.Diagnostics;
using Sitecore.Extensions;
using Sitecore.IO;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Tasks;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Templates;
using static Sitecore.Shell.UserOptions.HtmlEditor;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Services
{
    public class NavigationRootResolver : INavigationRootResolver
    {
        private readonly ILogRepository _logRepository;
        public NavigationRootResolver(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public Item GetNavigationRoot(Item contextItem)
        {
            if (contextItem == null)
            {
                return null;
            }
            return contextItem.DescendsFrom(Templates.NavigationRoot.Id)
                ? contextItem
                : contextItem.Axes.GetAncestors().LastOrDefault(x => x.DescendsFrom(Templates.NavigationRoot.Id));
        }
        /// <summary>
        /// This method is used for getting children of particular datasource item
        /// </summary>
        /// <param name="contextItem"></param>
        /// <returns></returns>
        public List<Item> GetPropertyDetails(Item contextItem)
        {
            List<Item> items = new List<Item>();
            if (contextItem == null)
            {
                return null;
            }

            if (contextItem.TemplateID == Templates.ResidentialProjects.ResidentialLandingID && contextItem.Children.ToList().Count > 0)
            {
                items = contextItem.Axes.GetDescendants().Where(x => x.TemplateID == Templates.ResidentialProjects.TemplateID).ToList();
            }
            if (contextItem.TemplateID == Templates.CommercialProjects.CommercialLandingID && contextItem.Children.ToList().Count > 0)
            {
                items = contextItem.Axes.GetDescendants().Where(x => x.TemplateID == Templates.CommercialProjects.TemplateID).ToList();
            }
            if (contextItem.TemplateID == Templates.Township.TemplateID && contextItem.Children.ToList().Count > 0)
            {
                items = contextItem.Axes.GetDescendants().Where(x => x.TemplateID == Templates.Township.TemplateID).ToList();
            }
            if (contextItem.TemplateID == Templates.SocialClubs.SocialCLubsID && contextItem.Children.ToList().Count > 0)
            {
                items = contextItem.Axes.GetDescendants().Where(x => x.TemplateID == Templates.SocialClubs.TemplateID).ToList();
            }

            return items;
        }
        public List<Item> GetTestimonialList(Item contextItem)
        {
            List<Item> items = new List<Item>();
            if (contextItem == null)
            {
                return null;
            }
            if (contextItem.TemplateID.ToString() == "{FD9E749E-F8CA-4109-9466-6CC099439D48}" && contextItem.Children.ToList().Count > 0)
            {
                items = contextItem.Axes.GetDescendants().Where(x => x.TemplateID == Templates.TestimonialList.TemplateID).ToList();
            }
            return items;

        }
        public Item AboutAdaniHouse(Item contextItem)
        {
            if (contextItem == null)
            {
                return null;
            }
            return contextItem;
        }
        public MobileMenu MobilemenuList(Rendering rendering)
        {
            MobileMenu mobileMenu = new MobileMenu();
            try
            {

                mobileMenu.data = MobilemenuItem(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" NavigationRootResolver MobilemenuList gives -> " + ex.Message);
            }


            return mobileMenu;
        }
        public List<Object> MobilemenuItem(Rendering rendering)
        {
            List<Object> MobilemenuList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                MobileMenuItem menuItem;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    menuItem = new MobileMenuItem();
                    menuItem.icon = !string.IsNullOrEmpty(item.Fields[MobileMenuTemp.Fields.class1ID].Value.ToString()) ? item.Fields[MobileMenuTemp.Fields.class1ID].Value.ToString() : "";
                    menuItem.iconsrc = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, MobileMenuTemp.Fields.logoFieldName) != null ?
                                    Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, MobileMenuTemp.Fields.logoFieldName) : "";
                    menuItem.gifsrc = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, MobileMenuTemp.Fields.thumbID.ToString()) != null ?
                                    Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, MobileMenuTemp.Fields.thumbID.ToString()) : "";
                    menuItem.alt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, MobileMenuTemp.Fields.thumbID.ToString()) != null ?
                                         Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, MobileMenuTemp.Fields.thumbID.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    menuItem.link = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, MobileMenuTemp.Fields.linkFieldName) != null ?
                                    Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, MobileMenuTemp.Fields.linkFieldName) : "";
                    menuItem.desc = !string.IsNullOrEmpty(item.Fields[MobileMenuTemp.Fields.headingFieldName].Value.ToString()) ? item.Fields[MobileMenuTemp.Fields.headingFieldName].Value.ToString() : "";

                    MobilemenuList.Add(menuItem);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" NavigationRootResolver MobilemenuItem gives -> " + ex.Message);
            }

            return MobilemenuList;
        }
        public HeaderMenuList GetHeaderMenuList(Rendering rendering)
        {
            HeaderMenuList headerMenuMenuList = new HeaderMenuList();
            try
            {
                headerMenuMenuList.hamburgerMenu = GetHeaderMenuItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver GetHeaderMenuMenuList gives -> " + ex.Message);
            }


            return headerMenuMenuList;
        }
        public List<HamburgerItem> GetHeaderMenuItem(Rendering rendering)
        {
            List<HamburgerItem> listofhamburger = new List<HamburgerItem>();
            HamburgerItem MobilemenuList = new HamburgerItem();
            int count = 1;
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
                string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
                var homeItem = Sitecore.Context.Database.GetItem(Templates.HomeItemID);
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                MobilemenuList.logosrc = !string.IsNullOrEmpty(homeItem.Fields[headerlogo.headerLogID].Value) ? Helper.GetImageURL(homeItem, headerlogo.HeaderLogoName) : "";
                MobilemenuList.logoAlt = Helper.GetImageDetails(homeItem, headerlogo.HeaderLogoName) != null ?
                Helper.GetImageDetails(homeItem, headerlogo.HeaderLogoName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                HeaderMenuItem menuItem;
                foreach (Item item in datasource.Children)
                {
                    List<HeaderMenuItem> HeaderMenuList = new List<HeaderMenuItem>();
                    List<AdaniBusinessItem> adaniBusinessList = new List<AdaniBusinessItem>();
                    AdaniBusinessItem adaniBusinessItem = new AdaniBusinessItem();

                    menuItem = new HeaderMenuItem();
                    if (item.TemplateID == HeaderItemTemp.HamburgerMenuLinkTemplateID)
                    {
                        if (item.ID != Templates.HeaderItemTemp.adaniBusinessesItemID)
                        {
                            menuItem.headerText = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? item.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                            menuItem.headerLink = Helper.GetLinkURL(item, HeaderItemTemp.Fields.headerLinkName) != null ?
                                            Helper.GetLinkURL(item, HeaderItemTemp.Fields.headerLinkName) : "";
                            menuItem.headerLeftIcon = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? item.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                            menuItem.headerRightIcon = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? item.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                            if (item.HasChildren)
                            {
                                List<collapseItemslist> dataStoreageObject = new List<collapseItemslist>();
                                foreach (Item subitem in item.Children)
                                {
                                    collapseItemslist innerobject = new collapseItemslist();
                                    if (subitem.TemplateID == HeaderItemTemp.TemplateID)
                                    {

                                        innerobject.itemText = !string.IsNullOrEmpty(subitem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? subitem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                                        innerobject.itemSubText = !string.IsNullOrEmpty(subitem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString()) ? subitem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString() : "";
                                        innerobject.itemLink = Helper.GetLinkURL(subitem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                        Helper.GetLinkURL(subitem, HeaderItemTemp.Fields.headerLinkName) : "";
                                        if (innerobject.itemLink == strSitedomain)
                                        {
                                            innerobject.itemLink = "";
                                        }
                                        innerobject.target = Helper.GetLinkURLTargetSpace(subitem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                        Helper.GetLinkURLTargetSpace(subitem, HeaderItemTemp.Fields.headerLinkName) : "";
                                        innerobject.itemLeftIcon = !string.IsNullOrEmpty(subitem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? subitem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                                        innerobject.itemRightIcon = !string.IsNullOrEmpty(subitem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? subitem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                                        if (subitem.HasChildren)
                                        {
                                            List<collapseItemslist> ineerDataStorage = new List<collapseItemslist>();
                                            foreach (Item innerItems in subitem.Children)
                                            {
                                                collapseItemslist innerItemobject = new collapseItemslist();
                                                if (innerItems.TemplateID == HeaderItemTemp.TemplateID)
                                                {
                                                    innerItemobject.itemText = !string.IsNullOrEmpty(innerItems.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? innerItems.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                                                    innerItemobject.itemSubText = !string.IsNullOrEmpty(innerItems.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString()) ? innerItems.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString() : "";
                                                    innerItemobject.itemLink = Helper.GetLinkURL(innerItems, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                                         Helper.GetLinkURL(innerItems, HeaderItemTemp.Fields.headerLinkName) : "";
                                                    if (innerItemobject.itemLink == strSitedomain)
                                                    {
                                                        innerItemobject.itemLink = "";
                                                    }
                                                    innerItemobject.target = Helper.GetLinkURLTargetSpace(innerItems, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                                         Helper.GetLinkURLTargetSpace(innerItems, HeaderItemTemp.Fields.headerLinkName) : "";
                                                    innerItemobject.itemLeftIcon = !string.IsNullOrEmpty(innerItems.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? innerItems.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                                                    innerItemobject.itemRightIcon = !string.IsNullOrEmpty(innerItems.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? innerItems.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                                                    if (innerItems.HasChildren)
                                                    {
                                                        List<collapseItemslist> nestedDataStorage = new List<collapseItemslist>();
                                                        foreach (Item nestedItem in innerItems.Children)
                                                        {
                                                            collapseItemslist nestedItemobject = new collapseItemslist();
                                                            if (nestedItem.TemplateID == HeaderItemTemp.TemplateID)
                                                            {
                                                                nestedItemobject.itemText = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? nestedItem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                                                                nestedItemobject.itemSubText = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString()) ? nestedItem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString() : "";
                                                                nestedItemobject.itemLink = Helper.GetLinkURL(nestedItem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                                                     Helper.GetLinkURL(nestedItem, HeaderItemTemp.Fields.headerLinkName) : "";
                                                                if (nestedItemobject.itemLink == strSitedomain)
                                                                {
                                                                    nestedItemobject.itemLink = "";
                                                                }
                                                                nestedItemobject.target = Helper.GetLinkURLTargetSpace(nestedItem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                                                     Helper.GetLinkURLTargetSpace(nestedItem, HeaderItemTemp.Fields.headerLinkName) : "";
                                                                nestedItemobject.itemLeftIcon = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? nestedItem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                                                                nestedItemobject.itemRightIcon = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? nestedItem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                                                                nestedItemobject.itemImage = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headerLinkID].Value) ? Helper.GetImageURL(nestedItem, HeaderItemTemp.Fields.headerImageName) : "";
                                                                nestedItemobject.linkHeading = nestedItem.Fields[HeaderItemTemp.Fields.linkHeading] != null ? Helper.GetCheckBoxSelection(nestedItem.Fields[HeaderItemTemp.Fields.linkHeading]) : false;
                                                                nestedDataStorage.Add(nestedItemobject);
                                                            }
                                                        }
                                                        innerItemobject.collapseItems = nestedDataStorage;
                                                    }
                                                    ineerDataStorage.Add(innerItemobject);
                                                }
                                            }
                                            innerobject.collapseItems = ineerDataStorage;
                                        }
                                        dataStoreageObject.Add(innerobject);
                                    }
                                    menuItem.items = dataStoreageObject;
                                }
                            }
                            HeaderMenuList.Add(menuItem);
                        }
                        else
                        {
                            adaniBusinessItem.headerText = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? item.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                            adaniBusinessItem.headerLink = Helper.GetLinkURL(item, HeaderItemTemp.Fields.headerLinkName) != null ?
                                            Helper.GetLinkURL(item, HeaderItemTemp.Fields.headerLinkName) : "";
                            adaniBusinessItem.headerLeftIcon = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? item.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                            adaniBusinessItem.headerRightIcon = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? item.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                            if (item.HasChildren)
                            {
                                List<collapseItemslist> dataStoreageObject = new List<collapseItemslist>();
                                foreach (Item subitem in item.Children)
                                {
                                    collapseItemslist innerobject = new collapseItemslist();
                                    if (subitem.TemplateID == HeaderItemTemp.TemplateID)
                                    {
                                        innerobject.itemText = !string.IsNullOrEmpty(subitem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? subitem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                                        innerobject.itemSubText = !string.IsNullOrEmpty(subitem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString()) ? subitem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString() : "";
                                        innerobject.itemLink = Helper.GetLinkURL(subitem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                        Helper.GetLinkURL(subitem, HeaderItemTemp.Fields.headerLinkName) : "";
                                        if (innerobject.itemLink == strSitedomain)
                                        {
                                            innerobject.itemLink = "";
                                        }
                                        innerobject.target = Helper.GetLinkURLTargetSpace(subitem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                        Helper.GetLinkURLTargetSpace(subitem, HeaderItemTemp.Fields.headerLinkName) : "";
                                        innerobject.itemLeftIcon = !string.IsNullOrEmpty(subitem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? subitem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                                        innerobject.itemRightIcon = !string.IsNullOrEmpty(subitem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? subitem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                                        if (subitem.HasChildren)
                                        {
                                            List<collapseItemslist> ineerDataStorage = new List<collapseItemslist>();
                                            foreach (Item innerItems in subitem.Children)
                                            {
                                                collapseItemslist innerItemobject = new collapseItemslist();
                                                if (innerItems.TemplateID == HeaderItemTemp.TemplateID)
                                                {
                                                    innerItemobject.itemText = !string.IsNullOrEmpty(innerItems.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? innerItems.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                                                    innerItemobject.itemSubText = !string.IsNullOrEmpty(innerItems.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString()) ? innerItems.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString() : "";
                                                    innerItemobject.itemLink = Helper.GetLinkURL(innerItems, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                                         Helper.GetLinkURL(innerItems, HeaderItemTemp.Fields.headerLinkName) : "";
                                                    innerItemobject.itemLeftIcon = !string.IsNullOrEmpty(innerItems.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? innerItems.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                                                    innerItemobject.itemRightIcon = !string.IsNullOrEmpty(innerItems.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? innerItems.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                                                    if (innerItems.HasChildren)
                                                    {
                                                        List<collapseItemslist> nestedDataStorage = new List<collapseItemslist>();
                                                        foreach (Item nestedItem in innerItems.Children)
                                                        {
                                                            collapseItemslist nestedItemobject = new collapseItemslist();
                                                            if (nestedItem.TemplateID == HeaderItemTemp.TemplateID)
                                                            {
                                                                nestedItemobject.itemText = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? nestedItem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                                                                nestedItemobject.itemSubText = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString()) ? nestedItem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString() : "";
                                                                nestedItemobject.itemLink = Helper.GetLinkURL(nestedItem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                                                     Helper.GetLinkURL(nestedItem, HeaderItemTemp.Fields.headerLinkName) : "";
                                                                nestedItemobject.itemLeftIcon = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? nestedItem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                                                                nestedItemobject.itemRightIcon = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? nestedItem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                                                                nestedItemobject.itemImage = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headerLinkID].Value) ? Helper.GetImageURL(nestedItem, HeaderItemTemp.Fields.headerImageName) : "";
                                                                nestedDataStorage.Add(nestedItemobject);
                                                            }
                                                        }
                                                        innerItemobject.collapseItems = nestedDataStorage;
                                                    }
                                                    ineerDataStorage.Add(innerItemobject);
                                                }
                                            }
                                            innerobject.collapseItems = ineerDataStorage;
                                        }
                                        dataStoreageObject.Add(innerobject);
                                    }
                                    adaniBusinessItem.collapseItems = dataStoreageObject;
                                }
                            }
                            adaniBusinessList.Add(adaniBusinessItem);
                        }
                        switch (count)
                        {
                            case 1:
                                MobilemenuList.adaniRealty = HeaderMenuList;
                                break;
                            case 2:
                                MobilemenuList.information = HeaderMenuList;
                                break;
                            case 3:
                                MobilemenuList.helpandsupport = HeaderMenuList;
                                break;
                            case 4:
                                MobilemenuList.adaniBusinesses = adaniBusinessList;
                                break;
                            //case 5:
                            //    MobilemenuList.others = HeaderMenuList;
                            //    break;
                            default:
                                MobilemenuList = null;
                                Sitecore.Diagnostics.Error.LogError("datasource is null at NavigationRootResolver GetHeaderMenuItem line no 261");
                                break;
                        }
                        count = count + 1;
                    }
                }
                listofhamburger.Add(MobilemenuList);
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver GetHeaderMenuItem gives -> " + ex.Message);
            }

            return listofhamburger;
        }
        public NewHeaderMenuList GetprimaryHeaderMenusList(Rendering rendering)
        {
            NewHeaderMenuList headerMenuMenuList = new NewHeaderMenuList();
            try
            {
                headerMenuMenuList.data = GetprimaryHeaderMenusItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver GetprimaryHeaderMenusList gives -> " + ex.Message);
            }
            return headerMenuMenuList;
        }
        public List<Object> GetprimaryHeaderMenusItem(Rendering rendering)
        {
            List<Object> mainPrimaryHeaderMenusList = new List<Object>();
            List<Object> primaryHeaderMenusList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                NewHeaderMenuItem newHeaderMenuItem;
                foreach (Item item in datasource.Children)
                {
                    newHeaderMenuItem = new NewHeaderMenuItem();
                    if (item.TemplateID == HeaderItemTemp.HamburgerMenuLinkTemplateID)
                    {
                        var fatNavBar = false;
                        newHeaderMenuItem.headerText = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? item.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                        newHeaderMenuItem.headerLink = Helper.GetLinkURL(item, HeaderItemTemp.Fields.headerLinkName) != null ?
                                        Helper.GetLinkURL(item, HeaderItemTemp.Fields.headerLinkName) : "";
                        newHeaderMenuItem.headerLeftIcon = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? item.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                        newHeaderMenuItem.headerRightIcon = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? item.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                        newHeaderMenuItem.defaultImage = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.itemImage].Value) ? Helper.GetImageURLbyField(item.Fields[HeaderItemTemp.Fields.itemImage]) : "";
                        newHeaderMenuItem.defaultLogo = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.itemLogo].Value) ? Helper.GetImageURLbyField(item.Fields[HeaderItemTemp.Fields.itemLogo]) : "";
                        Sitecore.Data.Fields.CheckboxField fatNav = item.Fields[HeaderItemTemp.Fields.fatnavChechboxID];
                        Sitecore.Data.Fields.CheckboxField headerCallback = item.Fields[HeaderItemTemp.Fields.headerCallbackID];

                        if (fatNav != null && fatNav.Checked)
                        {
                            newHeaderMenuItem.fatNav = true;
                        }
                        else
                        {
                            newHeaderMenuItem.fatNav = false;
                        }
                        if (headerCallback != null && headerCallback.Checked)
                        {
                            newHeaderMenuItem.headerCallback = true;
                        }
                        else
                        {
                            newHeaderMenuItem.headerCallback = false;
                        }
                        if (item.HasChildren)
                        {
                            foreach (var childItem in item.Children.ToList())
                            {
                                List<NewcollapseItemslist> listofnewcollapseItems = new List<NewcollapseItemslist>();
                                if (childItem.TemplateID == HeaderItemTemp.TemplateID)
                                {
                                    NewcollapseItemslist childncollapesItem = new NewcollapseItemslist();
                                    childncollapesItem.fatNavSectionLink = childItem.Name;
                                    if (childItem.TemplateID == HeaderItemTemp.TemplateID)
                                    {
                                        NewcollapseItemslist newcollapseItemslist = new NewcollapseItemslist();
                                        if (childItem.HasChildren)
                                        {
                                            List<SectionPromoImages> listOfsectionPromoImages = new List<SectionPromoImages>();
                                            List<InnerSection> InnerSectionList = new List<InnerSection>();
                                            foreach (var nestedchild in childItem.Children.ToList())
                                            {
                                                newcollapseItemslist.sectionTitle = nestedchild.Name;
                                                if (nestedchild.TemplateID == HeaderItemTemp.TemplateID)
                                                {
                                                    List<sampaleTemplate> sampaleTemplateslist = new List<sampaleTemplate>();
                                                    InnerSection nestedcollapseItemslist = new InnerSection();

                                                    if (nestedchild.HasChildren)
                                                    {
                                                        nestedcollapseItemslist.sectionTitle = nestedchild.Name;
                                                        nestedcollapseItemslist.sectionTitleLink = Helper.GetLinkURL(nestedchild, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                                Helper.GetLinkURL(nestedchild, HeaderItemTemp.Fields.headerLinkName) : "";
                                                        if (nestedcollapseItemslist.sectionTitleLink == Helper.GetSitecoreDomain())
                                                        {
                                                            nestedcollapseItemslist.sectionTitleLink = "";
                                                        }
                                                        nestedcollapseItemslist.sectionImage = Helper.GetImageURL(nestedchild, HeaderItemTemp.Fields.itemImage.ToString()) != null ? Helper.GetImageURL(nestedchild, HeaderItemTemp.Fields.itemImage.ToString()) : "";
                                                        nestedcollapseItemslist.sectionLogo = Helper.GetImageURL(nestedchild, HeaderItemTemp.Fields.itemLogo.ToString()) != null ?
                                                               Helper.GetImageURL(nestedchild, HeaderItemTemp.Fields.itemLogo.ToString()) : "";
                                                        foreach (var innerchildItem in nestedchild.Children.ToList())
                                                        {
                                                            if (innerchildItem.TemplateID == HeaderItemTemp.TemplateID)
                                                            {
                                                                sampaleTemplate sampaleTemplate = new sampaleTemplate();
                                                                sampaleTemplate.itemText = !string.IsNullOrEmpty(innerchildItem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? innerchildItem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                                                                sampaleTemplate.itemLink = Helper.GetLinkURL(innerchildItem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                                Helper.GetLinkURL(innerchildItem, HeaderItemTemp.Fields.headerLinkName) : "";
                                                                if (sampaleTemplate.itemLink == Helper.GetSitecoreDomain())
                                                                {
                                                                    sampaleTemplate.itemLink = "";
                                                                }
                                                                sampaleTemplate.target = Helper.GetLinkURLTargetSpace(innerchildItem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                                Helper.GetLinkURLTargetSpace(innerchildItem, HeaderItemTemp.Fields.headerLinkName) : "";
                                                                sampaleTemplate.itemImage = !string.IsNullOrEmpty(innerchildItem.Fields[HeaderItemTemp.Fields.itemImage].Value) ? Helper.GetImageURLbyField(innerchildItem.Fields[HeaderItemTemp.Fields.itemImage]) : "";
                                                                sampaleTemplate.itemLogo = !string.IsNullOrEmpty(innerchildItem.Fields[HeaderItemTemp.Fields.itemLogo].Value) ? Helper.GetImageURLbyField(innerchildItem.Fields[HeaderItemTemp.Fields.itemLogo]) : "";
                                                                sampaleTemplate.linkHeading = innerchildItem.Fields[HeaderItemTemp.Fields.linkHeading] != null ? Helper.GetCheckBoxSelection(innerchildItem.Fields[HeaderItemTemp.Fields.linkHeading]) : false;
                                                                sampaleTemplateslist.Add(sampaleTemplate);
                                                            }
                                                            nestedcollapseItemslist.sectionItems = sampaleTemplateslist;
                                                        }
                                                        InnerSectionList.Add(nestedcollapseItemslist);
                                                    }
                                                    childncollapesItem.items = InnerSectionList;
                                                }
                                                else if (nestedchild.TemplateID == HeaderItemTemp.SectionPromoImagesTemplateID)
                                                {
                                                    SectionPromoImages sectionPromoImages = new SectionPromoImages();
                                                    sectionPromoImages.promoImage = !string.IsNullOrEmpty(nestedchild.Fields[HeaderItemTemp.SectionPromoImmagesFields.PromoImageID].Value) ? Helper.GetImageURL(nestedchild, HeaderItemTemp.SectionPromoImmagesFields.promoImageName) : "";

                                                    sectionPromoImages.promoLogo = !string.IsNullOrEmpty(nestedchild.Fields[HeaderItemTemp.SectionPromoImmagesFields.PromoLogoID].Value) ? Helper.GetImageURL(nestedchild, HeaderItemTemp.SectionPromoImmagesFields.promoLogoName) : "";
                                                    sectionPromoImages.promoLink = Helper.GetLinkURL(nestedchild, HeaderItemTemp.SectionPromoImmagesFields.PromoLinkName) != null ? Helper.GetLinkURL(nestedchild, HeaderItemTemp.SectionPromoImmagesFields.PromoLinkName) : "";
                                                    sectionPromoImages.promoAltText = !string.IsNullOrEmpty(nestedchild.Fields[HeaderItemTemp.SectionPromoImmagesFields.PromoALTtextID].Value.ToString()) ? nestedchild.Fields[HeaderItemTemp.SectionPromoImmagesFields.PromoALTtextID].Value.ToString() : "";
                                                    listOfsectionPromoImages.Add(sectionPromoImages);
                                                    childncollapesItem.sectionPromoImages = listOfsectionPromoImages;

                                                }
                                            }
                                            newHeaderMenuItem.items.Add(childncollapesItem);
                                        }
                                        else
                                        {
                                            newcollapseItemslist.itemText = !string.IsNullOrEmpty(childItem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? childItem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                                            newcollapseItemslist.itemSubText = !string.IsNullOrEmpty(childItem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString()) ? childItem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString() : "";
                                            newcollapseItemslist.itemLeftIcon = !string.IsNullOrEmpty(childItem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? childItem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                                            newcollapseItemslist.itemRightIcon = !string.IsNullOrEmpty(childItem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? childItem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                                            newcollapseItemslist.itemLink = Helper.GetLinkURL(childItem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                Helper.GetLinkURL(childItem, HeaderItemTemp.Fields.headerLinkName) : "";
                                            newHeaderMenuItem.items.Add(newcollapseItemslist);
                                        }
                                    }
                                }
                            }
                            primaryHeaderMenusList.Add(newHeaderMenuItem);
                        }
                        else
                        {
                            newHeaderMenuItem.items = null;
                            primaryHeaderMenusList.Add(newHeaderMenuItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver GetprimaryHeaderMenusItem gives -> " + ex.Message);
            }
            return primaryHeaderMenusList;
        }
        private string GetSlugValue(Item item)
        {
            string value = string.Empty;
            try
            {
                if (item != null)
                {
                    value = item?.Fields[Templates.CommunicationCornerTemp.Fields.SlugText]?.Value?.ToLower();
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return value;
        }
        public List<BreadCrumbModel> GetBreadCrumbData(Rendering rendering)
        {
            List<BreadCrumbModel> breadcrumblist = new List<BreadCrumbModel>();
            bool status = false;
            var catTitle = "";
            var catLink = "";
            var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
            var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString()) ?
                            commonItem.Fields[Templates.commonData.Fields.blogLinkId].Value.ToString() : "";
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            string urlParam = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Location");
            string categoryId = Sitecore.Context.Request?.QueryString["catName"];
            urlParam = !string.IsNullOrEmpty(urlParam) ? urlParam.Replace("property-in-", "") : "";
            try
            {
                var currentitem = Sitecore.Context.Item;
                // Null Check for datasource
                if (currentitem == null)
                {
                    throw new NullReferenceException();
                }
                var anchestors = currentitem.Axes.GetAncestors().ToList();
                anchestors = anchestors.SkipWhile(i => i.ID != HomeItemID).ToList();
                if (string.IsNullOrEmpty(urlParam) && string.IsNullOrEmpty(categoryId))
                {
                    var categoryname = "";
                    foreach (var item in anchestors)
                    {
                        //var renderings = item.Visualization.GetRenderings(Sitecore.Context.Device, true);&& renderings.Count() > 0
                        if (!(item.TemplateID == LocalDatasourceFolderTemplateID || item.TemplateID == CommonFolderTemplateID))
                        {
                            BreadCrumbModel breadCrumb = new BreadCrumbModel();
                            breadCrumb.active = status;
                            if (item.TemplateID != LocationlandingID)
                            {
                                breadCrumb.href = string.Format(realtyLink + "{0}", Sitecore.Links.LinkManager.GetItemUrl(item).ToLower());
                                breadCrumb.href = breadCrumb.href.Contains(strSitedomain) ? breadCrumb.href : strSitedomain + breadCrumb.href;
                            }
                            else
                            {
                                breadCrumb.href = !string.IsNullOrEmpty(Helper.GetLinkURL(item, ResidentialProjects.Fields.linkFieldName)) ? Helper.GetLinkURL(item, ResidentialProjects.Fields.linkFieldName) : "";
                                breadCrumb.href = breadCrumb.href.Contains(strSitedomain) ? breadCrumb.href : strSitedomain + breadCrumb.href;
                            }
                            //if (item.TemplateID.ToString() == "{4D88E182-FE14-48F7-A1E7-723F45E59CBE}")
                            //{
                            //    breadCrumb.active = true;
                            //}
                            ItemPropclass inneritemPropclass = new ItemPropclass();
                            inneritemPropclass.itemProp = "item";
                            breadCrumb.linkProps = inneritemPropclass;
                            breadCrumb.label = item?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value == "" ? item.Name : item?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value;
                            breadcrumblist.Add(breadCrumb);
                        }
                    }
                    if (currentitem.TemplateID == CommunicationCornerTemp.TemplateID)
                    {
                        var multiListForItem = currentitem.GetMultiListValueItem(CommunicationCornerTemp.Fields.category);
                        if (multiListForItem != null)
                        {
                            foreach (var selectedCatagory in multiListForItem)
                            {
                                if (selectedCatagory.TemplateID == CommunicationCornerTemp.BlogAnchorsID)
                                {
                                    catTitle = /*!string.IsNullOrEmpty(selectedCatagory.Fields[CommunicationCornerTemp.Fields.CatagorytitleID].Value.ToString()) ? selectedCatagory.Fields[CommunicationCornerTemp.Fields.CatagorytitleID].Value : null;*/ selectedCatagory.Name;
                                    catLink = string.Format(realtyLink + "/blogs/category/{0}", GetSlugValue(selectedCatagory));
                                    categoryname = selectedCatagory?.Fields[CommunicationCornerTemp.Fields.CatagorytitleID] != null ? selectedCatagory?.Fields[CommunicationCornerTemp.Fields.CatagorytitleID].Value : selectedCatagory.Name;
                                    break;
                                }
                            }
                            status = false;
                            BreadCrumbModel blogCatObj = new BreadCrumbModel();
                            blogCatObj.active = status;
                            blogCatObj.href = strSitedomain + catLink;
                            ItemPropclass inneritemPropclass = new ItemPropclass();
                            inneritemPropclass.itemProp = "item";
                            blogCatObj.linkProps = inneritemPropclass;
                            blogCatObj.label = categoryname;
                            breadcrumblist.Add(blogCatObj);
                        }
                    }
                    status = true;
                    BreadCrumbModel itemBreadCrumb = new BreadCrumbModel();
                    itemBreadCrumb.active = status;
                    itemBreadCrumb.href = string.Format(strSitedomain + realtyLink + "{0}", Sitecore.Links.LinkManager.GetItemUrl(currentitem).ToLower());
                    itemBreadCrumb.href = itemBreadCrumb.href.Contains(strSitedomain) ? itemBreadCrumb.href : strSitedomain + itemBreadCrumb.href;
                    ItemPropclass itemPropclass = new ItemPropclass();
                    itemPropclass.itemProp = "item";
                    itemBreadCrumb.linkProps = itemPropclass;
                    itemBreadCrumb.label = currentitem?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value == "" ? currentitem.Name : currentitem?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value;
                    breadcrumblist.Add(itemBreadCrumb);
                }
                else if (!string.IsNullOrEmpty(urlParam))
                {
                    foreach (var item in anchestors)
                    {
                        var renderings = item.Visualization.GetRenderings(Sitecore.Context.Device, true);
                        if ((item.TemplateID != LocalDatasourceFolderTemplateID || item.TemplateID != CommonFolderTemplateID) && renderings.Count() > 0)
                        {
                            BreadCrumbModel breadCrumb = new BreadCrumbModel();
                            breadCrumb.active = status;
                            breadCrumb.href = string.Format(realtyLink + "{0}", Sitecore.Links.LinkManager.GetItemUrl(item).ToLower());
                            breadCrumb.href = breadCrumb.href.Contains(strSitedomain) ? breadCrumb.href : strSitedomain + breadCrumb.href;
                            ItemPropclass inneritemPropclass = new ItemPropclass();
                            inneritemPropclass.itemProp = "item";
                            breadCrumb.linkProps = inneritemPropclass;
                            breadCrumb.label = item?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value == "" ? item.Name : item?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value;
                            breadcrumblist.Add(breadCrumb);
                        }
                    }
                    status = true;
                    BreadCrumbModel itemBreadCrumb = new BreadCrumbModel();
                    itemBreadCrumb.active = status;
                    itemBreadCrumb.href = string.Format(strSitedomain + realtyLink + "{0}", Sitecore.Links.LinkManager.GetItemUrl(currentitem).ToLower());
                    itemBreadCrumb.href = itemBreadCrumb.href.Contains(strSitedomain) ? itemBreadCrumb.href : strSitedomain + itemBreadCrumb.href;
                    ItemPropclass itemPropclass = new ItemPropclass();
                    itemPropclass.itemProp = "item";
                    itemBreadCrumb.linkProps = itemPropclass;
                    if (currentitem.ID.ToString() != "{57AFF2A6-2113-4E27-8A0B-8F9E4CEA645C}")
                    {
                        itemBreadCrumb.label = currentitem?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value == "" ? currentitem.Name : currentitem?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value;
                    }
                    else
                    {
                        itemBreadCrumb.label = !string.IsNullOrEmpty(commonItem.Fields[commonData.Fields.LocationPageBreadcrumb].Value) ? commonItem.Fields[commonData.Fields.LocationPageBreadcrumb].Value + char.ToUpper(urlParam[0]) + urlParam.Substring(1) : char.ToUpper(urlParam[0]) + urlParam.Substring(1);
                    }
                    breadcrumblist.Add(itemBreadCrumb);
                }
                else if (!string.IsNullOrEmpty(categoryId))
                {
                    foreach (var item in anchestors)
                    {
                        var renderings = item.Visualization.GetRenderings(Sitecore.Context.Device, true);
                        if ((item.TemplateID != LocalDatasourceFolderTemplateID || item.TemplateID != CommonFolderTemplateID) && renderings.Count() > 0)
                        {
                            BreadCrumbModel breadCrumb = new BreadCrumbModel();
                            breadCrumb.active = status;
                            breadCrumb.href = string.Format(strSitedomain + realtyLink + "{0}", Sitecore.Links.LinkManager.GetItemUrl(item).ToLower());
                            ItemPropclass itemPropclass = new ItemPropclass();
                            itemPropclass.itemProp = "item";
                            breadCrumb.linkProps = itemPropclass;
                            breadCrumb.label = item?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value == "" ? item.Name : item?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value;
                            breadcrumblist.Add(breadCrumb);
                        }
                        if ((item.TemplateID != LocalDatasourceFolderTemplateID || item.TemplateID != CommonFolderTemplateID) && currentitem.ID == BlogCategory.ItemID)
                        {
                            var communicationPage = Sitecore.Context.Database.GetItem(CommunicationCornerTemp.ItemID);
                            BreadCrumbModel blogBreadCrumb = new BreadCrumbModel();
                            blogBreadCrumb.active = status;
                            blogBreadCrumb.href = string.Format(strSitedomain + realtyLink + "{0}", Sitecore.Links.LinkManager.GetItemUrl(communicationPage).ToLower());
                            ItemPropclass itemPropclass = new ItemPropclass();
                            itemPropclass.itemProp = "item";
                            blogBreadCrumb.linkProps = itemPropclass;
                            blogBreadCrumb.label = communicationPage?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value == "" ? communicationPage.Name : communicationPage?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value;
                            breadcrumblist.Add(blogBreadCrumb);
                        }
                    }
                    status = true;
                    var currerntCategory = Sitecore.Context.Database.GetItem(BlogCategory.BlogAnchorsFolder);
                    var categoryNameCheck = currerntCategory.Children.Where(x => x.Name.ToLower().Contains(categoryId)).FirstOrDefault();
                    if (categoryNameCheck != null && !string.IsNullOrEmpty(categoryNameCheck.Name))
                    {
                        BreadCrumbModel itemBreadCrumb = new BreadCrumbModel();
                        itemBreadCrumb.active = status;
                        itemBreadCrumb.href = string.Format(strSitedomain + realtyLink + "{0}", Sitecore.Links.LinkManager.GetItemUrl(categoryNameCheck).ToLower());
                        ItemPropclass itemPropclass = new ItemPropclass();
                        itemPropclass.itemProp = "item";
                        itemBreadCrumb.linkProps = itemPropclass;
                        itemBreadCrumb.label = categoryNameCheck?.Fields[BlogCategory.Fields.Keyword]?.Value == "" ? categoryNameCheck.Name : categoryNameCheck?.Fields[BlogCategory.Fields.Keyword]?.Value;
                        breadcrumblist.Add(itemBreadCrumb);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver GetBreadCrumbData gives -> " + ex.Message);
            }
            return breadcrumblist;
        }
        public List<BreadCrumbModel> GetSeoBreadCrumbData(Rendering rendering)
        {
            var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
            List<BreadCrumbModel> breadcrumblist = new List<BreadCrumbModel>();
            bool status = false;
            var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
            string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            try
            {
                var currentitem = Sitecore.Context.Item;
                // Null Check for datasource
                if (currentitem == null)
                {
                    throw new NullReferenceException();
                }
                System.Collections.Specialized.NameValueCollection BreadCrumCollection = datasource != null ? Sitecore.Web.WebUtil.ParseUrlParameters(datasource[SeoBreadcrum.fields.ParentNodes]) : null;
                foreach (var nv in BreadCrumCollection)
                {
                    BreadCrumbModel breadCrumb = new BreadCrumbModel();
                    breadCrumb.active = status;
                    breadCrumb.href = strSitedomain + BreadCrumCollection[nv.ToString()];
                    ItemPropclass inneritemPropclass = new ItemPropclass();
                    inneritemPropclass.itemProp = "item";
                    breadCrumb.linkProps = inneritemPropclass;
                    breadCrumb.label = nv.ToString();
                    breadCrumb.label = breadCrumb.label.Contains("_") ? breadCrumb.label.Replace("_", " ") : breadCrumb.label;
                    breadcrumblist.Add(breadCrumb);
                }
                status = true;
                BreadCrumbModel itemBreadCrumb = new BreadCrumbModel();
                itemBreadCrumb.active = status;
                itemBreadCrumb.href = Helper.GetLinkURL(datasource, ResidentialProjects.Fields.linkID.ToString()) != null ?
                                             Helper.GetLinkURL(datasource, ResidentialProjects.Fields.linkID.ToString()) : "";
                ItemPropclass itemPropclass = new ItemPropclass();
                itemPropclass.itemProp = "item";
                itemBreadCrumb.linkProps = itemPropclass;
                itemBreadCrumb.label = currentitem?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value == "" ? currentitem.Name : currentitem?.Fields[PageTemplate.PageTemplateFields.BreadCrumbTitle]?.Value;
                breadcrumblist.Add(itemBreadCrumb);
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver GetBreadCrumbData gives -> " + ex.Message);
            }
            return breadcrumblist;
        }
        public EnquireForm GetEnquiryItem(Rendering rendering)
        {
            EnquireForm enquireForm = new EnquireForm();
            var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
            var commonItem = Sitecore.Context.Database.GetItem(commonData.ItemID);
            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException();
            }
            try
            {
                enquireForm.enquireNow = datasource?.Fields[Templates.EnquiryFormTemp.Fields.Title].Value;
                enquireForm.planAVsiit = datasource?.Fields[Templates.EnquiryFormTemp.Fields.planAVsiit].Value;
                enquireForm.ContactUsTitle = datasource?.Fields[Templates.EnquiryFormTemp.Fields.ContactUsTitle].Value;
                enquireForm.PropertyLabel = datasource?.Fields[Templates.EnquiryFormTemp.Fields.PropertyLabel].Value;
                enquireForm.shareContact = datasource?.Fields[Templates.EnquiryFormTemp.Fields.ShareContact].Value;
                enquireForm.sendusQuery = datasource?.Fields[Templates.EnquiryFormTemp.Fields.SendUsQuery].Value;
                enquireForm.firstName = datasource?.Fields[Templates.EnquiryFormTemp.Fields.FirstName].Value;
                enquireForm.lastName = datasource?.Fields[Templates.EnquiryFormTemp.Fields.lastName].Value;
                enquireForm.name = datasource?.Fields[Templates.EnquiryFormTemp.Fields.name].Value;
                enquireForm.email = datasource?.Fields[Templates.EnquiryFormTemp.Fields.Email].Value;
                enquireForm.projectType = datasource?.Fields[Templates.EnquiryFormTemp.Fields.ProjectType].Value;
                enquireForm.popupTitle = datasource?.Fields[Templates.EnquiryFormTemp.Fields.popUpTitle].Value;
                enquireForm.popupSubTitle = datasource?.Fields[Templates.EnquiryFormTemp.Fields.popupSubTitle].Value;
                var projectTypeList = datasource?.GetMultiListValueItem(Templates.EnquiryFormTemp.Fields.ProjectTypeSelection);
                if (projectTypeList != null)
                {
                    List<Dropdown> dropdowns = new List<Dropdown>();
                    foreach (var item in projectTypeList)
                    {
                        Dropdown projectTypedropdown = new Dropdown();
                        projectTypedropdown.Id = item?.ID.ToString();
                        projectTypedropdown.Value = item.TemplateID == ResidentialProjects.ResidentialLandingID ? commonItem.Fields[commonData.Fields.residentialID].Value : item.TemplateID == CommercialProjects.CommercialLandingID ? commonItem.Fields[commonData.Fields.commercialID].Value : commonItem.Fields[commonData.Fields.ClubId].Value;
                        dropdowns.Add(projectTypedropdown);
                    }
                    enquireForm.ProjectTypeOptions = dropdowns;
                }
                enquireForm.agreeToConnect = datasource?.Fields[Templates.EnquiryFormTemp.Fields.AgreeToConnect].Value;
                enquireForm.overrideRegistry = datasource?.Fields[Templates.EnquiryFormTemp.Fields.overrideRegistry].Value;
                enquireForm.submitDetail = datasource?.Fields[Templates.EnquiryFormTemp.Fields.submitDetail].Value;
                enquireForm.selectPropertyType = datasource?.Fields[Templates.EnquiryFormTemp.Fields.SelectProjectProperty].Value;
                var projectPropertyTypeList = datasource?.GetMultiListValueItem(Templates.EnquiryFormTemp.Fields.PropertyTypeSelection);
                if (projectPropertyTypeList != null)
                {
                    List<Dropdown> dropdowns = new List<Dropdown>();
                    foreach (var item in projectPropertyTypeList)
                    {
                        Dropdown projectPropertyType = new Dropdown();
                        projectPropertyType.Id = item?.ID.ToString();
                        projectPropertyType.controllerName = item?.Fields[EnquiryFormTemp.Fields.HeaderTextID].Value;
                        projectPropertyType.Value = item?.DisplayName != "" ? item.DisplayName : item.Name;
                        dropdowns.Add(projectPropertyType);
                    }
                    enquireForm.PropertyTypeOptions = dropdowns;
                }
                enquireForm.StartDate = datasource?.Fields[Templates.EnquiryFormTemp.Fields.startDate].Value;
                enquireForm.homeLoanInterested = datasource?.Fields[Templates.EnquiryFormTemp.Fields.homeLoanInterested].Value;
                enquireForm.selectProjectProperty = datasource?.Fields[Templates.EnquiryFormTemp.Fields.SelectProjectProperty].Value;
                enquireForm.configuration = datasource?.Fields[Templates.EnquiryFormTemp.Fields.configuration].Value;
                enquireForm.timeSlots = datasource?.Fields[Templates.EnquiryFormTemp.Fields.timeSlotsLabel].Value;
                var projectTimeSlot = datasource?.GetMultiListValueItem(Templates.EnquiryFormTemp.Fields.timeSlots);
                if (projectTimeSlot != null)
                {
                    List<Dropdown> dropdowns = new List<Dropdown>();
                    foreach (var item in projectTimeSlot)
                    {
                        Dropdown dropdown = new Dropdown();
                        dropdown.Id = item?.ID.ToString();
                        dropdown.Value = item?.Name;
                        dropdowns.Add(dropdown);
                    }
                    enquireForm.TimeSlotsOptions = dropdowns;
                }
                enquireForm.date = datasource?.Fields[Templates.EnquiryFormTemp.Fields.date].Value;
                enquireForm.from = datasource?.Fields[Templates.EnquiryFormTemp.Fields.from].Value;
                enquireForm.to = datasource?.Fields[Templates.EnquiryFormTemp.Fields.to].Value;
                enquireForm.heading = datasource?.Fields[Templates.EnquiryFormTemp.Fields.heading].Value;
                enquireForm.para = datasource?.Fields[Templates.EnquiryFormTemp.Fields.para].Value;
                enquireForm.cancelLabel = datasource?.Fields[Templates.EnquiryFormTemp.Fields.cancelLabel].Value;
                enquireForm.continueLabel = datasource?.Fields[Templates.EnquiryFormTemp.Fields.continueLabel].Value;
                enquireForm.description = datasource?.Fields[Templates.EnquiryFormTemp.Fields.description].Value;
                enquireForm.PrintLabel = datasource?.Fields[Templates.EnquiryFormTemp.Fields.PrintLabel].Value;
                enquireForm.saveaspdfLabel = datasource?.Fields[Templates.EnquiryFormTemp.Fields.saveaspdfLabel].Value;
                enquireForm.DoneButtonLabel = datasource?.Fields[Templates.EnquiryFormTemp.Fields.DoneButtonLabel].Value;
                enquireForm.MobileLabel = datasource?.Fields[Templates.EnquiryFormTemp.Fields.MobileLabel].Value;
                enquireForm.SubmitButtonText = datasource?.Fields[Templates.EnquiryFormTemp.Fields.SubmitButtonText].Value;
                enquireForm.EnterOTPLabel = datasource?.Fields[Templates.EnquiryFormTemp.Fields.EnterOTPLabel].Value;
                enquireForm.WehavesentviaSMStoLabel = datasource?.Fields[Templates.EnquiryFormTemp.Fields.WehavesentviaSMStoLabel].Value;
                enquireForm.HavenotreceivedaOTPLabel = datasource?.Fields[Templates.EnquiryFormTemp.Fields.HavenotreceivedaOTPLabel].Value;
                enquireForm.editButtonLable = datasource?.Fields[Templates.EnquiryFormTemp.Fields.EditButtonLabel].Value;
                enquireForm.ResendButtonLabel = datasource?.Fields[Templates.EnquiryFormTemp.Fields.ResendButtonLabel].Value;
                enquireForm.PurposeLabel = datasource?.Fields[Templates.EnquiryFormTemp.Fields.purposelabelId].Value;
                enquireForm.errorMessageTitle = datasource?.Fields[Templates.EnquiryFormTemp.Fields.errorMessageTitle].Value;
                enquireForm.BrochureHeading = datasource?.Fields[Templates.EnquiryFormTemp.Fields.BrochureHeadingID].Value;
                enquireForm.BrochureFormDescription = datasource?.Fields[Templates.EnquiryFormTemp.Fields.BrochureFormDescriptionID].Value;
                enquireForm.BrochureThankyouDescription = datasource?.Fields[Templates.EnquiryFormTemp.Fields.BrochureThankyouDescriptionID].Value;
                enquireForm.errorMessageDesription = datasource?.Fields[Templates.EnquiryFormTemp.Fields.errorMessageDesription].Value;
                var purposeSelectionList = datasource?.GetMultiListValueItem(Templates.EnquiryFormTemp.Fields.purposeListID);
                if (purposeSelectionList != null)
                {
                    List<Dropdown> purposes = new List<Dropdown>();
                    foreach (var item in purposeSelectionList)
                    {
                        Dropdown dropdown = new Dropdown();
                        dropdown.Id = item?.ID.ToString();
                        dropdown.Value = item?.Fields[Templates.EnquiryFormTemp.Fields.ITitleFieldID].Value;
                        purposes.Add(dropdown);
                    }
                    enquireForm.PurposeList = purposes;
                }
                ErrorData errorData = new ErrorData();
                errorData.name = datasource?.Fields[Templates.EnquiryFormTemp.Fields.fullname].Value;
                errorData.fname = datasource?.Fields[Templates.EnquiryFormTemp.Fields.fname].Value;
                errorData.lname = datasource?.Fields[Templates.EnquiryFormTemp.Fields.lname].Value;
                errorData.email = datasource?.Fields[Templates.EnquiryFormTemp.Fields.email].Value; ;
                errorData.phoneNo = datasource?.Fields[Templates.EnquiryFormTemp.Fields.phoneNo].Value; ;
                errorData.projectType = datasource?.Fields[Templates.EnquiryFormTemp.Fields.projectType].Value;
                errorData.projectName = datasource?.Fields[Templates.EnquiryFormTemp.Fields.projectName].Value;
                errorData.configuration = datasource?.Fields[Templates.EnquiryFormTemp.Fields.configurationmessage].Value;
                errorData.timeslot = datasource?.Fields[Templates.EnquiryFormTemp.Fields.timeslot].Value;
                errorData.contactAdaniRealty = datasource?.Fields[Templates.EnquiryFormTemp.Fields.contactAdaniRealty].Value; ;
                errorData.purpose = datasource?.Fields[Templates.EnquiryFormTemp.Fields.purpose].Value;
                errorData.planAVsiit = datasource?.Fields[Templates.EnquiryFormTemp.Fields.dateErrormessage].Value;
                enquireForm.errorData = errorData;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return enquireForm;
        }
        public List<LocatinAboutAdani> LocationAboutAdaniHouse(Rendering rendering)
        {
            List<LocatinAboutAdani> locatinAboutAdaniList = new List<LocatinAboutAdani>();
            string urlParam = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Location");
            urlParam = !string.IsNullOrEmpty(urlParam) ? urlParam.Replace("property-in-", "") : "";
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
                string strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;

                foreach (Item item in datasource.Children)
                {
                    if (string.IsNullOrEmpty(urlParam))
                    {
                        var nestedChild = item.Axes.GetDescendants().Where(x => x.TemplateID == Templates.AboutAdaniHouse.TemplateID).ToList();
                        foreach (var root in nestedChild)
                        {
                            LocatinAboutAdani locatinAboutAdani = new LocatinAboutAdani();
                            locatinAboutAdani.Location = root.Parent.Name;
                            locatinAboutAdani.heading = root.Fields[Templates.AboutAdaniHouse.Fields.headingFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.headingFieldName].Value : "";
                            locatinAboutAdani.about = root.Fields[Templates.AboutAdaniHouse.Fields.aboutFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.aboutFieldName].Value : "";
                            locatinAboutAdani.readMore = root.Fields[Templates.AboutAdaniHouse.Fields.readMoreFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.readMoreFieldName].Value : "";
                            locatinAboutAdani.Links = Helper.GetLinkURL(root, Templates.AboutAdaniHouse.Fields.LinkFieldName) != null ? Helper.GetLinkURL(root, Templates.AboutAdaniHouse.Fields.LinkFieldName) : "";
                            //locatinAboutAdani.Links = locatinAboutAdani.Links != "" ?  locatinAboutAdani.Links : locatinAboutAdani.Links;
                            locatinAboutAdani.terms = root.Fields[Templates.AboutAdaniHouse.Fields.termsFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.termsFieldName].Value : "";
                            locatinAboutAdani.detailLink = root.Fields[Templates.AboutAdaniHouse.Fields.detailLinkFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.detailLinkFieldName].Value : "";
                            locatinAboutAdani.extrCharges = root.Fields[Templates.AboutAdaniHouse.Fields.extrChargesFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.extrChargesFieldName].Value : "";

                            locatinAboutAdani.readMoreLabel = root.Fields[Templates.AboutAdaniHouse.Fields.ReadMore] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.ReadMore].Value : "";
                            locatinAboutAdani.readLessLabel = root.Fields[Templates.AboutAdaniHouse.Fields.ReadLess] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.ReadLess].Value : "";
                            locatinAboutAdaniList.Add(locatinAboutAdani);
                        }

                    }
                    else
                    {
                        var nestedChild = item.Axes.GetDescendants().Where(x => x.TemplateID == Templates.AboutAdaniHouse.TemplateID).ToList();
                        foreach (var root in nestedChild)
                        {
                            if (item.Name.ToString().ToLower() == urlParam.ToString().ToLower())
                            {
                                LocatinAboutAdani locatinAboutAdani = new LocatinAboutAdani();
                                locatinAboutAdani.Location = root.Parent.Name;
                                locatinAboutAdani.heading = root.Fields[Templates.AboutAdaniHouse.Fields.headingFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.headingFieldName].Value : "";
                                locatinAboutAdani.about = root.Fields[Templates.AboutAdaniHouse.Fields.aboutFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.aboutFieldName].Value : "";
                                locatinAboutAdani.readMore = root.Fields[Templates.AboutAdaniHouse.Fields.readMoreFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.readMoreFieldName].Value : "";
                                locatinAboutAdani.Links = Helper.GetLinkURL(root, Templates.AboutAdaniHouse.Fields.LinkFieldName) != null ? Helper.GetLinkURL(root, Templates.AboutAdaniHouse.Fields.LinkFieldName) : "";
                                //locatinAboutAdani.Links = locatinAboutAdani.Links != "" ? strSitedomain + locatinAboutAdani.Links : locatinAboutAdani.Links;

                                locatinAboutAdani.terms = root.Fields[Templates.AboutAdaniHouse.Fields.termsFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.termsFieldName].Value : "";
                                locatinAboutAdani.detailLink = root.Fields[Templates.AboutAdaniHouse.Fields.detailLinkFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.detailLinkFieldName].Value : "";
                                locatinAboutAdani.extrCharges = root.Fields[Templates.AboutAdaniHouse.Fields.extrChargesFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.extrChargesFieldName].Value : "";

                                locatinAboutAdani.readMoreLabel = root.Fields[Templates.AboutAdaniHouse.Fields.ReadMore] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.ReadMore].Value : "";
                                locatinAboutAdani.readLessLabel = root.Fields[Templates.AboutAdaniHouse.Fields.ReadLess] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.ReadLess].Value : "";
                                locatinAboutAdaniList.Add(locatinAboutAdani);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver LocationAboutAdaniHouse gives -> " + ex.Message);
            }
            return locatinAboutAdaniList;
        }
        public List<LocatinAboutAdani> SeoAboutAdaniHouse(Rendering rendering)
        {
            List<LocatinAboutAdani> locatinAboutAdaniList = new List<LocatinAboutAdani>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                List<Item> AboutList = new List<Item>() { datasource };

                foreach (var root in AboutList)
                {
                    LocatinAboutAdani locatinAboutAdani = new LocatinAboutAdani();
                    //locatinAboutAdani.Location = root.Parent.Name;
                    locatinAboutAdani.heading = root.Fields[Templates.AboutAdaniHouse.Fields.headingFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.headingFieldName].Value : "";
                    locatinAboutAdani.about = root.Fields[Templates.AboutAdaniHouse.Fields.aboutFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.aboutFieldName].Value : "";
                    locatinAboutAdani.readMore = root.Fields[Templates.AboutAdaniHouse.Fields.readMoreFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.readMoreFieldName].Value : "";
                    locatinAboutAdani.Links = Helper.GetLinkURL(root, Templates.AboutAdaniHouse.Fields.LinkFieldName) != null ? Helper.GetLinkURL(root, Templates.AboutAdaniHouse.Fields.LinkFieldName) : "";
                    //locatinAboutAdani.Links = locatinAboutAdani.Links != "" ?  locatinAboutAdani.Links : locatinAboutAdani.Links;
                    locatinAboutAdani.terms = root.Fields[Templates.AboutAdaniHouse.Fields.termsFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.termsFieldName].Value : "";
                    locatinAboutAdani.detailLink = root.Fields[Templates.AboutAdaniHouse.Fields.detailLinkFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.detailLinkFieldName].Value : "";
                    locatinAboutAdani.extrCharges = root.Fields[Templates.AboutAdaniHouse.Fields.extrChargesFieldName] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.extrChargesFieldName].Value : "";

                    locatinAboutAdani.readMoreLabel = root.Fields[Templates.AboutAdaniHouse.Fields.ReadMore] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.ReadMore].Value : "";
                    locatinAboutAdani.readLessLabel = root.Fields[Templates.AboutAdaniHouse.Fields.ReadLess] != null ? root.Fields[Templates.AboutAdaniHouse.Fields.ReadLess].Value : "";
                    locatinAboutAdaniList.Add(locatinAboutAdani);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver LocationAboutAdaniHouse gives -> " + ex.Message);
            }
            return locatinAboutAdaniList;
        }
        public topNavigationModel GetTopnavigationList(Rendering rendering)
        {
            topNavigationModel headerMenuMenuList = new topNavigationModel();
            try
            {
                headerMenuMenuList.topNavigation = GetTopnavigationItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver GetTopnavigationList gives -> " + ex.Message);
            }


            return headerMenuMenuList;
        }
        public List<HeaderMenuItem> GetTopnavigationItem(Rendering rendering)
        {
            List<HeaderMenuItem> listOfTopNavigation = new List<HeaderMenuItem>();
            var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                                : null;
            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException();
            }
            try
            {
                HeaderMenuItem menuItem;
                foreach (Item item in datasource.Children)
                {
                    menuItem = new HeaderMenuItem();
                    if (item.TemplateID == HeaderItemTemp.HamburgerMenuLinkTemplateID)
                    {
                        menuItem.headerText = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? item.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                        menuItem.headerLink = Helper.GetLinkURL(item, HeaderItemTemp.Fields.headerLinkName) != null ?
                                        Helper.GetLinkURL(item, HeaderItemTemp.Fields.headerLinkName) : "";
                        menuItem.headerLeftIcon = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? item.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                        menuItem.headerRightIcon = !string.IsNullOrEmpty(item.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? item.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                        if (item.HasChildren)
                        {
                            List<collapseItemslist> dataStoreageObject = new List<collapseItemslist>();
                            foreach (Item subitem in item.Children)
                            {
                                collapseItemslist innerobject = new collapseItemslist();
                                if (subitem.TemplateID == HeaderItemTemp.TemplateID)
                                {
                                    innerobject.itemText = !string.IsNullOrEmpty(subitem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? subitem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                                    innerobject.itemSubText = !string.IsNullOrEmpty(subitem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString()) ? subitem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString() : "";
                                    innerobject.itemLink = Helper.GetLinkURL(subitem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                    Helper.GetLinkURL(subitem, HeaderItemTemp.Fields.headerLinkName) : "";
                                    innerobject.target = Helper.GetLinkURLTargetSpace(subitem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                    Helper.GetLinkURLTargetSpace(subitem, HeaderItemTemp.Fields.headerLinkName) : "";
                                    innerobject.itemLeftIcon = !string.IsNullOrEmpty(subitem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? subitem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                                    innerobject.itemRightIcon = !string.IsNullOrEmpty(subitem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? subitem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                                    if (subitem.HasChildren)
                                    {
                                        List<collapseItemslist> ineerDataStorage = new List<collapseItemslist>();
                                        foreach (Item innerItems in subitem.Children)
                                        {
                                            collapseItemslist innerItemobject = new collapseItemslist();
                                            if (innerItems.TemplateID == HeaderItemTemp.TemplateID)
                                            {
                                                innerItemobject.itemText = !string.IsNullOrEmpty(innerItems.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? innerItems.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                                                innerItemobject.itemSubText = !string.IsNullOrEmpty(innerItems.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString()) ? innerItems.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString() : "";
                                                innerItemobject.itemLink = Helper.GetLinkURL(innerItems, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                                     Helper.GetLinkURL(innerItems, HeaderItemTemp.Fields.headerLinkName) : "";
                                                innerItemobject.itemLeftIcon = !string.IsNullOrEmpty(innerItems.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? innerItems.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                                                innerItemobject.itemRightIcon = !string.IsNullOrEmpty(innerItems.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? innerItems.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                                                if (innerItems.HasChildren)
                                                {
                                                    List<collapseItemslist> nestedDataStorage = new List<collapseItemslist>();
                                                    foreach (Item nestedItem in innerItems.Children)
                                                    {
                                                        collapseItemslist nestedItemobject = new collapseItemslist();
                                                        if (nestedItem.TemplateID == HeaderItemTemp.TemplateID)
                                                        {
                                                            nestedItemobject.itemText = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString()) ? nestedItem.Fields[HeaderItemTemp.Fields.headerTextID].Value.ToString() : "";
                                                            nestedItemobject.itemSubText = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString()) ? nestedItem.Fields[HeaderItemTemp.Fields.headersubTextID].Value.ToString() : "";
                                                            nestedItemobject.itemLink = Helper.GetLinkURL(nestedItem, HeaderItemTemp.Fields.headerLinkName) != null ?
                                                                                 Helper.GetLinkURL(nestedItem, HeaderItemTemp.Fields.headerLinkName) : "";
                                                            nestedItemobject.itemLeftIcon = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString()) ? nestedItem.Fields[HeaderItemTemp.Fields.headerLeftIconID].Value.ToString() : "";
                                                            nestedItemobject.itemRightIcon = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString()) ? nestedItem.Fields[HeaderItemTemp.Fields.headerRightIconID].Value.ToString() : "";
                                                            nestedItemobject.itemImage = !string.IsNullOrEmpty(nestedItem.Fields[HeaderItemTemp.Fields.headerLinkID].Value) ? Helper.GetImageURL(nestedItem, HeaderItemTemp.Fields.headerImageName) : "";
                                                            nestedDataStorage.Add(nestedItemobject);
                                                        }
                                                    }
                                                    innerItemobject.collapseItems = nestedDataStorage;
                                                }
                                                ineerDataStorage.Add(innerItemobject);
                                            }
                                        }
                                        innerobject.collapseItems = ineerDataStorage;
                                    }
                                    dataStoreageObject.Add(innerobject);
                                }
                                menuItem.items = dataStoreageObject;
                            }
                        }
                        listOfTopNavigation.Add(menuItem);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver GetTopnavigationItem gives -> " + ex.Message);
            }
            return listOfTopNavigation;
        }
        public Headerairportlist GetAirportList(Rendering rendering)
        {
            Headerairportlist headerMenuList = new Headerairportlist();
            try
            {
                headerMenuList.airportList = GetHeaderAirportItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver GetAirportList gives -> " + ex.Message);
            }
            return headerMenuList;
        }
        public List<AirportList> GetHeaderAirportItem(Rendering rendering)
        {
            List<AirportList> listOfTopNavigation = new List<AirportList>();
            var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                                : null;
            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException();
            }
            var childern = datasource.Children.Where(x => x.TemplateID == Templates.AirportHeaderTemp.AirportLinkTemplatedID).ToList();

            try
            {
                if (childern != null)
                {
                    foreach (var item in childern)
                    {
                        List<AirportItem> airportItems = new List<AirportItem>();
                        AirportList airportList = new AirportList();
                        airportList.headerText = !string.IsNullOrEmpty(item.Fields[Templates.AirportHeaderTemp.Fields.headerText].Value) ? item.Fields[Templates.AirportHeaderTemp.Fields.headerText].Value : "";
                        airportList.headerLink = Helper.GetLinkURLbyField(item, item.Fields[AirportHeaderTemp.Fields.headerLink]) != null ?
                                  Helper.GetLinkURLbyField(item, item.Fields[AirportHeaderTemp.Fields.headerLink]) : "";
                        airportList.headerImage = Helper.GetImageSource(item, AirportHeaderTemp.Fields.headerImage.ToString()) != null ?
                                   Helper.GetImageSource(item, AirportHeaderTemp.Fields.headerImage.ToString()) : "";
                        airportList.headerLogoLink = Helper.GetLinkURLbyField(item, item.Fields[AirportHeaderTemp.Fields.headerLogoLink]) != null ?
                                  Helper.GetLinkURLbyField(item, item.Fields[AirportHeaderTemp.Fields.headerLogoLink]) : "";
                        airportList.headerLogoAltText = Helper.GetImageDetails(item, AirportHeaderTemp.Fields.headerImage.ToString()) != null ?
                                        Helper.GetImageDetails(item, AirportHeaderTemp.Fields.headerImage.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                        var currentItem = Sitecore.Context.Item;
                        if (currentItem.ID != HomeItemID)
                        {
                            airportList.headerBackText = !string.IsNullOrEmpty(item.Fields[Templates.AirportHeaderTemp.Fields.headerBackText].Value) ? item.Fields[Templates.AirportHeaderTemp.Fields.headerBackText].Value : "";
                            airportList.headerBackLink = Helper.GetLinkURLbyField(item, item.Fields[AirportHeaderTemp.Fields.headerBackLink]) != null ?
                                      Helper.GetLinkURLbyField(item, item.Fields[AirportHeaderTemp.Fields.headerBackLink]) : "";
                        }
                        var nestedlist = item.Children.Where(x => x.TemplateID == AirportHeaderTemp.AirportHeaderTemplatedID).ToList();
                        if (nestedlist != null)
                        {
                            foreach (var nestedItem in nestedlist)
                            {
                                AirportItem nestedObject = new AirportItem();
                                nestedObject.itemText = !string.IsNullOrEmpty(nestedItem.Fields[Templates.AirportHeaderTemp.Fields.headerText].Value) ? nestedItem.Fields[Templates.AirportHeaderTemp.Fields.headerText].Value : "";
                                nestedObject.itemLink = Helper.GetLinkURLbyField(nestedItem, nestedItem.Fields[AirportHeaderTemp.Fields.headerLink]) != null ?
                                  Helper.GetLinkURLbyField(nestedItem, nestedItem.Fields[AirportHeaderTemp.Fields.headerLink]) : "";
                                nestedObject.itemImage = Helper.GetImageSource(nestedItem, AirportHeaderTemp.Fields.headerImage.ToString()) != null ?
                                           Helper.GetImageSource(nestedItem, AirportHeaderTemp.Fields.headerImage.ToString()) : "";
                                nestedObject.airportcode = !string.IsNullOrEmpty(nestedItem.Fields[Templates.AirportHeaderTemp.Fields.airportcode].Value) ? nestedItem.Fields[Templates.AirportHeaderTemp.Fields.airportcode].Value : "";
                                airportItems.Add(nestedObject);
                            }
                        }
                        else
                        {
                            nestedlist = null;
                            Sitecore.Diagnostics.Error.LogError("datasource is null at NavigationRootResolver GetHeaderAirportItem line no 792");
                        }
                        airportList.items = airportItems;
                        listOfTopNavigation.Add(airportList);
                    }
                }
                else
                {
                    listOfTopNavigation = null;
                    Sitecore.Diagnostics.Error.LogError("datasource is null at NavigationRootResolver GetHeaderAirportItem line no 771");
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver GetHeaderAirportItem gives -> " + ex.Message);
            }
            return listOfTopNavigation;
        }
        public SEOData GetSEOdataCOntentResolver(Rendering rendering)
        {
            SEOData seoDataObj = new SEOData();
            var currentItem = Sitecore.Context.Item;
            string urlParam = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Location");
            urlParam = !string.IsNullOrEmpty(urlParam) ? urlParam.Replace("property-in-", "") : "";
            string categoryId = Sitecore.Context.Request?.QueryString["catName"];
            var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
            var domain = !string.IsNullOrEmpty(commonItem.Fields[Templates.commonData.Fields.SiteDomain].Value) ? commonItem.Fields[Templates.commonData.Fields.SiteDomain].Value : "";
            try
            {
                if (string.IsNullOrEmpty(urlParam) && string.IsNullOrEmpty(categoryId))
                {
                    if (currentItem != null)
                    {
                        seoDataObj.MetaTitle = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.metatitleID].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.metatitleID].Value : "";
                        seoDataObj.MetaKeywords = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaKeywords].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaKeywords].Value : "";
                        seoDataObj.metaDescription = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaDescription].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaDescription].Value : "";
                        seoDataObj.pageTitle = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.PageTitle].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.PageTitle].Value : "";
                        seoDataObj.ogTitle = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.OgTitle].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.OgTitle].Value : "";
                        seoDataObj.robotsTags = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.RobotsTags].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.RobotsTags].Value : "";
                        seoDataObj.browserTitle = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.BrowserTitle].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.BrowserTitle].Value : "";
                        seoDataObj.ogDescription = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.OgDescription].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.OgDescription].Value : "";
                        seoDataObj.ogImage = Helper.GetImageSource(currentItem, SEODataTemplate.PageTemplateFields.OgImage.ToString()) != null ? Helper.GetImageSource(currentItem, SEODataTemplate.PageTemplateFields.OgImage.ToString()) : "";
                        seoDataObj.ogKeyword = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.OgKeyword].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.OgKeyword].Value : "";
                        if (currentItem.ID != Templates.HomeItemID)
                        {
                            seoDataObj.canonicalUrl = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.canonicalUrl].Value) ? domain + currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.canonicalUrl].Value : "";
                        }
                        else
                        {
                            seoDataObj.canonicalUrl = domain + "/";
                        }
                        seoDataObj.googleSiteVerification = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.googleSiteVerification].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.googleSiteVerification].Value : "";
                        seoDataObj.msValidate = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.msValidate].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.msValidate].Value : "";
                        orgSchema orgSchema = new orgSchema();
                        orgSchema.telephone = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.telephone].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.telephone].Value : "";
                        orgSchema.contactType = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.contactType].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.contactType].Value : "";
                        orgSchema.areaServed = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.areaServed].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.areaServed].Value : "";
                        orgSchema.streetAddress = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.streetAddress].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.streetAddress].Value : "";
                        orgSchema.addressLocality = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.addressLocality].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.addressLocality].Value : "";
                        orgSchema.addressRegion = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.addressRegion].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.addressRegion].Value : "";
                        orgSchema.postalCode = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.postalCode].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.postalCode].Value : "";
                        orgSchema.contactOption = !string.IsNullOrEmpty(currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.ContactOption].Value) ? currentItem.Fields[Templates.SEODataTemplate.PageTemplateFields.ContactOption].Value : "";
                        orgSchema.logo = Helper.GetImageSource(currentItem, SEODataTemplate.PageTemplateFields.Logo.ToString()) != null ? Helper.GetImageSource(currentItem, SEODataTemplate.PageTemplateFields.Logo.ToString()) : "";
                        orgSchema.url = seoDataObj.canonicalUrl;
                        var childern = Helper.GetMultiListValueItem(currentItem, Templates.SEODataTemplate.PageTemplateFields.sameASTemplateID);
                        if (childern != null)
                        {
                            foreach (var nestedItem in childern)
                            {
                                var linkFeild = Helper.GetLinkURL(nestedItem, Templates.SEODataTemplate.PageTemplateFields.Fields.navLink.ToString()) != null ?
                                                     Helper.GetLinkURL(nestedItem, Templates.SEODataTemplate.PageTemplateFields.Fields.navLink.ToString()) : "";
                                orgSchema.sameAs.Add(linkFeild);
                            }
                        }
                        seoDataObj.orgSchema = orgSchema;
                    }
                }
                else if (!string.IsNullOrEmpty(urlParam))
                {
                    if (currentItem.ID == Templates.LocationPageTemp.ItemID)
                    {
                        var locSelection = Sitecore.Context.Database.GetItem(LocationPageTemp.Fields.LocationSelectionID);
                        var citiesValues = Helper.GetMultiListValueItem(locSelection, LocationPageTemp.Fields.Cities);
                        if (citiesValues != null)
                        {
                            foreach (var city in citiesValues)
                            {
                                if (city.Name.ToString().ToLower() == urlParam.ToString().ToLower())
                                {
                                    seoDataObj.MetaTitle = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.metatitleID].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.metatitleID].Value : "";
                                    seoDataObj.MetaKeywords = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaKeywords].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaKeywords].Value : "";
                                    seoDataObj.metaDescription = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaDescription].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaDescription].Value : "";
                                    seoDataObj.pageTitle = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.PageTitle].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.PageTitle].Value : "";
                                    seoDataObj.ogTitle = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.OgTitle].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.OgTitle].Value : "";
                                    seoDataObj.ogDescription = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.OgDescription].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.OgDescription].Value : "";
                                    seoDataObj.robotsTags = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.RobotsTags].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.RobotsTags].Value : "";
                                    seoDataObj.browserTitle = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.BrowserTitle].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.BrowserTitle].Value : "";
                                    seoDataObj.ogImage = Helper.GetImageSource(city, SEODataTemplate.PageTemplateFields.OgImage.ToString()) != null ? Helper.GetImageSource(city, SEODataTemplate.PageTemplateFields.OgImage.ToString()) : "";
                                    seoDataObj.ogKeyword = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.OgKeyword].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.OgKeyword].Value : "";
                                    seoDataObj.canonicalUrl = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.canonicalUrl].Value) ? domain + city.Fields[Templates.SEODataTemplate.PageTemplateFields.canonicalUrl].Value : "";
                                    seoDataObj.googleSiteVerification = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.googleSiteVerification].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.googleSiteVerification].Value : "";
                                    seoDataObj.msValidate = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.msValidate].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.msValidate].Value : "";
                                    orgSchema orgSchema = new orgSchema();
                                    orgSchema.telephone = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.telephone].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.telephone].Value : "";
                                    orgSchema.contactType = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.contactType].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.contactType].Value : "";
                                    orgSchema.areaServed = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.areaServed].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.areaServed].Value : "";
                                    orgSchema.streetAddress = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.streetAddress].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.streetAddress].Value : "";
                                    orgSchema.addressLocality = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.addressLocality].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.addressLocality].Value : "";
                                    orgSchema.addressRegion = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.addressRegion].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.addressRegion].Value : "";
                                    orgSchema.postalCode = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.postalCode].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.postalCode].Value : "";
                                    orgSchema.contactOption = !string.IsNullOrEmpty(city.Fields[Templates.SEODataTemplate.PageTemplateFields.ContactOption].Value) ? city.Fields[Templates.SEODataTemplate.PageTemplateFields.ContactOption].Value : "";
                                    orgSchema.logo = Helper.GetImageSource(city, SEODataTemplate.PageTemplateFields.Logo.ToString()) != null ? Helper.GetImageSource(city, SEODataTemplate.PageTemplateFields.Logo.ToString()) : "";
                                    orgSchema.url = seoDataObj.canonicalUrl;
                                    var childern = Helper.GetMultiListValueItem(city, Templates.SEODataTemplate.PageTemplateFields.sameASTemplateID);
                                    if (childern != null)
                                    {
                                        foreach (var nestedItem in childern)
                                        {
                                            var linkFeild = Helper.GetLinkURL(nestedItem, Templates.SEODataTemplate.PageTemplateFields.Fields.navLink.ToString()) != null ?
                                                                 Helper.GetLinkURL(nestedItem, Templates.SEODataTemplate.PageTemplateFields.Fields.navLink.ToString()) : "";
                                            orgSchema.sameAs.Add(linkFeild);
                                        }
                                    }
                                    seoDataObj.orgSchema = orgSchema;
                                }
                            }
                        }
                    }

                }
                else if (!string.IsNullOrEmpty(categoryId))
                {
                    var currerntCategory = Sitecore.Context.Database.GetItem(BlogCategory.BlogAnchorsFolder);
                    var categoryNameCheck = currerntCategory.Children.Where(x => x.Name.ToLower().Contains(categoryId)).FirstOrDefault();
                    if (categoryNameCheck != null && categoryNameCheck.TemplateID == CommunicationCornerTemp.BlogAnchorsID)
                    {
                        seoDataObj.MetaTitle = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.metatitleID].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.metatitleID].Value : "";
                        seoDataObj.MetaKeywords = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaKeywords].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaKeywords].Value : "";
                        seoDataObj.metaDescription = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaDescription].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaDescription].Value : "";
                        seoDataObj.pageTitle = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.PageTitle].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.PageTitle].Value : "";
                        seoDataObj.ogTitle = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.OgTitle].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.OgTitle].Value : "";
                        seoDataObj.robotsTags = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.RobotsTags].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.RobotsTags].Value : "";
                        seoDataObj.browserTitle = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.BrowserTitle].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.BrowserTitle].Value : "";
                        seoDataObj.ogDescription = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.OgDescription].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.OgDescription].Value : "";
                        seoDataObj.ogImage = Helper.GetImageSource(categoryNameCheck, SEODataTemplate.PageTemplateFields.OgImage.ToString()) != null ? Helper.GetImageSource(categoryNameCheck, SEODataTemplate.PageTemplateFields.OgImage.ToString()) : "";
                        seoDataObj.ogKeyword = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.OgKeyword].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.OgKeyword].Value : "";
                        seoDataObj.canonicalUrl = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.canonicalUrl].Value) ? domain + categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.canonicalUrl].Value : "";
                        seoDataObj.googleSiteVerification = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.googleSiteVerification].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.googleSiteVerification].Value : "";
                        seoDataObj.msValidate = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.msValidate].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.msValidate].Value : "";
                        orgSchema orgSchema = new orgSchema();
                        orgSchema.telephone = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.telephone].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.telephone].Value : "";
                        orgSchema.contactType = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.contactType].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.contactType].Value : "";
                        orgSchema.areaServed = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.areaServed].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.areaServed].Value : "";
                        orgSchema.streetAddress = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.streetAddress].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.streetAddress].Value : "";
                        orgSchema.addressLocality = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.addressLocality].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.addressLocality].Value : "";
                        orgSchema.addressRegion = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.addressRegion].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.addressRegion].Value : "";
                        orgSchema.postalCode = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.postalCode].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.postalCode].Value : "";
                        orgSchema.contactOption = !string.IsNullOrEmpty(categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.ContactOption].Value) ? categoryNameCheck.Fields[Templates.SEODataTemplate.PageTemplateFields.ContactOption].Value : "";
                        orgSchema.logo = Helper.GetImageSource(categoryNameCheck, SEODataTemplate.PageTemplateFields.Logo.ToString()) != null ? Helper.GetImageSource(categoryNameCheck, SEODataTemplate.PageTemplateFields.Logo.ToString()) : "";
                        orgSchema.url = seoDataObj.canonicalUrl;
                        var childern = Helper.GetMultiListValueItem(categoryNameCheck, Templates.SEODataTemplate.PageTemplateFields.sameASTemplateID);
                        if (childern != null)
                        {
                            foreach (var nestedItem in childern)
                            {
                                var linkFeild = Helper.GetLinkURL(nestedItem, Templates.SEODataTemplate.PageTemplateFields.Fields.navLink.ToString()) != null ?
                                                     Helper.GetLinkURL(nestedItem, Templates.SEODataTemplate.PageTemplateFields.Fields.navLink.ToString()) : "";
                                orgSchema.sameAs.Add(linkFeild);
                            }
                        }
                        seoDataObj.orgSchema = orgSchema;
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" NavigationRootResolver GetSEOdataCOntentResolver gives -> " + ex.Message);
            }
            return seoDataObj;
        }
        public otherProjects GetOtherProjectdata(Rendering rendering)
        {
            otherProjects otherProjects = new otherProjects();
            paramdata paramdata = new paramdata();
            List<projectdata> otherprojectsdata = new List<projectdata>();
            
            List<Item> listOfProperty = new List<Item>();
            List<Item> data = new List<Item>();
            List<Item> filtereddata = new List<Item>();
            List<Item> cityFilter = new List<Item>();
           

            paramdata.ressOffer = Sitecore.Context.Item.Fields["Heading"].Value;
            otherProjects.param = paramdata;
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                var propertyType = datasource.TemplateID.ToString() == OtherProjects.TemplateID.ToString() ? datasource.GetMultiListValueItem(OtherProjects.PropertiesID) : null;
                if (propertyType != null)
                {
                    foreach (var propertyTypeItem in propertyType)
                    {
                        var x = propertyTypeItem.Axes.GetDescendants().Where(y => y.TemplateID == OtherProjects.ResidentialID || y.TemplateID == OtherProjects.CommercialID);
                        listOfProperty.AddRange(x);
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }

                var location = Sitecore.Context.Item.Fields["CityName"].Value; //SEO Page
                if (location is null)
                {
                    filtereddata.AddRange(listOfProperty);
                }
                if (location != null)
                {
                    foreach (Item item in listOfProperty)
                    {
                        var locationItem1 = Helper.GetSelectedItemFromDroplistFieldValue(item, OtherProjects.Fields.Location); //All Property
                        if (location == locationItem1)
                        {
                            data.Add(item);
                        }
                    }
                    filtereddata.Clear();
                }
                filtereddata.AddRange(data);
                data.Clear();

                var projectType = Helper.GetSelectedItemFromDroplistFieldValue(Sitecore.Context.Item, OtherProjects.Fields.ProjectTypes); //SEO Page
                if (projectType != null)
                {
                    foreach (Item item in filtereddata)
                    {
                        var projectType1 = Helper.GetSelectedItemFromDroplistFieldValue(item, OtherProjects.Fields.ProjectType); //All Property
                        if (projectType == projectType1)
                        {
                            data.Add(item);
                        }
                    }
                    filtereddata.Clear();
                }
                filtereddata.AddRange(data);
                data.Clear();

                var area = Helper.GetSelectedItemFromDroplistFieldValue(Sitecore.Context.Item, OtherProjects.Fields.Area); //SEO Page
                if (area != null)
                {
                    foreach (var item in filtereddata)
                    {
                        var locationItem1 = Helper.GetSelectedItemFromDroplistFieldValue(item, OtherProjects.Fields.Subcity); //All Property
                        if (area == locationItem1)
                        {
                            data.Add(item);
                        }
                    }
                    filtereddata.Clear();
                    filtereddata.AddRange(data);
                }
                data.Clear();

                var Flats = Helper.GetSelectedItemFromDroplistFieldValue(Sitecore.Context.Item, OtherProjects.Fields.Flats); ; //SEO Page
                if (Flats != null)
                {
                    List<string> FlatItem = new List<string>();
                    foreach (var item in filtereddata)
                    {
                        var Flat = item.GetMultiListValueItem(OtherProjects.Fields.PropertyType);
                        foreach (var type in Flat)
                        {
                            var x = !string.IsNullOrEmpty(type.DisplayName) ? type.DisplayName : type.Name;
                            FlatItem.Add(x);
                        }
                        if (FlatItem.Contains(Flats))
                        {
                            data.Add(item);
                        }
                        FlatItem.Clear();

                    }
                    filtereddata.Clear();
                    filtereddata.AddRange(data);
                }
                data.Clear();

                var projectStatus = Helper.GetSelectedItemFromDroplistFieldValue(Sitecore.Context.Item, OtherProjects.Fields.SEOProjectStatus); //SEO Page
                if (projectStatus != null)
                {
                    foreach (var item in filtereddata)
                    {
                        var locationItem1 = Helper.GetSelectedItemFromDroplistFieldValue(item, OtherProjects.Fields.ProjectStatus); //All Property
                        if (projectStatus.ToLower() == locationItem1.ToLower())
                        {
                            data.Add(item);
                        }
                    }
                    filtereddata.Clear();
                    filtereddata.AddRange(data);
                }
                data.Clear();

                var typeItem = Helper.GetSelectedItemFromDroplistFieldValue(Sitecore.Context.Item, OtherProjects.Fields.ConfugurationType); //SEO Page
                if (typeItem != null)
                {
                    List<string> FlatItem = new List<string>();
                    foreach (var item in filtereddata)
                    {
                        var field = item.GetMultiListValueItem(OtherProjects.Fields.typeID);
                        foreach (var type in field)
                        {
                            var x = !string.IsNullOrEmpty(type.DisplayName) ? type.DisplayName : type.Name;
                            FlatItem.Add(x);
                        }
                        if (FlatItem.Contains(typeItem))
                        {
                            data.Add(item);
                        }
                        FlatItem.Clear();
                    }
                    filtereddata.Clear();
                    filtereddata.AddRange(data);
                }
                data.Clear();

                foreach (var item in listOfProperty)
                {
                    var currentProjectType = Helper.GetSelectedItemFromDroplistFieldValue(item, OtherProjects.Fields.ProjectType); //All Property
                    var currentLocation = Helper.GetSelectedItemFromDroplistFieldValue(item, OtherProjects.Fields.Location);
                    if (!filtereddata.Contains(item) && projectType == currentProjectType)
                    {
                        if (currentLocation == location)
                        {
                            cityFilter.Insert(0, item);
                        }
                        else
                        {
                            cityFilter.Add(item);
                        }
                    }
                }
                
                foreach (var item in cityFilter)
                {
                    var currentProjectType = Helper.GetSelectedItemFromDroplistFieldValue(item, OtherProjects.Fields.ProjectType); //All Property
                    //var currentLocation = Helper.GetSelectedItemFromDroplistFieldValue(item, OtherProjects.Fields.Location);
                    if (!filtereddata.Contains(item) && projectType == currentProjectType )
                    {
                        projectdata projectdatas = new projectdata();
                        projectdatas.propertyID = item.ID.ToString();
                        projectdatas.link = Helper.GetLinkURL(item, ResidentialProjects.Fields.linkFieldName) != null ?
                                             Helper.GetLinkURL(item, ResidentialProjects.Fields.linkFieldName) : "";
                        projectdatas.linktarget = Helper.GetLinkURLTargetSpace(item, ResidentialProjects.Fields.linkFieldName) != null ?
                                  Helper.GetLinkURLTargetSpace(item, ResidentialProjects.Fields.linkFieldName) : "";
                        projectdatas.logo = !string.IsNullOrEmpty(Helper.GetImageSource(item, ResidentialProjects.Fields.Property_LogoFieldName)) ?
                                   Helper.GetImageSource(item, ResidentialProjects.Fields.Property_LogoFieldName) : "";
                        projectdatas.logotitle = Helper.GetImageDetails(item, ResidentialProjects.Fields.Property_LogoFieldName) != null ?
                                   Helper.GetImageDetails(item, ResidentialProjects.Fields.Property_LogoFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                        projectdatas.logoalt = Helper.GetImageDetails(item, ResidentialProjects.Fields.Property_LogoFieldName) != null ?
                                   Helper.GetImageDetails(item, ResidentialProjects.Fields.Property_LogoFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                        projectdatas.src = !string.IsNullOrEmpty(Helper.GetImageSource(item, ResidentialProjects.Fields.ImageFieldName)) ?
                                   Helper.GetImageSource(item, ResidentialProjects.Fields.ImageFieldName) : "";
                        projectdatas.mobileimage = !string.IsNullOrEmpty(Helper.GetImageSource(item, ResidentialProjects.Fields.mobileimageFieldName)) ?
                                   Helper.GetImageSource(item, ResidentialProjects.Fields.mobileimageFieldName) : "";
                        projectdatas.imgalt = Helper.GetImageDetails(item, ResidentialProjects.Fields.ImageFieldName) != null ?
                                   Helper.GetImageDetails(item, ResidentialProjects.Fields.ImageFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                        projectdatas.title = item.Fields[ResidentialProjects.Fields.TitleFieldName].Value != null ? item.Fields[ResidentialProjects.Fields.TitleFieldName].Value : "";
                        projectdatas.imgtitle = Helper.GetImageDetails(item, ResidentialProjects.Fields.ImageFieldName) != null ?
                                   Helper.GetImageDetails(item, ResidentialProjects.Fields.ImageFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                        projectdatas.location = item.Fields[ResidentialProjects.Fields.LocationFieldName].Value != null ? item.Fields[ResidentialProjects.Fields.LocationFieldName].Value : "";
                        projectdatas.type = item.Fields[ResidentialProjects.Fields.TypeFieldName].Value != null ? item.Fields[ResidentialProjects.Fields.TypeFieldName].Value : "";
                        projectdatas.imgtype = item.Fields[ResidentialProjects.Fields.imgtypeFieldName].Value != null ? item.Fields[ResidentialProjects.Fields.imgtypeFieldName].Value : "";
                        projectdatas.propertyType = item.Fields[ResidentialProjects.Fields.Property_TypeFieldName].Value != null ? item.Fields[ResidentialProjects.Fields.Property_TypeFieldName].Value : "";
                        projectdatas.latitude = item.Fields[ResidentialProjects.Fields.LatitudeID].Value != null ? item.Fields[ResidentialProjects.Fields.LatitudeID].Value : "";
                        projectdatas.logitude = item.Fields[ResidentialProjects.Fields.LongitudeID].Value != null ? item.Fields[ResidentialProjects.Fields.LongitudeID].Value : "";
                        projectdatas.city = Helper.GetSelectedItemFromDroplistFieldValue(item, OtherProjects.Fields.Location);
                        //object k = projectdatas.location;

                        otherprojectsdata.Add(projectdatas);
                    }
                }
                otherProjects.data = otherprojectsdata;
                return otherProjects;
            }
            catch (Exception)
            {
                throw new NullReferenceException();
            }
        }

    }
}