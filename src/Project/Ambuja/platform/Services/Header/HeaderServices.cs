using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models;
using Project.AmbujaCement.Website.Templates;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using Item = Sitecore.Data.Items.Item;

namespace Project.AmbujaCement.Website.Services.Header
{
    public class HeaderServices : IHeaderServices
    {
        readonly Item _contextItem;
        public HeaderServices()
        {
            _contextItem = Sitecore.Context.Item;
        }

        public HeaderModel GetHeader(Rendering rendering)
        {
            HeaderModel headerdata = new HeaderModel();
            try
            {

                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                Item NavData = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Nav Data"));
                Item TopBarList = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Top Bar List"));
                Item HamburgerMenulist = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "HamburgerMenu Data"));

                headerdata.HeaderLogo = Utils.GetImageURLByFieldId(datasource, BaseTemplates.LogoSourceTemplate.headerLogo);
                headerdata.Logo = Utils.GetImageURLByFieldId(datasource, BaseTemplates.LogoSourceTemplate.Logo);
                headerdata.MobileLogo = Utils.GetImageURLByFieldId(datasource, BaseTemplates.LogoSourceTemplate.MobileLogo);
                headerdata.BuLink = Utils.GetLinkURL(datasource.Fields[HomePageTemplate.HeaderComponant.BuLink]);
                headerdata.LinkTarget = Utils.GetValue(datasource, BaseTemplates.LinkTemplate.LinkTargetFieldId);
                headerdata.IsAbsolute = Utils.GetBoleanValue(datasource, HomePageTemplate.HeaderComponant.IsAbsolute);
                headerdata.AddOnClass = Utils.GetValue(datasource, HomePageTemplate.HeaderComponant.AddOnClass);
                headerdata.PageHeading = Utils.GetValue(datasource, HomePageTemplate.HeaderComponant.PageHeading);
                headerdata.BuLogoAltText = Utils.GetValue(datasource, BaseTemplates.LogoSourceTemplate.LogoAlt);
                List<NavDatum> navData = new List<NavDatum>();
                List<TopbarList> topbarData = new List<TopbarList>();
                List<HamburgerMenuDatum> hamburgerMenuData = new List<HamburgerMenuDatum>();

                //Nav Data
                if (NavData == null) return null;
                foreach (Item item in NavData.Children)
                {
                    var navItem = new NavDatum
                    {
                        LinkText = Utils.GetValue(item, BaseTemplates.LinkTemplate.LinkTextFieldId),
                        LinkTarget = Utils.GetValue(item, BaseTemplates.LinkTemplate.LinkTargetFieldId),
                        Link = Utils.GetLinkURL(item.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                        HeaderCallback = Utils.GetBoleanValue(item, HomePageTemplate.HeaderComponant.HeaderCallback),
                        IsActive = Utils.GetBoleanValue(item, BaseTemplates.LinkTemplate.IsActive),
                        DefaultImage = Utils.GetImageURLByFieldId(item, HomePageTemplate.HeaderComponant.DefaultImage),
                        GtmData = new GtmData
                        {
                            Event = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Title = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                            Category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            SubCategory = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Label = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            PageType = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        }

                    };

                    if (item.HasChildren)
                    {
                        var subMenuItem = new List<Models.Item>();

                        foreach (Item subItem in item.Children)
                        {
                            var subNavItem = new Models.Item
                            {
                                LinkText = Utils.GetValue(subItem, BaseTemplates.LinkTemplate.LinkTextFieldId),
                                LinkTarget = Utils.GetValue(subItem, BaseTemplates.LinkTemplate.LinkTargetFieldId),
                                Link = Utils.GetLinkURL(subItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                                GtmData = new GtmData
                                {
                                    Event = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                                    Title = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                                    Category = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                                    SubCategory = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                                    Label = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                                    PageType = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                                },


                            };
                            if (subItem.HasChildren)
                            {
                                var childsubMenuItem = new List<Models.SubMenu>();
                                foreach (Item childsubItem in subItem.Children)
                                {
                                    var childsubMenuItems = new Models.SubMenu
                                    {
                                        Link = Utils.GetLinkURL(childsubItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                                        LinkText = Utils.GetValue(childsubItem, BaseTemplates.LinkTemplate.LinkTextFieldId),
                                        LinkTarget = Utils.GetValue(childsubItem, BaseTemplates.LinkTemplate.LinkTargetFieldId),
                                        GtmData = new GtmData
                                        {
                                            Event = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                                            Title = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                                            Category = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                                            SubCategory = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                                            Label = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                                            PageType = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                                        },
                                    };
                                    childsubMenuItem.Add(childsubMenuItems);
                                }
                                subNavItem.SubMenu = childsubMenuItem;
                            }
                            subMenuItem.Add(subNavItem);


                        }
                        navItem.Items = subMenuItem;
                    }
                    navData.Add(navItem);
                }
                //Top Bar List
                if (TopBarList == null) return null;
                foreach (Item item in TopBarList.Children)
                {

                    var topbarlistdata = new TopbarList
                    {
                        Phone = Utils.GetValue(item, HomePageTemplate.HeaderComponant.Phone),
                        PhoneLink = Utils.GetLinkURL(item.Fields[HomePageTemplate.HeaderComponant.PhoneLink]),
                        Link = Utils.GetLinkURL(item.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                        PhoneIcon = Utils.GetValue(item, HomePageTemplate.HeaderComponant.PhoneIcon),
                        LinkText = Utils.GetValue(item, BaseTemplates.LinkTemplate.LinkTextFieldId),
                        HeaderLeftIcon = Utils.GetValue(item, HomePageTemplate.HeaderComponant.HeaderLeftIcon),
                        HeaderRightIcon = Utils.GetValue(item, HomePageTemplate.HeaderComponant.HeaderRightIcon),
                        GtmData = new GtmData
                        {
                            Event = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Title = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                            Category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            SubCategory = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Label = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            PageType = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        },

                    };

                    if (item.HasChildren)
                    {
                        var navigationitemdata = new List<Models.CollapseItem>();
                        foreach (Item subItem in item.Children)
                        {

                            var subNavItem = new Models.CollapseItem
                            {
                                LinkText = Utils.GetValue(subItem, BaseTemplates.LinkTemplate.LinkTextFieldId),
                                ItemSubText = Utils.GetValue(subItem, HomePageTemplate.HeaderComponant.ItemSubText),
                                LinkTarget = Utils.GetValue(subItem, BaseTemplates.LinkTemplate.LinkTargetFieldId),
                                Link = Utils.GetLinkURL(subItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                                ItemLeftIcon = Utils.GetValue(subItem, BaseTemplates.CommonItemlist.ItemLeftIcon),
                                IsActive = Utils.GetBoleanValue(subItem, HomePageTemplate.HeaderComponant.IsActive),
                                GtmData = new GtmData
                                {
                                    Event = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                                    Title = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                                    Category = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                                    SubCategory = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                                    Label = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                                    PageType = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                                },
                            };

                            navigationitemdata.Add(subNavItem);
                        }

                        topbarlistdata.Items = navigationitemdata;
                    }

                    topbarData.Add(topbarlistdata);
                }
                //Hamburger Menu
                if (HamburgerMenulist == null) return null;
                foreach (Item item in HamburgerMenulist.Children)
                {

                    var hamburgerData = new HamburgerMenuDatum
                    {
                        LinkText = Utils.GetValue(item, BaseTemplates.LinkTemplate.LinkTextFieldId),
                        ItemSubText = Utils.GetValue(item, HomePageTemplate.HeaderComponant.ItemSubText),
                        ItemLeftIcon = Utils.GetValue(item, BaseTemplates.CommonItemlist.ItemLeftIcon),
                        LinkTarget = Utils.GetValue(item, BaseTemplates.LinkTemplate.LinkTargetFieldId),
                        Link = Utils.GetLinkURL(item.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                        HeaderLeftIcon = Utils.GetValue(item, BaseTemplates.HeaderFieldTemplate.HeaderLeftIcon),
                        HeaderRightIcon = Utils.GetValue(item, BaseTemplates.HeaderFieldTemplate.HeaderRightIcon),
                        GtmData = new GtmData
                        {
                            Event = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Title = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                            Category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            SubCategory = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Label = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            PageType = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        }

                    };

                    if (item.HasChildren)
                    {
                        var subMenuItem = new List<Models.HamburgerMenuItem>();

                        foreach (Item subItem in item.Children)
                        {
                            var subNavItem = new Models.HamburgerMenuItem
                            {
                                LinkText = Utils.GetValue(subItem, BaseTemplates.LinkTemplate.LinkTextFieldId),
                                LinkTarget = Utils.GetValue(subItem, BaseTemplates.LinkTemplate.LinkTargetFieldId),
                                Link = Utils.GetLinkURL(subItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                                ItemLeftIcon = Utils.GetValue(subItem, BaseTemplates.CommonItemlist.ItemLeftIcon),
                                ItemRightIcon = Utils.GetValue(subItem, BaseTemplates.HeaderFieldTemplate.ItemRightIcon),
                                HighlightLabel = Utils.GetValue(subItem, BaseTemplates.HeaderFieldTemplate.HighlightLabel),
                                GtmData = new GtmData
                                {
                                    Event = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                                    Title = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                                    Category = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                                    SubCategory = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                                    Label = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                                    PageType = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                                },
                            };

                            if (subItem.HasChildren)
                            {
                                var childsubMenuItem = new List<Models.CollapseItem>();
                                foreach (Item childsubItem in subItem.Children)
                                {
                                    var childsubMenuItems = new Models.CollapseItem
                                    {
                                        LinkText = Utils.GetValue(childsubItem, BaseTemplates.LinkTemplate.LinkTextFieldId),
                                        ItemSubText = Utils.GetValue(childsubItem, HomePageTemplate.HeaderComponant.ItemSubText),
                                        LinkTarget = Utils.GetValue(childsubItem, BaseTemplates.LinkTemplate.LinkTargetFieldId),
                                        Link = Utils.GetLinkURL(childsubItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                                        ItemLeftIcon = Utils.GetValue(childsubItem, BaseTemplates.CommonItemlist.ItemLeftIcon),
                                        IsActive = Utils.GetBoleanValue(childsubItem, BaseTemplates.ActiveIdTemplate.ActiveFieldId),
                                        GtmData = new GtmData
                                        {
                                            Event = Utils.GetValue(childsubItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                                            Title = Utils.GetValue(childsubItem, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                                            Category = Utils.GetValue(childsubItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                                            SubCategory = Utils.GetValue(childsubItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                                            Label = Utils.GetValue(childsubItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                                            PageType = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                                        },
                                    };
                                    childsubMenuItem.Add(childsubMenuItems);
                                }
                                subNavItem.collapseItems = childsubMenuItem;
                            }
                            subMenuItem.Add(subNavItem);

                        }
                        if (item.Name == "Adani Businesses")
                        {
                            hamburgerData.collapseItems = subMenuItem;
                        }
                        else
                        {
                            hamburgerData.Items = subMenuItem;
                        }
                    }


                    hamburgerMenuData.Add(hamburgerData);
                }

                headerdata.TopbarList = topbarData;
                headerdata.NavData = navData;
                headerdata.HamburgerMenuData = hamburgerMenuData;
            }

            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return headerdata;
        }
    }
}