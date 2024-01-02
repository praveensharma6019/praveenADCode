using Adani.SuperApp.Realty.Feature.Sitemap.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using static Adani.SuperApp.Realty.Feature.Sitemap.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Sitemap.Platform.Services
{
    public class SitemapRootResolverService : ISitemapRootResolverService
    {

        private readonly ILogRepository _logRepository;
        public SitemapRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }

        #region SitemapLink

        public SitemapData GetSitemapDataItemList(Rendering rendering)
        {
            SitemapData sitemapData = new SitemapData();
            try
            {
                sitemapData.PageTitle = GetSitemapDataItem(rendering).Item1;
                sitemapData.links = GetSitemapDataItem(rendering).Item2;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return sitemapData;
        }
        public List<DynamicSitemap> GetDynamicSitemapData(Rendering rendering)
        {
            List<DynamicSitemap> sitemapData = new List<DynamicSitemap>();
            try
            {
                List<Item> siteMapItemList = new List<Item>();
                var homeItem = Sitecore.Context.Database.GetItem(Templates.Home.ItemID);
                var commonItem = Sitecore.Context.Database.GetItem(Templates.Commondata.ItemID);
                var domain = !string.IsNullOrEmpty(commonItem.Fields[Templates.Commondata.Fields.SiteDomain].Value) ? commonItem.Fields[Templates.Commondata.Fields.SiteDomain].Value : "";
                var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Templates.Commondata.Fields.blogLinkId].Value.ToString()) ?
                            commonItem.Fields[Templates.Commondata.Fields.blogLinkId].Value.ToString() : "";
                var homeChildern = homeItem.Axes.GetDescendants().ToList();
                foreach (var sitemapItem in homeChildern)
                {
                    CheckboxField checkboxField = sitemapItem.Fields[Templates.SEOData.IncludedInSItemap];
                    if (checkboxField.Checked)
                    {
                        siteMapItemList.Add(sitemapItem);
                    }
                }
                siteMapItemList.Add(homeItem);
                var latestUpdatedItem = siteMapItemList.OrderByDescending(x => x.Statistics.Created).FirstOrDefault();
                foreach (var item in siteMapItemList.OrderByDescending(x => x.Name))
                {
                    DynamicSitemap dynamicSitemap = new DynamicSitemap();
                    var itemUrl = !string.IsNullOrEmpty(item?.Fields["Slug"]?.Value) ? item?.Fields["Slug"].Value : LinkManager.GetItemUrl(item);
                    dynamicSitemap.url = itemUrl.ToLower().Contains("/realty") == false ? domain + realtyLink + itemUrl : domain + itemUrl;
                    if (item.TemplateID == Templates.SEOData.TemplateIdClass.HomePagetemplateID)
                    {
                        dynamicSitemap.priority = "1.00";
                        dynamicSitemap.descPriority = 1.0;
                    }
                    else if (item.TemplateID == Templates.SEOData.TemplateIdClass.Commercial || item.TemplateID == Templates.SEOData.TemplateIdClass.Residential || item.TemplateID == Templates.SEOData.TemplateIdClass.Club || item.TemplateID == Templates.SEOData.TemplateIdClass.BLogCategory)
                    {
                        dynamicSitemap.priority = "0.80";
                        dynamicSitemap.descPriority = 0.8;
                    }
                    else if (item.TemplateID == Templates.SEOData.TemplateIdClass.BlogDetail)
                    {
                        var insideRealtyLink = realtyLink + "/blogs/";
                        dynamicSitemap.url = itemUrl.ToLower().Contains("/realty") == false ? domain + insideRealtyLink + itemUrl : domain + itemUrl;
                        dynamicSitemap.priority = "0.70";
                        dynamicSitemap.descPriority = 0.7;
                    }
                    else
                    {
                        dynamicSitemap.priority = "0.90";
                        dynamicSitemap.descPriority = 0.9;
                    }
                    _logRepository.Info(string.Format("Latest Updated Item:{0} ", latestUpdatedItem.Name.ToString()), this);
                    dynamicSitemap.url = dynamicSitemap.url.ToLower();
                    dynamicSitemap.lastmod = latestUpdatedItem.Statistics.Created.ToString("yyyy-MM-ddTHH:mm:sszzz");
                    sitemapData.Add(dynamicSitemap);
                    sitemapData = sitemapData.OrderByDescending(x => x.descPriority).ToList();
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" SitemapRootResolverService GetDynamicSitemapData gives -> " + ex.Message);
            }
            return sitemapData;
        }

        public Tuple<string, List<Object>> GetSitemapDataItem(Rendering rendering)
        {
            List<Object> sitemapDataItemList = new List<Object>();
            List<Item> cityData = new List<Item>();
            string Title = string.Empty;
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
                SitemapDataItem sitemapDataItem;
                Title = datasource.Fields[Templates.ITitle.FieldsName.Title].Value != null ? datasource.Fields[Templates.ITitle.FieldsName.Title].Value : "";
                if (datasource.TemplateID == Templates.SitemapFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {

                    List<Item> children = datasource.Children.Where(x => x.TemplateID.Equals(Templates.SitemapItem.TemplateID)
                                                || x.TemplateID.Equals(Templates.SitemapOtherLinkFolder.TemplateID)).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Item item in children)
                        {
                            sitemapDataItem = new SitemapDataItem();
                            if (item.TemplateID == Templates.SitemapItem.TemplateID)
                            {
                                sitemapDataItem.SitemapHeading = item.Fields[Templates.SitemapItem.Fields.FieldsName.SitemapTitle].Value != null ? item.Fields[Templates.SitemapItem.Fields.FieldsName.SitemapTitle].Value : "";
                                sitemapDataItem.SitemapMD = item.Fields[Templates.SitemapItem.Fields.FieldsName.SitemapPriority].Value != null ? item.Fields[Templates.SitemapItem.Fields.FieldsName.SitemapPriority].Value : "";


                                var multiListForItem = item.GetMultiListValueItem(Templates.SitemapItem.Fields.FieldsID.IncludeItemInSitemap);
                                if (multiListForItem != null && multiListForItem.Count() > 0)
                                {
                                    foreach (var selectedSitemapItem in multiListForItem)
                                    {
                                        List<SitemapInnerDataItem> siteMapInnerItem = new List<SitemapInnerDataItem>();
                                        if (selectedSitemapItem.ID == SEO.TemplatId)
                                        {
                                            GetSEOSitemap(selectedSitemapItem);
                                        }
                                        else
                                        {
                                            var SitemapkeysItem = selectedSitemapItem.GetChildren();
                                            if (SitemapkeysItem != null && SitemapkeysItem.Count() > 0)
                                            {

                                                foreach (Item innerPageItem in SitemapkeysItem)
                                                {
                                                    if (DoesSitecoreItemHavePresentation(innerPageItem))
                                                    {
                                                        IEnumerable<string> cityNameData = from cityName in cityData select cityName.Name;
                                                        if (!cityNameData.Contains(innerPageItem.Name))
                                                        {
                                                            cityData.Add(innerPageItem);
                                                        }
                                                        SitemapInnerDataItem innerObjItem = new SitemapInnerDataItem();
                                                        innerObjItem.SitemapInnerTitle = innerPageItem.Name;
                                                        innerObjItem.link = GetLinkURLbyField(innerPageItem);
                                                        innerObjItem.target = Helper.GetLinkURLTargetSpace(innerPageItem, "link") != null ?
                                                                        Helper.GetLinkURLTargetSpace(innerPageItem, "link") : "";
                                                        //}
                                                        var KeyPages = innerPageItem.GetChildren();
                                                        List<SitemapInnerDataItemKeyPage> KeyPagesItems = new List<SitemapInnerDataItemKeyPage>();
                                                        foreach (Item selectedSitemapKeyItem in KeyPages)
                                                        {
                                                            SitemapInnerDataItemKeyPage innerKeyObj = new SitemapInnerDataItemKeyPage();
                                                            if (DoesSitecoreItemHavePresentation(selectedSitemapKeyItem))
                                                            {
                                                                innerKeyObj.SitemapInnerDataItemkeyPage = selectedSitemapKeyItem.Fields["Title"] != null ? selectedSitemapKeyItem.Fields["Title"].Value : selectedSitemapKeyItem.Name;
                                                                var commonItem = Sitecore.Context.Database.GetItem(Templates.Commondata.ItemID);
                                                                var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Templates.Commondata.Fields.blogLinkId].Value.ToString()) ?
                                            commonItem.Fields[Templates.Commondata.Fields.blogLinkId].Value.ToString() : "";
                                                                innerKeyObj.Link = selectedSitemapKeyItem.TemplateID.ToString().Equals("{778D2F65-2BAC-4F22-91AB-19BD6E88FC5F}") ? realtyLink : GetLinkURLbyField(selectedSitemapKeyItem);
                                                                innerKeyObj.target = Helper.GetLinkURLTargetSpace(selectedSitemapKeyItem, "link") != null ?
                                                        Helper.GetLinkURLTargetSpace(selectedSitemapKeyItem, "link") : "";
                                                                KeyPagesItems.Add(innerKeyObj);
                                                            }
                                                        }
                                                        innerObjItem.SitemapInnerkeysPage = KeyPagesItems;
                                                        siteMapInnerItem.Add(innerObjItem);
                                                        sitemapDataItem.SitemapItems = siteMapInnerItem;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (item.ID == Templates.SitemapOtherLinkFolder.ItemID)
                            {
                                var otherSitemapLinksItem = item.GetChildren().Where(x => x.TemplateID.Equals(Templates.SitemapOtherLink.TemplateID)).ToList();
                                List<SitemapInnerDataItem> siteMapInnerItem = new List<SitemapInnerDataItem>();
                                foreach (Item othersLinkSitemapItem in otherSitemapLinksItem)
                                {
                                    sitemapDataItem.SitemapHeading = item.Fields[Templates.SitemapOtherLinkFolder.Fields.FieldsName.SitemapTitle].Value != null ? item.Fields[Templates.SitemapOtherLinkFolder.Fields.FieldsName.SitemapTitle].Value : "";
                                    sitemapDataItem.SitemapMD = item.Fields[Templates.SitemapOtherLinkFolder.Fields.FieldsName.SitemapPriority].Value != null ? item.Fields[Templates.SitemapOtherLinkFolder.Fields.FieldsName.SitemapPriority].Value : "";

                                    SitemapInnerDataItem innerObjItem = new SitemapInnerDataItem();
                                    innerObjItem.SitemapInnerTitle =/* !string.IsNullOrEmpty(othersLinkSitemapItem.Fields["Title"].ToString()) ? othersLinkSitemapItem.Fields["Title"].Value : */othersLinkSitemapItem.Name;
                                    innerObjItem.link = GetLinkURLbyField(othersLinkSitemapItem);
                                    innerObjItem.target = Helper.GetLinkURLTargetSpace(othersLinkSitemapItem, "link") != null ?
                                                    Helper.GetLinkURLTargetSpace(othersLinkSitemapItem, "link") : "";
                                    var multiListForItem = othersLinkSitemapItem.GetMultiListValueItem(Templates.SitemapOtherLink.Fields.FieldsID.SitemapOtherLinkItems);
                                    if (multiListForItem != null && multiListForItem.Count() > 0)
                                    {
                                        List<SitemapInnerDataItemKeyPage> KeyPagesItems = new List<SitemapInnerDataItemKeyPage>();
                                        foreach (var selectedOtherLinkSitemapItem in multiListForItem)
                                        {

                                            SitemapInnerDataItemKeyPage innerKeyObj = new SitemapInnerDataItemKeyPage();

                                            innerKeyObj.SitemapInnerDataItemkeyPage = /*selectedOtherLinkSitemapItem.Fields["Title"] != null ? selectedOtherLinkSitemapItem.Fields["Title"].Value :*/ !string.IsNullOrEmpty(selectedOtherLinkSitemapItem.DisplayName) ? selectedOtherLinkSitemapItem.DisplayName : selectedOtherLinkSitemapItem.Name;
                                            innerKeyObj.Link = selectedOtherLinkSitemapItem.TemplateID.ToString().Equals("{778D2F65-2BAC-4F22-91AB-19BD6E88FC5F}") ? "/" : GetLinkURLbyField(selectedOtherLinkSitemapItem);
                                            innerKeyObj.target = Helper.GetLinkURLTargetSpace(selectedOtherLinkSitemapItem, "link") != null ?
                                                    Helper.GetLinkURLTargetSpace(selectedOtherLinkSitemapItem, "link") : "";
                                            KeyPagesItems.Add(innerKeyObj);
                                        }
                                        sitemapDataItem.SitemapItems = siteMapInnerItem;
                                        innerObjItem.SitemapInnerkeysPage = KeyPagesItems;
                                    }
                                    siteMapInnerItem.Add(innerObjItem);
                                }
                            }
                            sitemapDataItemList.Add(sitemapDataItem);
                        }

                    }

                }
                void GetSEOSitemap(Item item)
                {
                    List<SitemapInnerDataItem> siteMapInnerItem = new List<SitemapInnerDataItem>();
                    var SitemapkeysItem = cityData;
                    if (SitemapkeysItem != null && SitemapkeysItem.Count() > 0)
                    {

                        foreach (Item innerPageItem in SitemapkeysItem)
                        {
                            if (DoesSitecoreItemHavePresentation(innerPageItem))
                            {
                                SitemapInnerDataItem innerObjItem = new SitemapInnerDataItem();
                                
                                innerObjItem.SitemapInnerTitle = innerPageItem.Name;
                                innerObjItem.link = GetLinkURLbyField(innerPageItem);
                                innerObjItem.target = Helper.GetLinkURLTargetSpace(innerPageItem, "link") != null ?
                                                Helper.GetLinkURLTargetSpace(innerPageItem, "link") : "";
                                var KeyPages = item.GetChildren();
                                List<Item> SeoCityFiltered = new List<Item>();
                                foreach (Item SeoPage in KeyPages)
                                {
                                    var cityName = SeoPage.Fields[SEO.Page.CityName].Value != null ? SeoPage.Fields[SEO.Page.CityName].Value : "";
                                    if(cityName == innerPageItem.Name && SeoPage.TemplateID == SEO.Page.PageID)
                                    {
                                        SeoCityFiltered.Add(SeoPage);
                                    }
                                }
                                if (SeoCityFiltered != null && SeoCityFiltered.Count() > 0)
                                {
                                    List<SitemapInnerDataItemKeyPage> KeyPagesItems = new List<SitemapInnerDataItemKeyPage>();
                                    foreach (Item selectedSitemapKeyItem in SeoCityFiltered)
                                    {
                                        SitemapInnerDataItemKeyPage innerKeyObj = new SitemapInnerDataItemKeyPage();
                                        if (DoesSitecoreItemHavePresentation(selectedSitemapKeyItem))
                                        {
                                            innerKeyObj.SitemapInnerDataItemkeyPage = selectedSitemapKeyItem.Fields["Title"] != null ? selectedSitemapKeyItem.Fields["Title"].Value : selectedSitemapKeyItem.Name;
                                            var commonItem = Sitecore.Context.Database.GetItem(Templates.Commondata.ItemID);
                                            var realtyLink = !string.IsNullOrEmpty(commonItem.Fields[Templates.Commondata.Fields.blogLinkId].Value.ToString()) ?
                        commonItem.Fields[Templates.Commondata.Fields.blogLinkId].Value.ToString() : "";
                                            innerKeyObj.Link = selectedSitemapKeyItem.TemplateID.ToString().Equals("{778D2F65-2BAC-4F22-91AB-19BD6E88FC5F}") ? realtyLink : GetLinkURLbyField(selectedSitemapKeyItem);
                                            innerKeyObj.target = Helper.GetLinkURLTargetSpace(selectedSitemapKeyItem, "link") != null ?
                                    Helper.GetLinkURLTargetSpace(selectedSitemapKeyItem, "link") : "";
                                            KeyPagesItems.Add(innerKeyObj);
                                        }
                                    }
                                    innerObjItem.SitemapInnerkeysPage = KeyPagesItems;
                                    siteMapInnerItem.Add(innerObjItem);
                                    sitemapDataItem.SitemapItems = siteMapInnerItem;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return Tuple.Create(Title, sitemapDataItemList);
        }
        

        public string GetLinkURLbyField(Item item)
        {
            string linkURL = string.Empty;
            string strSitedomain = string.Empty;
            var CItem = Sitecore.Context.Database.GetItem(Templates.Commondata.ItemID);
            strSitedomain = CItem != null ? CItem.Fields["Site Domain"].Value : string.Empty;
            try
            {
                LinkField linkField = item.Fields["link"];
                if (linkField != null)
                {

                    if (!String.IsNullOrEmpty(linkField.ToString()))
                    {
                        switch (linkField.LinkType)
                        {
                            case "internal":

                            case "mailto":
                            case "anchor":
                            case "javascript":
                                linkURL = linkField.TargetItem != null ? Helper.GetUrlDomain() + LinkManager.GetItemUrl(linkField.TargetItem) : string.Empty;
                                break;
                            case "external":
                                linkURL = linkField.Url;
                                break;
                            case "media":
                                Sitecore.Data.Items.MediaItem media = new Sitecore.Data.Items.MediaItem(linkField.TargetItem);
                                linkURL = Helper.GetSitecoreDomain() + MediaManager.GetMediaUrl(media);
                                break;
                            case "":
                                break;
                            default:
                                //logger
                                string message = String.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);
                                break;
                        }
                    }
                    if (!(linkURL.Contains("http") || linkURL.Contains("#") || linkURL.Contains("tel:") || linkURL.Contains("mailto:")))
                    {
                        linkURL = strSitedomain + linkURL;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, new object());
            }

            return linkURL;
        }


        #endregion

        #region Configuration

        public ConfigurationData GetConfigurationItemList(Rendering rendering)
        {
            ConfigurationData configurationData = new ConfigurationData();
            try
            {

                configurationData.items = GetConfigurationDataItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return configurationData;
        }

        public List<Object> GetConfigurationDataItem(Rendering rendering)
        {
            List<Object> configurationDataItemList = new List<Object>();
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
                ConfigurationDataItem configurationDataItem;

                if (datasource.TemplateID == Templates.ConfigurationFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {

                    List<Item> children = datasource.Children.Where(x => x.TemplateID.Equals(Templates.Configuration.TemplateID)).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Item item in children)
                        {
                            configurationDataItem = new ConfigurationDataItem();
                            configurationDataItem.ConfigurationInnerTitle = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";
                            var multiListForItem = item.GetMultiListValueItem(Templates.Configuration.Fields.FieldsID.ConfigurationItem);
                            if (multiListForItem != null && multiListForItem.Count() > 0)
                            {
                                foreach (var selectedConfigItem in multiListForItem)
                                {
                                    List<ConfigurationInnerDataItem> configurationInnerItem = new List<ConfigurationInnerDataItem>();
                                    List<Item> configItemchildren = selectedConfigItem.Children.ToList();
                                    if (configItemchildren != null && configItemchildren.Count > 0)
                                    {
                                        foreach (Item configChildrenitem in configItemchildren)
                                        {
                                            if (DoesSitecoreItemHavePresentation(configChildrenitem))
                                            {
                                                ConfigurationInnerDataItem innerObjItem = new ConfigurationInnerDataItem();
                                                innerObjItem.ConfigurationInnerkeyword = configChildrenitem.Name;
                                                innerObjItem.ConfigurationInnerLink = "#";
                                                configurationInnerItem.Add(innerObjItem);
                                                configurationDataItem.ConfigurationInnerkeysPage = configurationInnerItem;

                                            }
                                        }
                                    }
                                }
                            }
                            configurationDataItemList.Add(configurationDataItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return configurationDataItemList;
        }



        #endregion

        public bool DoesSitecoreItemHavePresentation(Item item)
        {
            return item.Fields[Sitecore.FieldIDs.LayoutField] != null
            && item.Fields[Sitecore.FieldIDs.LayoutField].Value != String.Empty;
        }
    }
}