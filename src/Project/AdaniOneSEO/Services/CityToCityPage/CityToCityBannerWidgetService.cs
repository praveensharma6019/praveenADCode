using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models.CityToCityBannerWidget;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using widget = Project.AdaniOneSEO.Website.Models.CityToCityBannerWidget.widget;
using widgetItems = Project.AdaniOneSEO.Website.Models.CityToCityBannerWidget.widgetItems;
using System.Linq;
using Sitecore.Shell.Framework.Commands.TemplateBuilder;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public class CityToCityBannerWidgetService : ICityToCityBannerWidgetService
    {
        public CityToCityBannerWidgetModel GetCityToCityBannerWidgetModel(Rendering rendering)
        {
            CityToCityBannerWidgetModel CityToCityBannerDataModel = null;

            try
            {
                //var datasource = Utils.GetRenderingDatasource(rendering);
                //if (datasource == null) return null;
                var datasourcePath = GetItemId(rendering.DataSource);
                var solrItems = GetBucketedItemsFromSolr(datasourcePath);

                CityToCityBannerDataModel = new CityToCityBannerWidgetModel();
                widget widgetobj = new widget();
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
                }
                ProcessSolrItems(solrItems, widgetobj);
                CityToCityBannerDataModel.widget = widgetobj;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return CityToCityBannerDataModel;
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

        private void ProcessSolrItems(List<Item> solrItems, widget widgetobj)
        {
            List<widgetItems> widgetItems = new List<widgetItems>();
            foreach (var solrItem in solrItems)
            {
                widgetItems widgetItemsobj = new widgetItems();
                widgetItemsobj.title = Utils.GetValue(solrItem, BaseTemplates.CityToCityBannerWidgetTemplate.Title);
                widgetItemsobj.description = Utils.GetValue(solrItem, BaseTemplates.CityToCityBannerWidgetTemplate.Description);
                widgetItemsobj.url = Utils.GetLinkURL(solrItem, BaseTemplates.CityToCityBannerWidgetTemplate.url.ToString());
                widgetItemsobj.urlTarget = Utils.GetValue(solrItem, BaseTemplates.CityToCityBannerWidgetTemplate.urlTarget);
                widgetItemsobj.urlName = Utils.GetValue(solrItem, BaseTemplates.CityToCityBannerWidgetTemplate.urlName);
                widgetItemsobj.image = Utils.GetImageURLByFieldId(solrItem, BaseTemplates.CityToCityBannerWidgetTemplate.image);

                List<placesToVisitInCityItems> placesToVisitList = new List<placesToVisitInCityItems>();
                var field = (Sitecore.Data.Fields.NameValueListField)solrItem.Fields["Key"];
                foreach (var key in field.NameValues.AllKeys)
                {
                    placesToVisitInCityItems place = new placesToVisitInCityItems();
                    place.placeName = key;
                    //place.placeLink = Utils.GetLinkURL(solrItem, BaseTemplates.PlaceToVisitDetails.PlaceLink.ToString());
                    //place.locationIcon = Utils.GetValue(solrItem, BaseTemplates.PlaceToVisitDetails.LocationIcon);
                    placesToVisitList.Add(place);


                }
                widgetItemsobj.BuisnessDataList = placesToVisitList;
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
