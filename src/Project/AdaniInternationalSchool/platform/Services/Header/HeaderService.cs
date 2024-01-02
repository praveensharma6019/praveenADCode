using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.AdaniInternationalSchool.Website.Services.Header
{
    public class HeaderService : IHeaderService
    {
        readonly Item _contextItem;
        public HeaderService()
        {
            _contextItem = Sitecore.Context.Item;
        }

        public HeaderModel Render(Rendering rendering)
        {
            var dsItem = Utils.GetRenderingDatasource(rendering);
            if (dsItem == null) return null;

            try
            {
                var headerModel = new HeaderModel
                {
                    LogoSrc = Utils.GetImageURLByFieldId(dsItem, HeaderTemplate.Fields.LogoSrc),
                    LogoSrcSmall = Utils.GetImageURLByFieldId(dsItem, HeaderTemplate.Fields.LogoSrcSmall),
                    LogoSrcHamburger = Utils.GetImageURLByFieldId(dsItem, HeaderTemplate.Fields.LogoSrcHamburger),
                    HamburgerBG = Utils.GetImageURLByFieldId(dsItem, HeaderTemplate.Fields.HamburgerBG),
                    LogoAlt = Utils.GetValue(dsItem, HeaderTemplate.Fields.LogoAlt),
                    Url = _contextItem.Paths.ContentPath.EndsWith("/Home", StringComparison.CurrentCultureIgnoreCase) ? "" : "/",
                    GtmData = new GtmDataModel()
                    {
                        Event = Utils.GetValue(dsItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(dsItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(dsItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item),
                        Title = Utils.GetValue(dsItem, BaseTemplates.GTMTemplate.GtmTitleFieldId)
                    },

                };

                PopulateHeaderNavigation(headerModel, dsItem);
                PopulateHeaderContact(headerModel, dsItem);
                PopulateHeaderSocial(headerModel, dsItem);

                return headerModel;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return null;
        }

        private void PopulateHeaderSocial(HeaderModel headerModel, Item item)
        {
            Item social = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Social"));

            if (social != null)
            {
                foreach (Item socialItem in social.Children)
                {
                    var linkItem = ServiceHelper.GetLinkItem(socialItem);
                    ServiceHelper.FillLinkItemGtmValues(linkItem, socialItem);
                    headerModel.Social.Add(linkItem);
                }
            }
        }



        private void PopulateHeaderContact(HeaderModel headerModel, Item item)
        {
            Item contact = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Contact"));

            if (contact != null)
            {
                foreach (Item contactItem in contact.Children)
                {
                    headerModel.Contact
                        .Add(new HeaderContactModel
                        {
                            Label = Utils.GetValue(contactItem, HeaderTemplate.Contact.Fields.Label, contactItem.Name),
                            Url = Utils.GetValue(contactItem, HeaderTemplate.Contact.Fields.Url),
                            Type = Utils.GetValue(contactItem, HeaderTemplate.Contact.Fields.Type),
                            GtmData = new GtmDataModel()
                            {
                                Event = Utils.GetValue(contactItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                                Category = Utils.GetValue(contactItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                                Sub_category = Utils.GetValue(contactItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                                Label = Utils.GetValue(contactItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                                Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                            },
                        });
                }
            }
        }

        private void PopulateHeaderNavigation(HeaderModel headerModel, Item item)
        {
            Item navigation = item.Children.FirstOrDefault(x => Utils.CompareIgnoreCase(x.Name, "Navigation"));

            if (navigation != null)
            {
                foreach (Item navItem in navigation.Children)
                {

                    LinkField lf = navItem.Fields[HeaderTemplate.Navigation.Fields.Url];

                    var targetItemPath = Utils.GetValue(navItem, BaseTemplates.TargetItemPathTemplate.TargetItemPathFieldId);
                    bool isActive = targetItemPath.EndsWith(_contextItem.Paths.ContentPath, StringComparison.CurrentCultureIgnoreCase);

                    //get submenu items
                    var submenuItems = GetSubmenuItems(navItem, ref isActive);

                    headerModel.Navigation
                        .Add(new NavigationModel
                        {
                            Label = Utils.GetValue(navItem, HeaderTemplate.Navigation.Fields.Label, navItem.Name),
                            Target = lf.Target,
                            Url = Utils.GetLinkURL(lf),
                            IsActive = isActive,
                            IsHighLighted = Utils.GetBoleanValue(navItem, HeaderTemplate.Navigation.Fields.IsHighLighted),
                            HighlightLabel = Utils.GetValue(navItem, HeaderTemplate.Navigation.Fields.HighlightLabel),
                            GtmData = new GtmDataModel()
                            {
                                Event = Utils.GetValue(navItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                                Category = Utils.GetValue(navItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                                Sub_category = Utils.GetValue(navItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                                Label = Utils.GetValue(navItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                                Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                            },
                            SubMenu = submenuItems
                        });
                }
            }
        }

        private List<NavMenuItemModel> GetSubmenuItems(Item item, ref bool active)
        {
            var menuItemList = new List<NavMenuItemModel>();

            foreach (Item menuItem in item.Children)
            {
                LinkField lf = menuItem.Fields[BaseTemplates.CtaTemplate.CtaLinkFieldId];

                var targetItemPath = Utils.GetValue(menuItem, BaseTemplates.TargetItemPathTemplate.TargetItemPathFieldId);
                if (!active)
                {
                    active = targetItemPath.EndsWith(_contextItem.Paths.ContentPath, StringComparison.CurrentCultureIgnoreCase);
                }

                menuItemList.Add(new NavMenuItemModel
                {
                    Label = Utils.GetValue(menuItem, BaseTemplates.CtaTemplate.CtaTextFieldId, menuItem.Name),
                    Target = lf.Target,
                    Url = Utils.GetLinkURL(lf),
                    GtmData = new GtmDataModel()
                    {
                        Event = Utils.GetValue(menuItem, BaseTemplates.GTMTemplate.GtmEventFieldId),
                        Category = Utils.GetValue(menuItem, BaseTemplates.GTMTemplate.GtmCategoryFieldId),
                        Sub_category = Utils.GetValue(menuItem, BaseTemplates.GTMTemplate.GtmSubCategoryFieldId),
                        Label = Utils.GetValue(menuItem, BaseTemplates.GTMTemplate.GtmLabelFieldId),
                        Page_type = Utils.GetGtmPageTypeValue(Sitecore.Context.Item)
                    },
                    SubMenu = GetSubmenuItems(menuItem, ref active)

                });
            }

            return menuItemList;
        }
    }
}