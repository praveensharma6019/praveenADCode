using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models;
using Project.AmbujaCement.Website.Models.Common;
using Project.AmbujaCement.Website.Models.Home;
using Project.AmbujaCement.Website.Templates;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using Item = Sitecore.Data.Items.Item;

namespace Project.AmbujaCement.Website.Services
{
    public class FooterServices : IFooterServices
    {
        readonly Item _contextItem;
        public FooterServices()
        {
            _contextItem = Sitecore.Context.Item;
        }

        public Footer GetFooter(Rendering rendering)
        {
            Footer footer = new Footer();
            try
            {
                FooterData footerData = new FooterData();
                SocialLink socialLink = new SocialLink();
                Logo logo = new Logo();
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;
                ImageModel imageModel = new ImageModel();
                BottomLink bottomLinks = new BottomLink();
                List<MainNavigation> mainNavigations = new List<MainNavigation>();
                List<CopyRight> copyRights = new List<CopyRight>();
                List<SocialLink> socialLinks = new List<SocialLink>();
                List<BottomLinkItems> linkItems = new List<BottomLinkItems>();
                #region
                var BackgroundImageField = Sitecore.Context.Database.GetItem(FooterTemplate.FooterBackgroundImage.BackgroundImage);

                imageModel.ImageSource = Utils.GetImageURLByFieldId(BackgroundImageField, BaseTemplates.FooterImageSourceTemplate.ImageSourceFieldId);
                imageModel.ImageSourceMobile = Utils.GetImageURLByFieldId(BackgroundImageField, BaseTemplates.FooterImageSourceTemplate.ImageSourceMobileFieldId);
                imageModel.ImageSourceTablet = Utils.GetImageURLByFieldId(BackgroundImageField, BaseTemplates.FooterImageSourceTemplate.ImageSourceTabletFieldId);
                imageModel.ImageAlt = BackgroundImageField.Fields[BaseTemplates.FooterImageSourceTemplate.ImageAltFieldId].Value;

                footerData.BackgroundImage = imageModel;
                #endregion
                var BottomLinks = Sitecore.Context.Database.GetItem(FooterTemplate.FooterBottomLinks.BottomLinks);
                
                foreach (Item item in BottomLinks.Children)
                {
                    BottomLinkItems bottom = new BottomLinkItems();
                    bottom.LinkText = item.Fields[BaseTemplates.LinkTemplate.LinkTextFieldId].Value;
                    bottom.Description = item.Fields[BaseTemplates.DescriptionTemplate.DescriptionFieldId].Value;
                    bottom.Link = Utils.GetLinkURL(item.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                    bottom.IconName = item.Fields[BaseTemplates.IConItemPathTemplate.IconnameFieldId].Value;
                    bottom.LinkTarget = item.Fields[BaseTemplates.LinkTemplate.LinkTargetFieldId].Value;
                    bottom.GtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Title = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                        Label = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    linkItems.Add(bottom);
                }
                bottomLinks.links = linkItems;
                footerData.bottomLinks = bottomLinks;
                var MainNavigation = Sitecore.Context.Database.GetItem(FooterTemplate.FooterDataMainNavigations.MainNavigations);

                foreach (Item item in MainNavigation.Children)
                {
                    List<ItemModel> items = new List<ItemModel>();
                    MainNavigation mainNavigation = new MainNavigation();
                    mainNavigation.Heading = item.Fields[BaseTemplates.HeadingTemplate.HeadingFieldId].Value;
                    foreach (Item item1 in item.Children)
                    {
                        ItemModel itemModel = new ItemModel();
                        itemModel.LinkTarget = item1.Fields[BaseTemplates.LinkTemplate.LinkTargetFieldId].Value;
                        itemModel.Link = Utils.GetLinkURL(item1.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                        itemModel.LinkText = item1.Fields[BaseTemplates.LinkTemplate.LinkTextFieldId].Value;
                        itemModel.GtmData = new GtmDataModel
                        {
                            Event = Utils.GetValue(item1, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(item1, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(item1, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Title = Utils.GetValue(item1, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                            Label = Utils.GetValue(item1, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        };
                        items.Add(itemModel);
                    }
                    mainNavigation.Items = items;
                    mainNavigations.Add(mainNavigation);
                }
                footerData.MainNavigations = mainNavigations;

                var BuCopyright = Sitecore.Context.Database.GetItem(FooterTemplate.FooterDataBuCopyright.BuCopyright);

                footerData.BuCopyright = BuCopyright.Fields[BaseTemplates.HeadingTemplate.HeadingFieldId].Value;

                var SocialLinks = Sitecore.Context.Database.GetItem(FooterTemplate.FooterDataSocialLinks.SocialLinks);

                foreach (Item item1 in SocialLinks.Children)
                {
                    socialLink.Heading = item1.Fields[BaseTemplates.HeadingTemplate.HeadingFieldId].Value;
                    List<ItemModel> itemdata = new List<ItemModel>();
                    foreach (Item item in item1.Children)
                    {
                        ItemModel itemModel = new ItemModel();
                        itemModel.LinkTarget = item.Fields[BaseTemplates.LinkTemplate.LinkTargetFieldId].Value;
                        itemModel.Link = Utils.GetLinkURL(item.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                        itemModel.LinkText = item.Fields[BaseTemplates.LinkTemplate.LinkTextFieldId].Value;
                        itemModel.Itemicon = item.Fields[BaseTemplates.IConItemPathTemplate.IconalttextFieldId].Value;
                        itemModel.GtmData = new GtmDataModel
                        {
                            Event = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Title = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                            Label = Utils.GetValue(item, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        };
                        itemdata.Add(itemModel);
                    }
                    socialLink.Items = itemdata;
                    socialLinks.Add(socialLink);
                }
                footerData.SocialLinks = socialLinks;

                var SocialLogo = Sitecore.Context.Database.GetItem(FooterTemplate.FooterDatalogo.Logo);

                logo.ImageTitle = SocialLogo.Fields[FooterTemplate.FooterDatalogo.ImageTitle].Value;
                logo.ImageAlt = SocialLogo.Fields[BaseTemplates.ImageSourceTemplate.ImageAltFieldId].Value;
                logo.ImageSource = Utils.GetImageURLByFieldId(SocialLogo, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId);
                logo.ImageSourceMobile = Utils.GetImageURLByFieldId(SocialLogo, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId);
                logo.ImageSourceTablet = Utils.GetImageURLByFieldId(SocialLogo, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId);

                footerData.Logo = logo;


                var SocialCopyRight = Sitecore.Context.Database.GetItem(FooterTemplate.FooterDataCopyRight.CopyRight);

                foreach (Item item in SocialCopyRight.Children)
                {
                    CopyRight copyRight = new CopyRight();
                    copyRight.Heading = item.Fields[BaseTemplates.HeadingTemplate.HeadingFieldId].Value;
                    copyRight.SubHeading = item.Fields[BaseTemplates.SubHeadingTemplate.SubHeadingFieldId].Value;
                    List<ItemModel> itemdata = new List<ItemModel>();
                    foreach (Item item1 in item.Children)
                    {
                        ItemModel itemModel = new ItemModel();
                        itemModel.LinkTarget = item1.Fields[BaseTemplates.LinkTemplate.LinkTargetFieldId].Value;
                        itemModel.Link = Utils.GetLinkURL(item1.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);
                        itemModel.LinkText = item1.Fields[BaseTemplates.LinkTemplate.LinkTextFieldId].Value;
                        itemModel.Itemicon = item1.Fields[BaseTemplates.IConItemPathTemplate.IconalttextFieldId].Value;
                        itemModel.GtmData = new GtmDataModel
                        {
                            Event = Utils.GetValue(item1, BaseTemplates.GTMTemplate.GtmEventFieldId),
                            Category = Utils.GetValue(item1, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                            Sub_category = Utils.GetValue(item1, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                            Title = Utils.GetValue(item1, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                            Label = Utils.GetValue(item1, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                        };
                        itemdata.Add(itemModel);
                    }
                    copyRight.Items = itemdata;
                    copyRights.Add(copyRight);
                }
                footerData.CopyRight = copyRights;

                footer.FooterData = footerData;
                Item tabList = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Tabs"));
                Item navDataList = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Nav Data"));
                Item Productlist = datasource.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Products"));

                if (tabList == null) return null;

                StickyNavData StickyNavData = new StickyNavData
                {

                    Tabs = new List<Tab>(),
                    NavData = new NavData(),
                    Products = new List<Product>()
                };
                var defaultActiveTab = Sitecore.Context.Database.GetItem(BaseTemplates.LabelTemplate.defaultActiveTemplate);
                StickyNavData.defaultActiveTab = defaultActiveTab.Fields[BaseTemplates.LabelTemplate.defaultActiveTab].Value;
                //StickyNavData.defaultActiveTab = Utils.GetValue(BaseTemplates.LabelTemplate.defaultActiveTab);
                foreach (Item tabItem in tabList.Children)
                {
                    Tab tabListItem = new Tab();
                    tabListItem.Label = Utils.GetValue(tabItem, BaseTemplates.LabelTemplate.LabelFieldId);
                    tabListItem.ActiveId = Utils.GetValue(tabItem, BaseTemplates.ActiveIdTemplate.ActiveFieldId);
                    tabListItem.Icon = Utils.GetValue(tabItem, BaseTemplates.CommonItemlist.IconID);
                    tabListItem.LinkText = Utils.GetValue(tabItem, BaseTemplates.LinkTemplate.LinkTextFieldId);
                    tabListItem.LinkTarget = Utils.GetValue(tabItem, BaseTemplates.LinkTemplate.LinkTargetFieldId);
                    tabListItem.Link = Utils.GetLinkURL(tabItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]);

                    tabListItem.IsOffcanvas = Utils.GetBoleanValue(tabItem, BaseTemplates.IsOffcanvasIdTemplate.IsOffcanvasFieldId);


                    GtmDataModel gtmData = new GtmDataModel
                    {
                        Event = Utils.GetValue(tabItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Title = Utils.GetValue(tabItem, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                        Category = Utils.GetValue(tabItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(tabItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(tabItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    };
                    tabListItem.GtmData = gtmData;

                    StickyNavData.Tabs.Add(tabListItem);
                }

                if (navDataList == null) return null;
                foreach (Item item in navDataList.Children)
                {

                    var NavDataitem = new NavData
                    {
                        Heading = Utils.GetValue(item, BaseTemplates.HeadingTemplate.HeadingFieldId),
                        Description = Utils.GetValue(item, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                        HeadingIcon = Utils.GetValue(item, HomePageTemplate.HeadingIconField.HeadingiconiD),

                    };
                    OtherData otherdataitem = new OtherData();


                    if (item.HasChildren)
                    {
                        var subMenuItem = new List<Models.Common.OtherData>();

                        foreach (Item subItem in item.Children)
                        {
                            otherdataitem.Heading = Utils.GetValue(subItem, BaseTemplates.HeadingTemplate.HeadingFieldId);
                            var childsubMenuItem = new List<Models.Common.ItemModel>();
                            if (subItem.HasChildren)
                            {

                                foreach (Item childsubItem in subItem.Children)
                                {
                                    var ItemModelData = new Models.Common.ItemModel
                                    {
                                        LinkText = Utils.GetValue(childsubItem, BaseTemplates.LinkTemplate.LinkTextFieldId),
                                        IconName = Utils.GetValue(childsubItem, BaseTemplates.CommonItemlist.IconnamefieldID),
                                        Label = Utils.GetValue(childsubItem, BaseTemplates.LabelTemplate.LabelFieldId),
                                        LinkTarget = Utils.GetValue(childsubItem, BaseTemplates.LinkTemplate.LinkTargetFieldId),
                                        Link = Utils.GetLinkURL(childsubItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),

                                        GtmData = new GtmDataModel
                                        {
                                            Event = Utils.GetValue(childsubItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                                            Title = Utils.GetValue(childsubItem, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                                            Category = Utils.GetValue(childsubItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                                            Sub_category = Utils.GetValue(childsubItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                                            Label = Utils.GetValue(childsubItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                                            Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                                        },
                                    };
                                    childsubMenuItem.Add(ItemModelData);
                                }

                            }
                            otherdataitem.Items = childsubMenuItem;
                        }
                    }
                    NavDataitem.OtherData = otherdataitem;

                    StickyNavData.NavData = NavDataitem;
                }

                if (Productlist == null) return null;
                foreach (Item item in Productlist.Children)
                {
                    var navItem = new Product
                    {
                        ProductLabel = Utils.GetValue(item, HomecategoriesListTemplate.CategoriesList.ProductLabel),
                        ImageAlt = Utils.GetValue(item, HomecategoriesListTemplate.CategoriesList.ImageAlt),
                        ActiveId = Utils.GetValue(item, BaseTemplates.ActiveIdTemplate.ActiveFieldId),
                        ImageSource = Utils.GetImageURLByFieldId(item, HomecategoriesListTemplate.CategoriesList.ImageSource),

                    };

                    if (item.HasChildren)
                    {
                        var subMenuItem = new List<Models.Common.SubProduct>();

                        foreach (Item subItem in item.Children)
                        {
                            var subNavItem = new Models.Common.SubProduct
                            {
                                LinkText = Utils.GetValue(subItem, BaseTemplates.LinkTemplate.LinkTextFieldId),
                                LinkTarget = Utils.GetValue(subItem, BaseTemplates.LinkTemplate.LinkTargetFieldId),
                                Link = Utils.GetLinkURL(subItem.Fields[BaseTemplates.LinkTemplate.LinkFieldId]),
                                GtmData = new GtmDataModel
                                {
                                    Event = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                                    Title = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmTitleFieldId),
                                    Category = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                                    Sub_category = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                                    Label = Utils.GetValue(subItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                                    Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                                },


                            };
                            subMenuItem.Add(subNavItem);


                        }
                        navItem.SubProduct = subMenuItem;
                    }
                    StickyNavData.Products.Add(navItem);

                }
                footer.StickyNavData = StickyNavData;


            }

            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return footer;
        }
    }
}