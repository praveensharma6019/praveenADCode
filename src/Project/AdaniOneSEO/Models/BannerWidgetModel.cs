using System.Collections.Generic;

namespace Project.AdaniOneSEO.Website.Models
{
    public class BannerWidgetModel
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
        public string autoid { get; set; }
        public string imagesrc { get; set; }
        public string bannerlogo { get; set; }
        public string subtitle { get; set; }
        public string uniqueid { get; set; }
        public string mobileimage { get; set; }
        public string btnText { get; set; }
        public bool isAirportSelectNeeded { get; set; }
        public bool isAgePopup { get; set; }
        public string link { get; set; }
        public string linkTarget { get; set; }
        public string offerEligibility { get; set; }
        public int gridNumber { get; set; }
        public string cardBgColor { get; set; }
        public string listClass { get; set; }
        public tags tags { get; set; }
        public bool checkValidity { get; set; }
        public string effectiveFrom { get; set; }
        public string effectiveTo { get; set; }
    }

    public class tags
    {
        public string bannerCategory { get; set; }
        public string businessUnit { get; set; }
        public string category { get; set; }
        public string faqCategory { get; set; }
        public string label { get; set; }
        public string source { get; set; }
        public string subCategory { get; set; }
        public string type { get; set; }
        public string eventName { get; set; }
    }
}