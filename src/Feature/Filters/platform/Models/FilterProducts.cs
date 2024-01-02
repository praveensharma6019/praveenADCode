using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Realty.Feature.Widget.Platform.Models;


namespace Adani.SuperApp.Realty.Feature.Filters.Platform.Models
{
    public class FilterProductsWidgets
    {
        public WidgetItem widget { get; set; }
        //  public List<FilterProducts> widgetItems { get; set; }
    }
    public class NoresultFounfEnquiryForm
    {
        public string heading { get; set; }
        public string description { get; set; }
        public string buttonText { get; set; }
    }
    /// <summary>
    /// Class to get the Material Groups
    /// </summary>
    public class FilterProducts
    {
        public string title { get; set; }
        public string apiUrl { get; set; }

        public string materialGroup { get; set; }

        public string category { get; set; }

        public string subCategory { get; set; }

        public string brand { get; set; }

        public bool popular { get; set; }

        public bool newArrival { get; set; }

        public bool showOnHomepage { get; set; }
    }
    public class FilterData
    {
        public string heading { get; set; }
        public string clearAllLabel { get; set; }
        public string applyFilterLabel { get; set; }
        public List<FilterTabsData> filterTabsData { get; set; }
    }
    public class FilterTabsData
    {
        public string tabID { get; set; }
        public string tabHeading { get; set; }
        public List<filterButtons> filterButtons { get; set; }
    }
    public class filterButtons
    {
        public string type { get; set; }
        public string For { get; set; }
        public string filterHeading { get; set; }
        public List<buttons> buttons { get; set; }
        public string allInclusive { get; set; }
        public string minRangeValue { get; set; }
        public string maxRangeValue { get; set; }
        public string Rs { get; set; }
        public string addsign { get; set; }
    }
    public class buttons
    {
        public string id { get; set; }
        public string buttonLabel { get; set; }
        public string controllerName { get; set; }
    }
    public class CityDescriptionList
    {
        public List<object> data { get; set; }
    }
    public class CityDescriptionItem
    {
        public string src { get; set; }
        public string slug { get; set; }
        public string cityname { get; set; }
        public string citydetail { get; set; }
        public string readmore { get; set; }

    }
    public class CommonTextForAboutAdani
    {
        public string adaniRealty { get; set; }
        public string journey { get; set; }
        public string aboutAdaniRealty { get; set; }
        public string ourLocation { get; set; }

    }
    public class LocationSearchDataList
    {
        public List<object> data { get; set; }
    }
    public class LocationSearchDataItem
    {
        public string propertStatus { get; set; }
        public List<propertype> propertyType { get; set; }
        public string configuration { get; set; }
        public List<propertyConfiguration> propertyConfiguration { get; set; }
        public string priceRange { get; set; }
        public string allInclusion { get; set; }
        public string rangeStart { get; set; }
        public string rangeEnd { get; set; }
        public string residential { get; set; }
        public string commercialandRetail { get; set; }
        public string clearAll { get; set; }
        public string apply { get; set; }
    }
    public class propertype
    {
        public string id { get; set; }
        public string projectType { get; set; }
    }
    public class propertyConfiguration
    {
        public string id { get; set; }
        public string configuration { get; set; }
    }
    public class SearchData
    {
        public string type { get; set; }
        public List<OptionClass> options { get; set; }
        public string placeholder { get; set; }
        public string label { get; set; }
        public string errorMessage { get; set; }
    }
    public class OptionClass
    {
        public string label { get; set; }
        public string key { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class SEOHeadingDescription
    {
        public string src { get; set; }
        public string cityName { get; set; }
        public string cityDetail { get; set; }
        public string readmore { get; set; }
        public string heading { get; set; }
        public bool IsSEOPage { get; set; }
    }
}