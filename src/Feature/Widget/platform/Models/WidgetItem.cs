using Newtonsoft.Json;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.Models
{
    public class WidgetItem
    {
        public int widgetId { get; set; }
        public string widgetType { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public double subItemRadius { get; set; }
        public double subItemWidth { get; set; }
        public int gridColumn { get; set; }
        public double aspectRatio { get; set; }

        public ItemCSS itemMargin { get; set; }
        public ItemCSS subItemMargin { get; set; }

        public ActionTitle actionTitle { get; set; }

        public List<Object> widgetItems { get; set; }
    }
    public class FeedsdataList
    {
        public List<Object> data { get; set; }
    }
    public class FeedsdataItem
    {
        public string src { get; set; }
        public string link { get; set; }
        public string linktarget { get; set; }
        public string imgalt { get; set; }
        public string imgtitle { get; set; }
        public string heading { get; set; }
        public int columns { get; set; }
    }
    public class AllaccoladesList
    {
        public List<Object> data { get; set; }
    }
    public class AllaccoladesItem
    {
        public string src { get; set; }
        public string caption { get; set; }
        public string winner { get; set; }
        public string date { get; set; }
        public string imgalt { get; set; }
        public string imgtitle { get; set; }
    }
    public class TownshipList
    {
        public List<TownshipComponent> data { get; set; }
        public string bannerImage { get; set; }
        public string ImgType { get; set; }

    }
    public class TownshipComponent
    {
        public string start { get; set; }
        public string count { get; set; }
        public string detail { get; set; }
        public string delay { get; set; }
        public string imgType { get; set; }

    }
    public class TimelineList
    {
        public List<Object> data { get; set; }
    }
    public class TimelineComponent
    {
        public string src { get; set; }
        public string Alt { get; set; }
        public string year { get; set; }
        public string name { get; set; }
        public string highlight { get; set; }
        public string imgType { get; set; }
        public string mobileImage { get; set; }
    }
    public class ItemCSS
    {
        public double left { get; set; }
        public double right { get; set; }
        public double bottom { get; set; }
        public double top { get; set; }
    }

    public class ActionTitle
    {
        public int actionId { get; set; }
        public string deeplink { get; set; }
        public string name { get; set; }
    }
    public class OurBrand
    {
        [JsonProperty(PropertyName = "ourBrands")]
        public InnerData OurBrands { get; set; }
    }
    public class OurPartners
    {
        public string heading { get; set; }
        public List<object> Data { get; set; }
    }
    public class InnerData
    {
        [JsonProperty(PropertyName = "data")]
        public List<object> Data { get; set; }
    }
    public class Brand
    {
        [JsonProperty(PropertyName = "src")]
        public string Src { get; set; }
        [JsonProperty(PropertyName = "alt")]
        public string Alt { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
    public class Location
    {
        [JsonProperty(PropertyName = "locationData")]
        public HomeLocationData LocationData { get; set; }
    }
    public class HomeLocationData
    {
        [JsonProperty(PropertyName = "options")]
        public List<object> Options { get; set; }
    }

    public class Options
    {
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
    }
    public class EmployeeCard
    {
        [JsonProperty(PropertyName = "employeeCareCard")]
        public EmployeeDataArray EmployeeCareCard { get; set; }
    }
    public class EmployeeDataArray
    {
        [JsonProperty(PropertyName = "data")]
        public List<object> Data { get; set; }
    }
    public class EmployeeData
    {
        [JsonProperty(PropertyName = "src")]
        public string ImageSrc { get; set; }
        [JsonProperty(PropertyName = "img-alt")]
        public string ImageAlt { get; set; }
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "detail")]
        public string Detail { get; set; }
        [JsonProperty(PropertyName = "logo")]
        public string Logo { get; set; }
        [JsonProperty(PropertyName = "alt")]
        public string LogoAlt { get; set; }
    }
    public class PageContent
    {
        [JsonProperty(PropertyName = "content")]
        public Content PageInnerContent { get; set; }

    }
    public class CategoryLifestyleList
    {
        public List<object> data { get; set; }
    }
    public class CatagotyType
    {
        public string categorytitle { get; set; }
        public string categorylink { get; set; }
    }
    public class CommunicationItems
    {
        public List<CommunicationItemsData> Item { get; set; }
    }
    public class RestaurantInformation
    {
        public string Title { get; set; }
        public string location { get; set; }
        public string foodCategories { get; set; }
        public string foodPrice { get; set; }

    }

    public class RestaurantTabData
    {
        public List<RestaurantTab> RestaurantTabsData { get; set; }

    }

    public class RestaurantTab
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string LinkTitle { get; set; }

    }

    public class OpentimingsData
    {
        public OpenTiming OpenTiming { get; set; }
        

    }

    public class OpenTiming
    {
        public List<TimeingData> Data { get; set; }

    }
    public class ReadMoreArticles
    {
        public string componentname { get; set; }
        public List<Articles> data { get; set; }

    }
    public class Articles
    {
        public string articleType { get; set; }
        public string articleLink { get; set; }
        public string articleLinkIcon { get; set; }
        public string articleLinkTitle { get; set; }
        public string articleThumb { get; set; }
        public string articleThumbAlt { get; set; }
        public string articleTitle { get; set; }
        public string articleDescription { get; set; }

    }




    public class TimeingData
    {
        public string Day { get; set; }

        public string Time { get; set; }

    }

    public class ReasaurantContent
    {
        public string about { get; set; }
        public string menu { get; set; }
        public string otherInfo { get; set; }
        public string moreReastaurant { get; set; }

        [JsonProperty("for")]
        public string restrofor { get; set; }

        public string count { get; set; }
        public string member { get; set; }
        public string menuDownload { get; set; }
        public string share { get; set; }
        public string aboutReastaurant { get; set; }
        public string callUs { get; set; }
        public string contactNum { get; set; }
        public string openNow { get; set; }
        public string timeSlot1 { get; set; }
        public string enquireNow { get; set; }
    }

    public class ReasaurantCard
    {
        public string src { get; set; }
        public string title { get; set; }
        public string logo { get; set; }
        public string price { get; set; }
        public string discount { get; set; }
        public string status { get; set; }
    }
    public class ReasaurantMenu
    {
        public string src { get; set; }
        public string alt { get; set; }
        public string title { get; set; }
    }
    public class CommunicationItemsData
    {
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string redirectUrl { get; set; }
        public string Slug { get; set; }
        public string heading { get; set; }
        public string description { get; set; }
        public string quote { get; set; }
        public string date { get; set; }
        [JsonProperty("blogSchema")]
        public string blogSchema { get; set; }
        public List<CatagotyType> category { get; set; }
    }
    public class LifestyleItem
    {
        public bool IsDefault { get; set; }
        public string Slug { get; set; }
        public string link { get; set; }
        public string src { get; set; }
        public string imgalt { get; set; }
        public string imgtitle { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string datetime { get; set; }
        public List<CatagotyType> category { get; set; }
        public string readtime { get; set; }
        public string blogSchema { get; set; }
    }
    public class Content
    {
        [JsonProperty(PropertyName = "pageData")]
        public string PageData { get; set; }
        [JsonProperty(PropertyName = "pageHeading")]
        public string PageHeading { get; set; }
        [JsonProperty(PropertyName = "upcoming")]
        public string Upcoming { get; set; }
        [JsonProperty(PropertyName = "year2021")]
        public string Year2021 { get; set; }
        [JsonProperty(PropertyName = "year2020")]
        public string Year2020 { get; set; }
        [JsonProperty(PropertyName = "events")]
        public string Events { get; set; }
    }

    public class TopBar
    {
        public string title { get; set; }
        public string location { get; set; }
        public string roomPrice { get; set; }
        public string link { get; set; }

        public string linkTitle { get; set; }
        public string about { get; set; }
    }

    public class Feature
    {
        public string title { get; set; }
        public string icon { get; set; }
    }

    public class RoomTitle
    {
        public string title { get; set; }
        public List<Feature> features { get; set; }
    }

    public class RoomInfoTab
    {
        public string target { get; set; }
        public string tabTitle { get; set; }
    }

    public class RoomInfoTabInfos
    {
        public List<RoomInfoTab> roomInfoTabs { get; set; }
    }

    public class RoomFacilities
    {
        public List<Facility> facilities { get; set; }
    }

    public class Facility
    {
        public string title { get; set; }
        public string icon { get; set; }
    }

    public class OtherRooms
    {
        public string src { get; set; }
        public string title { get; set; }
        public string nonmemberPrice { get; set; }
        public string memberPrice { get; set; }
    }

    public class TopBarModel
    {
        public TopBar TopBar { get; set; }
    }

    public class RoomTitleModel
    {
        public RoomTitle RoomTitle { get; set; }
    }

    public class OtherRoomsModel
    {
        public OtherRooms OtherRooms { get; set; }
    }
}