using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Realty.Feature.Carousel.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Realty.Feature.Carousel.Platform.Models
{
    public class mediaCoverage
    {
        public string posterSrc { get; set; }
        public string posterAlt { get; set; }
        public string posterTitle { get; set; }
        public string title { get; set; }
        public string date { get; set; }
        public string link { get; set; }
        public string linkTitle { get; set; }
        public string modalTitle { get; set; }
        public string pdfSrc { get; set; }

    }
    public class imageData
    {
        public string src { get; set; }
        public string imgType { get; set; }
        public string imgAlt { get; set; }
    }
    public class HeroCarouselwidgets
    {
        public CarosuelList widget { get; set; }
        //  public List<HeroCarousel> widgetItems { get; set; }
    }
    public class CarosuelList
    {
        public List<object> data { get; set; }
    }
    public class BannerComponent
    {
        public List<object> data { get; set; }
    }
    public class BrandIconList
    {
        public List<Object> data
        {
            get; set;
        }
    }
    public class Shantigramhighlights
    {
        public List<Cards> cards { get; set; }
        public string disclaimer { get; set; }
        public string heading { get; set; }
    }
    public class Cards
    {
        public string frontImage { get; set; }
        public string backImage { get; set; }
        public string imageAlt { get; set; }
        public string title { get; set; }
        public string cardLink { get; set; }
        public string target { get; set; }
        public string backImageAlt { get; set; }
    }
    public class ResidentialProject
    {
        public string heading { get; set; }
        public string src { get; set; }
        public string srcMobile { get; set; }
        public string alt { get; set; }
        public string imgtitle { get; set; }
        public string description { get; set; }
        public string descriptionReadmore { get; set; }
        public string readmore { get; set; }
        public string readless { get; set; }
    }
    public class ClubLandingModel
    {
      public List<clubLanding> clubLanding { get; set; }
    }
    public class clubLanding
    {
        public string clubLogo { get; set; }
        public string clubLink { get; set; }
        public string clubLogoAlt { get; set; }
        public string clubName { get; set; }
        public string clubAddress { get; set; }
        public string clubAbout { get; set; }
        public string artisticDisclaimer { get; set; }
        public string readmore { get; set; }
        public string readless { get; set; }
        public List<ClubCarosuel> data { get; set; }
    }
    public class ClubCarosuel
    {
        public string clubImage { get; set; }
        public string clubAlt { get; set; }
        public string type { get; set; }
        public string imgtype { get; set; }
    }

    public class ProppertiesList
    {
        public List<Object> carouselImages { get; set; }
        public PropertiesItemdata data { get; set; }

    }
    public class ProjectFoundStatus
    {
        public string locationHeading { get; set; }
        public string projectsfound { get; set; }
        public string projectfound { get; set; }
        public string city { get; set; }
    }
    public class newClass
    {
        public List<ProppertiesList> listofproperty { get; set; }
    }
    public class propertyDetails
    {
        public List<carouselImagesItem> Items { get; set; }
    }
    public class propertyDetailsdata
    {
        public PropertiesItemdata data { get; set; }
    }
    public class OfficeData
    {
        public string heading { get; set; }
        public List<OfficeItem> data { get; set; }
    }
    public class ContactUsPageData
    {
        public string PageData { get; set; }
    }
    public class OfficeItem
    {
        public string src { get; set; }
        public string title { get; set; }
        public string address { get; set; }
    }
    public class PropertiesItemdata
    {
        public string PropertyID { get; set; }
        public string link { get; set; }
        public string linktarget { get; set; }
        public string PropertyLocation { get; set; }
        public string rera { get; set; }
        public string propertyStatus { get; set; }
        public string propertyStatusID { get; set; }
        public string projectName { get; set; }
        public string projectSpec { get; set; }
        public string areaLabel { get; set; }
        public string priceLabel { get; set; }
        public string areaDetail { get; set; }
        public string priceDetail { get; set; }
        public string onwards { get; set; }
        public int propertyPricefilter { get; set; }
        public string propertyPrice { get; set; }
        public List<string> propertyConfiguration { get; set; }
        public string propertyType { get; set; }
        public List<string> propertyConfigurationFilter { get; set; }
        public string propertyStatusFilter { get; set; }
        public string propertyTypeFilter { get; set; }
        public string condition { get; set; }
        public string township { get; set; }


    }
    public class carouselImagesItem
    {
        public string src { get; set; }
        public string alt { get; set; }
        public string porjectLogo { get; set; }
        public string porjectLogoAlt { get; set; }
        public string imageDesc { get; set; }
    }

    public class BranIconItem
    {
        public string src { get; set; }
        public string imgAlt { get; set; }
        public string location { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string projectcount { get; set; }
        public string link { get; set; }
        public string mobileimage { get; set; }

    }

    /// <summary>
    /// Class to get the Material Groups
    /// </summary>
    public class HeroCarousel
    {
        public string title { get; set; }

        public string imageSrc { get; set; }

        public string description { get; set; }

        public string ctaLink { get; set; }

        public string deepLink { get; set; }

        public string subTitle { get; set; }

    }
    public class RealityCarousel
    {
        public string isVideo { get; set; }
        public string logo { get; set; }
        public string logoalt { get; set; }
        public string logotitle { get; set; }
        public string heading { get; set; }
        public string subheading { get; set; }
        public string link { get; set; }
        public string linktarget { get; set; }
        public string linktitle { get; set; }
        public string rerano { get; set; }
        public string imgtype { get; set; }
        public string thumb { get; set; }
        public string thumbalt { get; set; }
        public string thumbtitle { get; set; }
        public string videoposter { get; set; }
        public string videoMp4 { get; set; }
        public string videoOgg { get; set; }
        public string videoposterMobile { get; set; }
        public string videoMp4Mobile { get; set; }
        public string videoOggMobile { get; set; }
        public string srcMobile { get; set; }
        public string propertyType { get; set; }
        public string propertyName { get; set; }
        public string SEOName { get; set; } 
        public string SEODescription { get; set; } 
        public string UploadDate { get; set; } 

    }
    public class Banner
    {
        public string src { get; set; }
        public string title { get; set; }
        public string alt { get; set; }
        public string thumb { get; set; }
        public string srcMobile { get; set; }
        public string headerdesc { get; set; }
        public string buttontext { get; set; }
        public string class1 { get; set; }
        public string class2 { get; set; }
        public string emailLink { get; set; }
    }
    public class AboutUsBanner
    {
        public string leftImg { get; set; }
        public string imgAlt { get; set; }
        public string imgTitle { get; set; }
        public string rightImg { get; set; }
        public string heading { get; set; }
        public string subHeading { get; set; }
        public string description { get; set; }
    }
    public class amenities
    {
        public string mainHeading { get; set; }
        public string id { get; set; }
        public List<dataList> dataList { get; set; }
    }
    public class dataList
    {
        public string sectionHeading { get; set; }
        public string subHeading { get; set; }
        public string description { get; set; }
        public string imageSource { get; set; }
        public string imageSourceMobile { get; set; }
        public string imageSourceTablet { get; set; }
        public string imageAlt { get; set; }

    }

}