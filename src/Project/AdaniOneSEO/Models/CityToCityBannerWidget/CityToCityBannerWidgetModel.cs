using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniOneSEO.Website.Models.CityToCityBannerWidget
{
        public class CityToCityBannerWidgetModel
        {
            public widget widget { get; set; }
        }
        public class widget
        {
            public int widgetId { get; set; }
            public string widgetType { get; set; }
            public string title { get; set; }
            public string subTitle { get; set; }
            public int subItemRadius { get; set; }
            public int subItemWidth { get; set; }
            public int gridColumn { get; set; }
            public int aspectRatio { get; set; }
            public int borderRadius { get; set; }
            public string backgroundColor { get; set; }
            public string itemMargin { get; set; }
            public string subItemMargin { get; set; }
            public string actionTitle { get; set; }
            public string carouselParam { get; set; }
            public string tabConfiguration { get; set; }
            public string gradientConfiguration { get; set; }
            public string gridConfiguration { get; set; }
            public string subItemColors { get; set; }

            public List<widgetItems> widgetItems { get; set; }
        }

        public class widgetItems
        {

            public string title { get; set; }
            public string description { get; set; }
            public string url { get; set; }
            public string urlTarget { get; set; }
            public string urlName { get; set; }
            public string image { get; set; }
            public List<placesToVisitInCityItems> BuisnessDataList { get; set; }
        }

        public class placesToVisitInCityItems
        {
            public string placeName { get; set; }
            public string placeLink { get; set; }
            public string locationIcon { get; set; }
        }
    
}