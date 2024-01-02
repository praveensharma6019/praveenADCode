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
using System.Collections.Specialized;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public class FAQWidget : IFAQWidget
    {
        public BannerWidgetModel GetFAQWidget(Rendering rendering)
        {
            BannerWidgetModel BannerWidgetDataModel = null;

            try
            {
               BannerWidgetDataModel = new BannerWidgetModel();


                widget widgetobj = new widget();

                #region
                //widgetobj.itemMargin = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.itemMargin);
                //widgetobj.subItemMargin = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.subItemMargin);
                //widgetobj.actionTitle = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.actionTitle);
                //widgetobj.carouselParam = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.carouselParam);
                //widgetobj.tabConfiguration = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.GalleryMobileBanner);
                //widgetobj.gradientConfiguration = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.gradientConfiguration);
                //widgetobj.gridConfiguration = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.gridConfiguration);
                //widgetobj.subItemColors = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.subItemColors);

                //if (datasource.HasChildren)
                //{
                //    List<widgetItems> widgetItems = new List<widgetItems>();
                //    foreach (Item child in datasource.Children)
                //    {
                //        widgetItems widgetItemsobj = new widgetItems();
                //        widgetItemsobj.title = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Title);
                //        widgetItemsobj.description = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Description);
                //        widgetItemsobj.autoid = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.AutoId);
                //        widgetItemsobj.imagesrc = Utils.GetImageURLByFieldId(child, BaseTemplates.BannerWidgetTemplate.Image);
                //        widgetItemsobj.bannerlogo = Utils.GetImageURLByFieldId(child, BaseTemplates.BannerWidgetTemplate.BannerLogo);
                //        widgetItemsobj.subtitle = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.SubTitle);
                //        widgetItemsobj.uniqueid = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.UniqueId);
                //        widgetItemsobj.mobileimage = Utils.GetImageURLByFieldId(child, BaseTemplates.BannerWidgetTemplate.MobileImage);
                //        widgetItemsobj.btnText = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.btnText);
                //        widgetItemsobj.isAirportSelectNeeded = Utils.GetCheckBoxSelection(child.Fields["isAirportSelectNeeded"]);
                //        widgetItemsobj.link = Utils.GetLinkURL(child, BaseTemplates.BannerWidgetTemplate.Link.ToString());
                //        widgetItemsobj.linkTarget = Utils.GetLinkURLTarget(child, BaseTemplates.BannerWidgetTemplate.Link.ToString());
                //        widgetItemsobj.isAgePopup = Utils.GetCheckBoxSelection(child.Fields["isAgePopup"]);
                //        widgetItemsobj.offerEligibility = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.OfferEligibility);
                //        widgetItemsobj.cardBgColor = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.cardBgColor);
                //        widgetItemsobj.gridNumber = Utils.GetIntValue(child, BaseTemplates.BannerWidgetTemplate.gridNumber);
                //        widgetItemsobj.listClass = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.listClass);
                //        widgetItemsobj.checkValidity = Utils.GetCheckBoxSelection(child.Fields["checkValidity"]);
                //        widgetItemsobj.effectiveFrom = Utils.GetDate(child, BaseTemplates.BannerWidgetTemplate.EffectiveFrom);
                //        widgetItemsobj.effectiveTo = Utils.GetDate(child, BaseTemplates.BannerWidgetTemplate.EffectiveTo);

                //        tags tagssobj = new tags();
                //        tagssobj.bannerCategory = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.BannerCategory);
                //        tagssobj.businessUnit = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.BusinessUnit);
                //        tagssobj.category = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Category);
                //        tagssobj.faqCategory = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.FaqCategory);
                //        tagssobj.label = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Label);
                //        tagssobj.source = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Source);
                //        tagssobj.subCategory = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.SubCategory);
                //        tagssobj.type = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Type);
                //        tagssobj.eventName = Utils.GetValue(child, BaseTemplates.BannerWidgetTemplate.Event);
                //        widgetItemsobj.tags = tagssobj;
                //        widgetItems.Add(widgetItemsobj);
                //    }
                //    var solrItems = GetBucketedItemsFromSolr(rendering.DataSource);

                //    // Process Solr items and populate the model
                //    ProcessSolrItems(solrItems, widgetobj);

                //    BannerWidgetDataModel.widget = widgetobj;
                //    //widgetobj.widgetItems = widgetItems;
                //}
                #endregion
                var datasourcePath = GetItemId(rendering.DataSource);
                var solrItems = GetBucketedItemsFromSolr(datasourcePath);
                if (solrItems.Any())
                {
                    widgetobj.widgetId = Utils.GetIntValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.widgetId);
                    widgetobj.widgetType = Utils.GetValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.widgetType);
                    widgetobj.title = Utils.GetValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.title);
                    widgetobj.subTitle = Utils.GetValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.subTitle);
                    widgetobj.subItemRadius = Utils.GetIntValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.subItemRadius);
                    widgetobj.subItemWidth = Utils.GetIntValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.subItemWidth);
                    widgetobj.gridColumn = Utils.GetIntValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.gridColumn);
                    widgetobj.aspectRatio = Utils.GetIntValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.aspectRatio);
                    widgetobj.borderRadius = Utils.GetIntValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.borderRadius);
                    widgetobj.backgroundColor = Utils.GetValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.backgroundColor);
                    widgetobj.children = Utils.GetValue(solrItems.First(), BaseTemplates.BannerWidgetTemplate.WidgetChildren);
                }
                ProcessSolrItems(solrItems, widgetobj);
                BannerWidgetDataModel.widget = widgetobj;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
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
                string keyy = Utils.GetValue(solrItem, BaseTemplates.BannerWidgetTemplate.Key);
                var field = (Sitecore.Data.Fields.NameValueListField)solrItem.Fields["Key"];
                foreach (var key in field.NameValues.AllKeys)
                {
                    widgetItems widgetItemsobj = new widgetItems();
                    //var value = field.NameValues[key];
                    widgetItemsobj.title = key;
                    widgetItemsobj.description = field.NameValues[key];
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