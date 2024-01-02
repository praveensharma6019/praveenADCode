using Adani.SuperApp.Realty.Feature.Property.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Services
{
    public interface IProperyDataBasicService
    {
        PropertyBasicInfo GetPropertyBasicInfo(Item item);
        TownshipSidebar GetTownshipSidebar(Rendering rendering);

    //    List<Amenities> GetAmenities(Item item);
        List<object> ExploreTownship(Rendering rendering);
        TownshipMasterLayout GetTownshipMasterLayout(Rendering rendering);
        AmenetiesData GetAmenities(Rendering rendering);

        FeaturesData GetFeatures(Rendering rendering);

        List<Highlights> GetHighlights(Item item);

        EmiCalculator GetEmiCalculator(Item item);

        PropertyAbout GetPropertyAbout(Item item);
        List<PropertyList> ResidentialPropertyList(Rendering rendering);

        ContactCtaDataModel GetContactCtaDataModel(Rendering rendering);

        dynamic GetProjectNameModel(Rendering rendering);

        dynamic GetNavbarTabsModel(Rendering rendering);

        PropertyHighLight GetProjectHightight(Item item);

        dynamic GetGalleryHighlightsModel(Rendering rendering);

        dynamic GetGalleryModalDataModel(Rendering rendering);

        MasterLayoutDataModel GetMasterLayoutDataModel(Rendering rendering);

        LocationDataModel GetLocationDataModel(Rendering rendering);

        SimilarProjectsDataModel GetSimilarProjectsDataModel(Rendering rendering);

        ProjectFloorPlanData GetProjectFloorPlanData(Rendering rendering);

        LivingTheGoodLifeDataModel GetLivingTheGoodLifeDataModel(Rendering rendering);

        ProjectUnitPlanData GetProjectUnitPlanData(Rendering rendering);

        dynamic GetConfigurationData(Rendering rendering);
        LayoutDataModel GetLayoutDataModel(Rendering rendering);

        PropertyFaq GetPropertyFaqData(Rendering rendering);

        ProjectActions GetProjectActions(Rendering rendering);

        AboutAdaniData GetAdaniData(Rendering rendering);
        List<CityTabs> GetCityData(Rendering rendering);
        List<CertificatesModel> GetCertificatesDetails(Rendering rendering);
        List<CompletedProjectsModel> GetCompletedProjectList(Rendering rendering);

        NRIBannerModel GetNRIBannerModel(Rendering rendering);

        WhyInvestModel GetWhyInvestModel(Rendering rendering);

        AboutNRIModel GetAboutNRIModel(Rendering rendering);

        ContentModel GetContentModel(Rendering rendering);

        OurLocationModel GetOurLocationModel(Rendering rendering);

        OurProjectModel GetOurProjectModel(Rendering rendering);

        InvestmentGuidelineModel GetInvestmentGuidelineModel(Rendering rendering);

        TestimonialModel GetTestimonialModel(Rendering rendering);

        ArticleModel GetArticleModel(Rendering rendering);

        MapLocationModel GetMapLocationData(Rendering rendering);

        AanganDrawResultModel GetDrawResultModel(Rendering rendering);

        NRIRatingSchemaModel GetNRIRatingSchemaModel(Rendering rendering);
    }
}
