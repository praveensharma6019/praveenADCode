using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models
{
    #region Model Type Item

    public class LocationDataItem
    {
        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }
    }
    public class AboutAdaniRealtyDataItem
    {
        [JsonProperty(PropertyName = "desc")]
        public string aboutAdaniRealtyText { get; set; }
    }
    public class AboutUsDataItem
    {
        [JsonProperty(PropertyName = "desc")]
        public string aboutUsText { get; set; }
    }
    public class OurLocationDataItem
    {
        [JsonProperty(PropertyName = "link")]
        public string link { get; set; }

        [JsonProperty(PropertyName = "src")]
        public string imageSrc { get; set; }

        [JsonProperty(PropertyName = "imgalt")]
        public string imageAlt { get; set; }

        [JsonProperty(PropertyName = "imgtitle")]
        public string imgTitle { get; set; }

        [JsonProperty(PropertyName = "projectcity")]
        public string projectCity { get; set; }

        [JsonProperty(PropertyName = "projectlist")]
        public List<ProjectlistDataItem> projectList { get; set; }

        [JsonProperty(PropertyName = "propertyType")]
        public List<string> propertyType { get; set; }

    }
    public class ProjectlistDataItem
    {
        [JsonProperty(PropertyName = "projecttitle")]
        public string projectTitle { get; set; }

        [JsonProperty(PropertyName = "projectprice")]
        public string projectPrice { get; set; }
        [JsonProperty(PropertyName = "projectType")]
        public string projectType { get; set; }
        [JsonProperty(PropertyName = "Projectlink")]
        public string Projectlink { get; set; }
        [JsonProperty(PropertyName = "linktarget")]
        public string linktarget { get; set; }
        [JsonProperty(PropertyName = "propertyType")]
        public string propertyType { get; set; }
        [JsonProperty(PropertyName = "allinclusive")]
        public string AllInclusiveLabel { get; set; }
        [JsonProperty(PropertyName = "PriceStarting")]
        public string pricestartinglabel { get; set; }
        [JsonProperty(PropertyName = "condition")]
        public string condition { get; set; }

    }
    public class ProjectType
    {
        [JsonProperty(PropertyName = "projecttypetitle")]
        public string projectTypeTitle { get; set; }
    }
    public class GetInTouchDataItem
    {
        [JsonProperty(PropertyName = "heading")]
        public string heading { get; set; }

        [JsonProperty(PropertyName = "desc")]
        public string description { get; set; }

        [JsonProperty(PropertyName = "button")]
        public string button { get; set; }
        [JsonProperty(PropertyName = "enquireNowLabel")]
        public string enquireNowLabel { get; set; }

        [JsonProperty(PropertyName = "buttonLink")]
        public string buttonLink { get; set; }
    }
    public class AboutGoodLifeDataItem
    {
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [JsonProperty(PropertyName = "about")]
        public string About { get; set; }

        [JsonProperty(PropertyName = "readMore")]
        public string ReadMore { get; set; }

        [JsonProperty(PropertyName = "terms")]
        public string Terms { get; set; }

        [JsonProperty(PropertyName = "detailLink")]
        public string DetailLink { get; set; }


        [JsonProperty(PropertyName = "extrCharges")]
        public string ExtraCharges { get; set; }

        [JsonProperty(PropertyName = "detailLinkText")]
        public string DetailLinkText { get; set; }
    }
    public class AOGBriefModel
    {
        public string pageHeading { get; set; }
        public string pageHeadingInGradiant { get; set; }
        public string sup { get; set; }
        public string subHeading { get; set; }
        public string description { get; set; }

    }
    public class GoodLifeDataItem
    {
        [JsonProperty(PropertyName = "src")]
        public string ImgSrc { get; set; }

        [JsonProperty(PropertyName = "imgalt")]
        public string ImgAlt { get; set; }

        [JsonProperty(PropertyName = "imgtitle")]
        public string ImgTitle { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string status { get; set; }
        [JsonProperty(PropertyName = "gallery")]
        public List<GoodLifeGallery> Gallery { get; set; }
    }
    public class GoodLifeGallery
    {
        [JsonProperty(PropertyName = "src")]
        public string ImgSrc { get; set; }

        [JsonProperty(PropertyName = "alt")]
        public string ImgAlt { get; set; }
        [JsonProperty(PropertyName = "imgType")]
        public string Imgtitle { get; set; }
    }
    public class FeaturedBlogCategoryListDataItem
    {
        [JsonProperty(PropertyName = "categorytitle")]
        public string CategoryTitle { get; set; }

        [JsonProperty(PropertyName = "categorylink")]
        public string CategoryLink { get; set; }

        [JsonProperty(PropertyName = "categorylinkText")]
        public string CategoryLinkText { get; set; }
    }
    public class FeaturedBlogDataItem
    {
        [JsonProperty(PropertyName = "category")]
        public List<FeaturedBlogCategoryListDataItem> FeaturedBlogCategoryListDataItemList { get; set; }

        [JsonProperty(PropertyName = "src")]
        public string ImgSrc { get; set; }

        [JsonProperty(PropertyName = "imgalt")]
        public string ImgAlt { get; set; }

        [JsonProperty(PropertyName = "imgtitle")]
        public string ImgTitle { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        [JsonProperty(PropertyName = "linkText")]
        public string LinkText { get; set; }


    }
    public class AboutCityDataItem
    {
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [JsonProperty(PropertyName = "about")]
        public string About { get; set; }

        [JsonProperty(PropertyName = "readMore")]
        public string ReadMore { get; set; }
    }
    public class StaticTextDataItem
    {
        [JsonProperty(PropertyName = "rera")]
        public string Rera { get; set; }

        [JsonProperty(PropertyName = "newLaunch")]
        public string NewLaunch { get; set; }
    }
    public class FeaturesListDataItem
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

    }
    public class RoomTitleDataItem
    {

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "features")]
        public List<FeaturesListDataItem> Features { get; set; }
    }
    public class RoomInfoTabsDataItem
    {

        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }

        [JsonProperty(PropertyName = "targetText")]
        public string TargetText { get; set; }

        [JsonProperty(PropertyName = "tabTitle")]
        public string TabTitle { get; set; }
    }
    public class MostFacilitiesDataItem
    {

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

    }
    public class FacilitiesCategoriesDataItem
    {

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "facilitiesCategories")]
        public List<FeaturesListDataItem> facilitiesCategoriesDataItem { get; set; }

    }
    public class OtherRoomsDataItem
    {

        [JsonProperty(PropertyName = "src")]
        public string ImgSrc { get; set; }

        [JsonProperty(PropertyName = "imgalt")]
        public string ImgAlt { get; set; }

        [JsonProperty(PropertyName = "imgtitle")]
        public string ImgTitle { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "nonmemberPrice")]
        public string NonmemberPrice { get; set; }

        [JsonProperty(PropertyName = "memberPrice")]
        public string MemberPrice { get; set; }

    }

    public class ClubHeroBannerDataItem
    {

        [JsonProperty(PropertyName = "bannerImg")]
        public string BannerImgSrc { get; set; }

        [JsonProperty(PropertyName = "bannerAlt")]
        public string BannerImgAlt { get; set; }

        [JsonProperty(PropertyName = "bannerImgtitle")]
        public string BannerImgTitle { get; set; }

        [JsonProperty(PropertyName = "thumbImg")]
        public string ThumbImgSrc { get; set; }

        [JsonProperty(PropertyName = "logoAlt")]
        public string ThumbImgAlt { get; set; }

        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "clubLink")]
        public string Link { get; set; }
        [JsonProperty(PropertyName = "target")]
        public string target { get; set; }

        [JsonProperty(PropertyName = "linkText")]
        public string LinkText { get; set; }
        [JsonProperty(PropertyName = "isVideo")]
        public string isVideo { get; set; }
        [JsonProperty(PropertyName = "videoposter")]
        public string videoposter { get; set; }
        [JsonProperty(PropertyName = "videoMp4")]
        public string videoMp4 { get; set; }
        [JsonProperty(PropertyName = "videoOgg")]
        public string videoOgg { get; set; }
        public string SEOName { get; set; }
        public string SEODescription { get; set; }
        public string UploadDate { get; set; }
        public string videoposterMobile { get; set; }
        public string videoMp4Mobile { get; set; }
        public string videoOggMobile { get; set; }
        

    }

    public class AboutAdaniSocialClubDataItem
    {

        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [JsonProperty(PropertyName = "about")]
        public string About { get; set; }

        [JsonProperty(PropertyName = "readMore")]
        public string ReadMore { get; set; }

    }

    public class ClubHighlightsDataItem
    {

        [JsonProperty(PropertyName = "imgsrc")]
        public string ImgSrc { get; set; }

        [JsonProperty(PropertyName = "imgalt")]
        public string ImgAlt { get; set; }

        [JsonProperty(PropertyName = "imgtitle")]
        public string ImgTitle { get; set; }

        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "discount")]
        public string Discount { get; set; }

    }

    public class AboutClubDataItem
    {

        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "clubLifeImageSrc")]
        public string clubLifeImageSrc { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "readmore")]
        public string readmore { get; set; }
        [JsonProperty(PropertyName = "readless")]
        public string readless { get; set; }

    }
    public class ConfirmBannerDataItem
    {

        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [JsonProperty(PropertyName = "detail")]
        public string Detail { get; set; }

    }
    public class SaveDetailsDataItem
    {

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "imgsrc")]
        public string ImgSrc { get; set; }

        [JsonProperty(PropertyName = "imgalt")]
        public string ImgAlt { get; set; }

        [JsonProperty(PropertyName = "imgtitle")]
        public string ImgTitle { get; set; }

    }

    public class OrderDetailsDataItem
    {

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "detail")]
        public string Detail { get; set; }

    }
    public class ExploreDataItem
    {

        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }
    }
    public class ConfigurationDataItem
    {

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "keys")]
        public List<ConfigurationKeysItem> Keys { get; set; }
    }
    public class ConfigurationKeysItem
    {

        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        [JsonProperty(PropertyName = "linkText")]
        public string LinkText { get; set; }

        [JsonProperty(PropertyName = "keyword")]
        public string keyword { get; set; }
    }
    #endregion

    #region Model Data

    public class LocationData
    {
        public List<Object> desc { get; set; }
    }
    public class AboutAdaniRealtyData
    {
        public List<Object> desc { get; set; }
    }
    public class AboutUsData
    {
        public List<Object> desc { get; set; }
    }
    public class OurLocationData
    {
        public List<Object> data { get; set; }
    }
    public class GetInTouchData
    {
        public List<Object> getInTouch { get; set; }
    }
    public class AboutGoodLifeData
    {
        public AboutGoodLifeDataItem AboutGoodLife { get; set; }
    }
    public class GoodLifeData
    {
        public List<Object> data { get; set; }
    }
    public class FeaturedBlogData
    {
        [JsonProperty(PropertyName = "featuredBlogTitle")]
        public string Title { get; set; }
        public List<Object> data { get; set; }
    }
    public class AboutCityData
    {
        public AboutCityDataItem aboutCity { get; set; }
    }
    public class StaticTextData
    {
        public StaticTextDataItem staticText { get; set; }
    }
    public class RoomTitleData
    {
        public RoomTitleDataItem roomTitle { get; set; }
    }
    public class RoomInfoTabsData
    {
        public List<Object> roomInfoTabs { get; set; }
    }
    public class MostFacilitiesData
    {
        public List<Object> facilities { get; set; }
    }
    public class FacilitiesCategoriesData
    {
        public List<Object> facilities { get; set; }
    }
    public class otherRoomsData
    {
        public OtherRoomsDataItem otherRooms { get; set; }
    }
    public class ClubHeroBannerData
    {
        public List<Object> herobanner { get; set; }
    }
    public class AboutAdaniSocialClubData
    {
        public AboutAdaniSocialClubDataItem AboutAdaniSocialClub { get; set; }
    }
    public class ClubHighLightsData
    {
        public string title { get; set; }
        public List<object> data { get; set; }
    }
    public class AboutClubData
    {
        public AboutClubDataItem data { get; set; }
    }
    public class ConfirmBannerData
    {
        public ConfirmBannerDataItem bookingConfirmed { get; set; }
    }
    public class OrderDetailsData
    {
        public List<Object> data { get; set; }
    }
    public class SaveDetailsData
    {
        public List<Object> saveDetails { get; set; }
    }
    public class ExploreData
    {
        public ExploreDataItem Explore { get; set; }
    }
    public class ConfigurationData
    {
        public List<Object> configuration { get; set; }
    }
    #endregion
}