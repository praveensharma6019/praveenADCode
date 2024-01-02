using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Shell.Framework.Commands.TemplateBuilder;
using Sitecore.Data;
using Sitecore.Layouts;
using System.Web;

namespace Project.AdaniOneSEO.Website.Services
{
    public class BannerWidget : IBannerWidget
    {
        public BannerWidgetModel GetBannerWidget(Rendering rendering)
        {
            BannerWidgetModel BannerWidgetDataModel = null;

            //try
            //{
            //   BannerWidgetDataModel = new BannerWidgetModel();


            //    widget widgetobj = new widget();

            //    var datasourcePath = GetItemId(rendering.DataSource);
            //    var solrItems = GetBucketedItemsFromSolr(datasourcePath);
            //    if (solrItems.Any())
            //    {
            //        widgetobj.widgetId = Utils.GetIntValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.widgetId);
            //        widgetobj.widgetType = Utils.GetValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.widgetType);
            //        widgetobj.title = Utils.GetValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.title);
            //        widgetobj.subTitle = Utils.GetValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.subTitle);
            //        widgetobj.subItemRadius = Utils.GetIntValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.subItemRadius);
            //        widgetobj.subItemWidth = Utils.GetIntValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.subItemWidth);
            //        widgetobj.gridColumn = Utils.GetIntValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.gridColumn);
            //        widgetobj.aspectRatio = Utils.GetIntValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.aspectRatio);
            //        widgetobj.borderRadius = Utils.GetIntValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.borderRadius);
            //        widgetobj.backgroundColor = Utils.GetValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.backgroundColor);
            //        widgetobj.children = Utils.GetValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.WidgetChildren);
            //    }
            //    ProcessSolrItems(solrItems, widgetobj);
            //    BannerWidgetDataModel.widget = widgetobj;
            //}
            //catch (Exception ex)
            //{
            //    Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            //}


            try
            {
                string itemName = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("ItemName");
                string indexName = "sitecore_master_index";
                string contextDB = Sitecore.Context.Database.Name;
                BannerWidgetDataModel = new BannerWidgetModel();
                widget widgetobj = new widget();
                using (var searchContext = ContentSearchManager.GetIndex(indexName).CreateSearchContext())
                {
                    ID itemTemplateID = new Sitecore.Data.ID(new Guid("{98753523-5CE8-4AA2-BF03-4562CC9FE17C}"));
                    if (!string.IsNullOrEmpty(itemName) && itemName != "domestic-flights")
                    {
                        var result = searchContext.GetQueryable<SearchResultItem>().
                            Where(
                                x => x.TemplateId == itemTemplateID &&
                                x.Name == itemName
                            ).FirstOrDefault();

                        if (result != null)
                        {
                            Item item = result.GetItem();
                            if (item != null)
                            {
                                RenderingReference[] renderingReferences = item.Visualization.GetRenderings(Sitecore.Context.Device, true);
                                foreach (var renderingReference in renderingReferences)
                                {
                                    if (renderingReference.WebEditDisplayName == "BannerWidget")
                                    {
                                        string renderingItemPath = renderingReference.RenderingItem.InnerItem.Paths.Path;
                                        string datasourceId = renderingReference.Settings.DataSource;
                                        Item renderingItem = Sitecore.Context.Database.GetItem(renderingItemPath);
                                        Item datasourceItem = Sitecore.Context.Database.GetItem(datasourceId);
                                        widgetobj.widgetId = Utils.GetIntValue(datasourceItem, BaseTemplates.BannerWidgetTemplate.widgetId);
                                        widgetobj.widgetType = Utils.GetValue(datasourceItem, BaseTemplates.BannerWidgetTemplate.widgetType);
                                        widgetobj.title = Utils.GetValue(datasourceItem, BaseTemplates.BannerWidgetTemplate.title);
                                        widgetobj.subTitle = Utils.GetValue(datasourceItem, BaseTemplates.BannerWidgetTemplate.subTitle);
                                        widgetobj.subItemRadius = Utils.GetIntValue(datasourceItem, BaseTemplates.BannerWidgetTemplate.subItemRadius);
                                        widgetobj.subItemWidth = Utils.GetIntValue(datasourceItem, BaseTemplates.BannerWidgetTemplate.subItemWidth);
                                        widgetobj.gridColumn = Utils.GetIntValue(datasourceItem, BaseTemplates.BannerWidgetTemplate.gridColumn);
                                        widgetobj.aspectRatio = Utils.GetIntValue(datasourceItem, BaseTemplates.BannerWidgetTemplate.aspectRatio);
                                        widgetobj.borderRadius = Utils.GetIntValue(datasourceItem, BaseTemplates.BannerWidgetTemplate.borderRadius);
                                        widgetobj.backgroundColor = Utils.GetValue(datasourceItem, BaseTemplates.BannerWidgetTemplate.backgroundColor);
                                        widgetobj.children = Utils.GetValue(datasourceItem, BaseTemplates.BannerWidgetTemplate.WidgetChildren);

                                        List<widgetItems> widgetItems = new List<widgetItems>();


                                        if(datasourceItem != null)
                                        {
                                            widgetItems widgetItemsobj = new widgetItems();
                                            widgetItemsobj.title = Utils.GetValue(item, BaseTemplates.BannerWidgetTemplate.Bannertitle);
                                            widgetItemsobj.description = Utils.GetValue(item, BaseTemplates.BannerWidgetTemplate.Bannerdescription);
                                            // widgetItemsobj.autoid = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.AutoId);
                                            widgetItemsobj.imagesrc = Utils.GetImageURLByFieldId(item, BaseTemplates.BannerWidgetTemplate.Imagesrc);
                                            widgetItemsobj.bannerlogo = Utils.GetImageURLByFieldId(item, BaseTemplates.BannerWidgetTemplate.BannerImagelogo);
                                            widgetItemsobj.subtitle = Utils.GetValue(item, BaseTemplates.BannerWidgetTemplate.Bannersubtitle);
                                            // widgetItemsobj.uniqueid = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.UniqueId);
                                            widgetItemsobj.mobileimage = Utils.GetImageURLByFieldId(item, BaseTemplates.BannerWidgetTemplate.Bannermobileimage);
                                          //  widgetItemsobj.btnText = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.btnText);
                                            // widgetItemsobj.isAirportSelectNeeded = Utils.GetCheckBoxSelection(solrItem.Fields["isAirportSelectNeeded"]);
                                            //widgetItemsobj.link = Utils.GetLinkURL(solrItem, BaseTemplates.BannerWidgetTemplate.Link.ToString());
                                            //widgetItemsobj.linkTarget = Utils.GetLinkURLTarget(solrItem, BaseTemplates.BannerWidgetTemplate.Link.ToString());
                                          //  widgetItemsobj.isAgePopup = Utils.GetCheckBoxSelection(solrItem.Fields["isAgePopup"]);
                                          //  widgetItemsobj.offerEligibility = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.OfferEligibility);
                                           // widgetItemsobj.cardBgColor = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.cardBgColor);
                                           // widgetItemsobj.gridNumber = Utils.GetIntValue(solrItem, BaseTemplates.BannerWidgetTemplate.gridNumber);
                                           // widgetItemsobj.listClass = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.listClass);
                                           // widgetItemsobj.checkValidity = Utils.GetCheckBoxSelection(solrItem.Fields["checkValidity"]);
                                           // widgetItemsobj.effectiveFrom = Utils.GetDate(solrItem, BaseTemplates.BannerWidgetTemplate.EffectiveFrom);
                                            // widgetItemsobj.effectiveTo = Utils.GetDate(solrItem, BaseTemplates.BannerWidgetTemplate.EffectiveTo);

                                            tags tagssobj = new tags();
                                          //  tagssobj.bannerCategory = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.BannerCategory);
                                          //  tagssobj.businessUnit = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.BusinessUnit);
                                          //  tagssobj.category = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Category);
                                          //  tagssobj.faqCategory = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.FaqCategory);
                                          //  tagssobj.label = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Label);
                                          //  tagssobj.source = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Source);
                                          //  tagssobj.subCategory = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.SubCategory);
                                          //  tagssobj.type = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Type);
                                          //  tagssobj.eventName = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Event);
                                            widgetItemsobj.tags = tagssobj;
                                            widgetItems.Add(widgetItemsobj);
                                        }
                                        widgetobj.widgetItems = widgetItems;
                                        BannerWidgetDataModel.widget = widgetobj;
                                    }
                                }
                            }
                        }
                    }
                }
                return BannerWidgetDataModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return BannerWidgetDataModel;
        }
        private List<Item> GetBucketedItemsFromSolr(ID itemId)
        {
            using (var context = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext())
            {
                var query = context.GetQueryable<SearchResultItem>()
                    .Where(item => item.ItemId == itemId)
                    .ToList();

                return query.Select(result => result.GetItem()).ToList();
            }
        }
        #region
        //private List<Item> GetBucketedItemsFromSolr(ID parentId)
        //{
        //    List<Item> solrItems = new List<Item>();

        //    // Convert the parent ID to a string
        //    string parentIdString = parentId.Guid.ToString();

        //    // Construct a Solr query to retrieve items with a specific parent ID
        //    var index = ContentSearchManager.GetIndex("sitecore_master_index");
        //    using (var context = index.CreateSearchContext())
        //    {
        //        // Example query: items with a specified parent ID
        //        var query = context.GetQueryable<SearchResultItem>()
        //            .Where(item => item["_parent"] == parentIdString)
        //            .ToList();

        //        // Convert search results to Sitecore items
        //        solrItems = query.Select(result => result.GetItem()).ToList();
        //    }

        //    return solrItems;
        //}
        #endregion
        private void ProcessSolrItems(List<Item> solrItems, widget widgetobj)
        {
            List<widgetItems> widgetItems = new List<widgetItems>();
            foreach (var solrItem in solrItems)
            {
                // Extract data from solrItem and populate the widgetobj
                widgetItems widgetItemsobj = new widgetItems();
                widgetItemsobj.title = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Title);
                widgetItemsobj.description = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Description);
                widgetItemsobj.autoid = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.AutoId);
                widgetItemsobj.imagesrc = Utils.GetImageURLByFieldId(solrItem, BaseTemplates.BannerWidgetTemplate.Image);
                widgetItemsobj.bannerlogo = Utils.GetImageURLByFieldId(solrItem, BaseTemplates.BannerWidgetTemplate.BannerLogo);
                widgetItemsobj.subtitle = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.SubTitle);
                widgetItemsobj.uniqueid = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.UniqueId);
                widgetItemsobj.mobileimage = Utils.GetImageURLByFieldId(solrItem, BaseTemplates.BannerWidgetTemplate.MobileImage);
                widgetItemsobj.btnText = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.btnText);
                widgetItemsobj.isAirportSelectNeeded = Utils.GetCheckBoxSelection(solrItem.Fields["isAirportSelectNeeded"]);
                //widgetItemsobj.link = Utils.GetLinkURL(solrItem, BaseTemplates.BannerWidgetTemplate.Link.ToString());
                //widgetItemsobj.linkTarget = Utils.GetLinkURLTarget(solrItem, BaseTemplates.BannerWidgetTemplate.Link.ToString());
                widgetItemsobj.isAgePopup = Utils.GetCheckBoxSelection(solrItem.Fields["isAgePopup"]);
                widgetItemsobj.offerEligibility = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.OfferEligibility);
                widgetItemsobj.cardBgColor = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.cardBgColor);
                widgetItemsobj.gridNumber = Utils.GetIntValue(solrItem, BaseTemplates.BannerWidgetTemplate.gridNumber);
                widgetItemsobj.listClass = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.listClass);
                widgetItemsobj.checkValidity = Utils.GetCheckBoxSelection(solrItem.Fields["checkValidity"]);
                widgetItemsobj.effectiveFrom = Utils.GetDate(solrItem, BaseTemplates.BannerWidgetTemplate.EffectiveFrom);
                widgetItemsobj.effectiveTo = Utils.GetDate(solrItem, BaseTemplates.BannerWidgetTemplate.EffectiveTo);

                tags tagssobj = new tags();
                tagssobj.bannerCategory = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.BannerCategory);
                tagssobj.businessUnit = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.BusinessUnit);
                tagssobj.category = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Category);
                tagssobj.faqCategory = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.FaqCategory);
                tagssobj.label = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Label);
                tagssobj.source = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Source);
                tagssobj.subCategory = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.SubCategory);
                tagssobj.type = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Type);
                tagssobj.eventName = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Event);
                widgetItemsobj.tags = tagssobj;
                widgetItems.Add(widgetItemsobj);
            }
            widgetobj.widgetItems = widgetItems;
        }

        private ID GetItemId(string dataSource)
        {
            if (ID.TryParse(dataSource, out ID itemId))
            {
                return itemId;
            }
            else if (Guid.TryParse(dataSource, out Guid itemGuid))
            {
                Item item = Sitecore.Context.Database.GetItem(new ID(itemGuid));
                if (item != null)
                {
                    return item.ID;
                }
            }
            else { return new ID(dataSource); }

            return null;
        }
    }
}