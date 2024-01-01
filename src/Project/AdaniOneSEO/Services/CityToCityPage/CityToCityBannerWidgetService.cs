using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models.CityToCityBannerWidget;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using widget = Project.AdaniOneSEO.Website.Models.CityToCityBannerWidget.widget;
using widgetItems = Project.AdaniOneSEO.Website.Models.CityToCityBannerWidget.widgetItems;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public class CityToCityBannerWidgetService : ICityToCityBannerWidgetService
    {
        public CityToCityBannerWidgetModel GetCityToCityBannerWidgetModel(Rendering rendering)
        {
            CityToCityBannerWidgetModel CityToCityBannerDataModel = null;

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                CityToCityBannerDataModel = new CityToCityBannerWidgetModel();

                widget widgetobj = new widget();
                widgetobj.widgetId = Utils.GetIntValue(datasource, BaseTemplates.BannerWidgetTemplate.widgetId);
                widgetobj.widgetType = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.widgetType);
                widgetobj.title = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.title);
                widgetobj.subTitle = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.subTitle);
                widgetobj.subItemRadius = Utils.GetIntValue(datasource, BaseTemplates.BannerWidgetTemplate.subItemRadius);
                widgetobj.subItemWidth = Utils.GetIntValue(datasource, BaseTemplates.BannerWidgetTemplate.subItemWidth);
                widgetobj.gridColumn = Utils.GetIntValue(datasource, BaseTemplates.BannerWidgetTemplate.gridColumn);
                widgetobj.aspectRatio = Utils.GetIntValue(datasource, BaseTemplates.BannerWidgetTemplate.aspectRatio);
                widgetobj.borderRadius = Utils.GetIntValue(datasource, BaseTemplates.BannerWidgetTemplate.borderRadius);
                widgetobj.backgroundColor = Utils.GetValue(datasource, BaseTemplates.BannerWidgetTemplate.backgroundColor);
               
                if (datasource.HasChildren)
                {
                    List<widgetItems> widgetItems = new List<widgetItems>();
                    foreach (Item child in datasource.Children)
                    {
                        widgetItems widgetItemsobj = new widgetItems();
                        widgetItemsobj.title = Utils.GetValue(child, BaseTemplates.CityToCityBannerWidgetTemplate.Title);
                        widgetItemsobj.description = Utils.GetValue(child, BaseTemplates.CityToCityBannerWidgetTemplate.Description);
                        widgetItemsobj.url = Utils.GetLinkURL(child, BaseTemplates.CityToCityBannerWidgetTemplate.url.ToString());
                        widgetItemsobj.urlTarget = Utils.GetValue(child, BaseTemplates.CityToCityBannerWidgetTemplate.urlTarget);
                        widgetItemsobj.urlName = Utils.GetValue(child, BaseTemplates.CityToCityBannerWidgetTemplate.urlName);
                        widgetItemsobj.image = Utils.GetImageURLByFieldId(child, BaseTemplates.CityToCityBannerWidgetTemplate.image);
                       
                        if (child.HasChildren)
                        {
                            List<placesToVisitInCityItems> placesToVisitList = new List<placesToVisitInCityItems>();
                            foreach (Item placeItem in child.Children)
                            {
                                placesToVisitInCityItems place = new placesToVisitInCityItems();
                                place.placeName = Utils.GetValue(placeItem, BaseTemplates.PlaceToVisitDetails.PlaceName);
                                place.placeLink = Utils.GetLinkURL(placeItem, BaseTemplates.PlaceToVisitDetails.PlaceLink.ToString()); 
                                place.locationIcon = Utils.GetValue(placeItem, BaseTemplates.PlaceToVisitDetails.LocationIcon);
                                placesToVisitList.Add(place);
                            }
                            widgetItemsobj.BuisnessDataList = placesToVisitList;
                        }
                        widgetItems.Add(widgetItemsobj);
                    }
                    
                    widgetobj.widgetItems = widgetItems;
                }

                CityToCityBannerDataModel.widget = widgetobj;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return CityToCityBannerDataModel;
        }
    }
}
