using Glass.Mapper.Sc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models;
using Project.AAHL.Website.Models.Awards;
using Project.AAHL.Website.Models.Common;
using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Models.OurLeadership;
using Project.AAHL.Website.Templates;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Shell.Framework.Commands.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using static Project.AAHL.Website.Templates.BaseTemplate;
using static Project.AAHL.Website.Templates.BaseTemplate.TitleTemplate;

namespace Project.AAHL.Website.Services.Common
{
    public class HeaderService : IHeaderService
    {

        private readonly ISitecoreService _sitecoreService;

        public HeaderService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public Header GetHeader(Rendering rendering)
        {
            Header header = new Header();
            List<HeaderDetail> headerDetails = new List<HeaderDetail>();
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;
            try
            {

                HeaderDetail headerDetail = new HeaderDetail();
                foreach (Item item in datasource.Children)
                {
                    List<BusinessesMenu> businessesMenu = new List<BusinessesMenu>();
                    List<TopNavigation> topNavigation = new List<TopNavigation>();
                    Brand brand = new Brand();
                    List<PrimaryHeaderMenu> primaryHeaderMenus = new List<PrimaryHeaderMenu>();
                    List<SearchData> search = new List<SearchData>();
                    var BusinessesMenuList = Sitecore.Context.Database.GetItem(BaseTemplate.HeaderTemplate.BusinessesMenuList);
                    foreach (Item BusinessesMenuListitem in BusinessesMenuList.Children)
                    {
                        BusinessesMenu businessesMenu1 = new BusinessesMenu();
                        List<ItemModel> itemModels = new List<ItemModel>();
                        var linkItem1 = Utils.GetLinkItem(BusinessesMenuListitem);
                        businessesMenu1.LinkUrl = linkItem1.LinkUrl;
                        businessesMenu1.Target = linkItem1.Target;
                        businessesMenu1.LinkText = linkItem1.LinkText;
                        foreach (Item BusinessesMenuitem in BusinessesMenuListitem.Children)
                        {
                            ItemModel itemModel = new ItemModel();
                            var linkItems = Utils.GetLinkItem(BusinessesMenuitem);
                            itemModel.Heading = BusinessesMenuitem.Fields[BaseTemplate.HeadingTemplate.HeadingFieldId].Value;
                            itemModel.LinkUrl = linkItems.LinkUrl;
                            itemModel.Target = linkItems.Target;
                            itemModel.LinkText = linkItems.LinkText;
                            itemModels.Add(itemModel);
                        }
                        businessesMenu1.Items = itemModels;
                        businessesMenu.Add(businessesMenu1);
                    }
                    headerDetail.BusinessesMenu = businessesMenu;
                    var TopNavigationList = Sitecore.Context.Database.GetItem(BaseTemplate.HeaderTemplate.TopNavigationList);
                    foreach (Item TopNavigation in TopNavigationList.Children)
                    {
                        TopNavigation topNavigation1 = new TopNavigation();
                        var linkItems = Utils.GetLinkItem(TopNavigation);
                        topNavigation1.LinkUrl = linkItems.LinkUrl;
                        topNavigation1.Target = linkItems.Target;
                        topNavigation1.LinkText = linkItems.LinkText;
                        topNavigation1.Items = null;
                        topNavigation.Add(topNavigation1);
                    }
                    headerDetail.TopNavigation = topNavigation;

                    var Brand = Sitecore.Context.Database.GetItem(BaseTemplate.HeaderTemplate.Brand);
                    brand.ColoredLogo = Utils.GetImageURLByFieldId(Brand, BaseTemplate.HeaderTemplate.ColoredLogo);
                    brand.DefaultLogo = Utils.GetImageURLByFieldId(Brand, BaseTemplate.HeaderTemplate.DefaultLogo);
                    var linkItem = Utils.GetLinkItem(Brand);
                    brand.ColoredLogoIconClass = Brand.Fields[BaseTemplate.HeaderTemplate.ColoredLogoIconClass].Value;
                    brand.DefaultLogoIconClass = Brand.Fields[BaseTemplate.HeaderTemplate.DefaultLogoIconClass].Value;
                    brand.LinkText = linkItem.LinkText;
                    brand.LinkUrl = linkItem.LinkUrl;
                    brand.Target = linkItem.Target;

                    headerDetail.Brand = brand;
                    var PrimaryHeaderMenu = Sitecore.Context.Database.GetItem(BaseTemplate.HeaderTemplate.PrimaryHeaderMenu);
                    foreach (Item primaryMenu in PrimaryHeaderMenu.Children)
                    {
                        PrimaryHeaderMenu primaryHeaderMenu = new PrimaryHeaderMenu();

                        List<primaryHeaderMenusItemModel> primaryHeaderMenusItemModels = new List<primaryHeaderMenusItemModel>();
                        var primaryMenulinkItem = Utils.GetLinkItem(primaryMenu);
                        primaryHeaderMenu.LinkText = primaryMenulinkItem.LinkText;
                        primaryHeaderMenu.LinkUrl = primaryMenulinkItem.LinkUrl;
                        primaryHeaderMenu.Target = primaryMenulinkItem.Target;
                        var PrimaryHeaderMenusCard = BaseTemplate.HeaderTemplate.PrimaryHeaderMenusCard;
                        var PrimaryHeaderMenusitems = BaseTemplate.HeaderTemplate.PrimaryHeaderMenusitems;
                        var PrimaryHeaderMenumessages = BaseTemplate.HeaderTemplate.PrimaryHeaderMenumessages;

                        foreach (Item primaryMenuitem in primaryMenu.Children)
                        {
                            if (primaryMenuitem.TemplateID == PrimaryHeaderMenusitems)
                            {
                                primaryHeaderMenusItemModel primaryHeaderMenusItemModel = new primaryHeaderMenusItemModel();
                                List<ItemModel> itemModels1 = new List<ItemModel>();
                                primaryHeaderMenusItemModel.Heading = primaryMenuitem.Fields[BaseTemplate.HeadingTemplate.HeadingFieldId].Value;
                                foreach (Item primaryMenuitemItem in primaryMenuitem.Children)
                                {
                                    ItemModel itemModel = new ItemModel();
                                    var primaryMenuitemItemItem = Utils.GetLinkItem(primaryMenuitemItem);
                                    itemModel.Heading = primaryMenuitemItem.Fields[BaseTemplate.HeadingTemplate.HeadingFieldId].Value;
                                    itemModel.SubHeading = primaryMenuitemItem.Fields[BaseTemplate.HeadingTemplate.SubHeadingFieldId].Value;
                                    itemModel.LinkText = primaryMenuitemItemItem.LinkText;
                                    itemModel.LinkUrl = primaryMenuitemItemItem.LinkUrl;
                                    itemModel.Target = primaryMenuitemItemItem.Target;
                                    itemModels1.Add(itemModel);
                                }
                                primaryHeaderMenusItemModel.Items = itemModels1;
                                primaryHeaderMenusItemModels.Add(primaryHeaderMenusItemModel);
                            }

                            if (primaryMenuitem.TemplateID == PrimaryHeaderMenusCard)
                            {
                                Card card = new Card();
                                card.ImagePath = Utils.GetImageURLByFieldId(primaryMenuitem, BaseTemplate.ImageSourceTemplate.ImageSourceFieldId);
                                var PrimaryHeaderMenusCardItem = Utils.GetLinkItem(primaryMenuitem);
                                card.LinkText = PrimaryHeaderMenusCardItem.LinkText;
                                card.LinkUrl = PrimaryHeaderMenusCardItem.LinkUrl;
                                card.Target = PrimaryHeaderMenusCardItem.Target;
                                card.IconClass = primaryMenuitem.Fields[BaseTemplate.TitleTemplate.TitleFieldId].Value;
                                card.Bg = primaryMenuitem.Fields[BaseTemplate.TitleTemplate.SubTitleFieldID].Value;
                                primaryHeaderMenu.Card = card;
                            }
                            if (primaryMenuitem.TemplateID == PrimaryHeaderMenumessages)
                            {
                                Messages messages = new Messages();
                                messages.Description = primaryMenuitem.Fields[BaseTemplate.DescriptionTemplate.DescriptionFieldId].Value;
                                messages.Title = primaryMenuitem.Fields[BaseTemplate.TitleTemplate.TitleFieldId].Value;
                                messages.SubTitle = primaryMenuitem.Fields[BaseTemplate.TitleTemplate.SubTitleFieldID].Value;
                                primaryHeaderMenu.Messages = messages;
                            }
                        }
                        primaryHeaderMenu.Items = primaryHeaderMenusItemModels;
                        primaryHeaderMenus.Add(primaryHeaderMenu);
                    }
                    headerDetail.PrimaryHeaderMenus = primaryHeaderMenus;


                    var Search = Sitecore.Context.Database.GetItem(BaseTemplate.HeaderTemplate.Search);
                    SearchData searchData = new SearchData();
                    searchData.Title = Search.Fields[BaseTemplate.TitleTemplate.TitleFieldId].Value;
                    searchData.EmptyMsg = Search.Fields[BaseTemplate.TextTemplate.TextFieldId].Value;
                    List<SearchItemModel> searchItems = new List<SearchItemModel>();
                    foreach (Item Searchitem in Search.Children)
                    {
                        SearchItemModel searchItemModel = new SearchItemModel();
                        searchItemModel.Popular = Utils.GetBoleanValue(Searchitem, BaseTemplate.IsactiveTemplate.IsactiveFieldId);
                        var SearchlinkItem = Utils.GetLinkItem(Searchitem);
                        searchItemModel.LinkText = SearchlinkItem.LinkText;
                        searchItemModel.LinkUrl = SearchlinkItem.LinkUrl;
                        searchItemModel.Target = SearchlinkItem.Target;
                        searchItems.Add(searchItemModel);
                    }
                    searchData.Items = searchItems;
                    search.Add(searchData);

                    headerDetail.Search = search;
                }
                headerDetails.Add(headerDetail);
                header.HeaderDetails = headerDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return header;
        }

    }
}