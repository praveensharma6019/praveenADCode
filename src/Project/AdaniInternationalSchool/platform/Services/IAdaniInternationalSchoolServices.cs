using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website
{
    public interface IAdaniInternationalSchoolServices
    {
        GalleryContentModel<LifeAtSchoolGalleryItem> GetLifeAtSchool(Rendering rendering);
        BaseCards<OurApproachModel> GetOurApproach(Rendering rendering);

        OurInfrastructure GetOurInfrastructure(Rendering rendering);
        GalleryContentModel<StoriesGallery> GetHomeStories(Rendering rendering);

        BaseCards<AdmissionCardGallery> GetAdmissionCard(Rendering rendering);

        HomeFAQ GetHomeFAQ(Rendering rendering);
        BaseCards<WelcomeCardItemModel> GetWelcomeCard(Rendering rendering);
        object GetHomeConditions(Rendering rendering);

        HomeMainBanner GetHomeMainBanner(Rendering rendering);

        HomeCurriculum GetCurriculum(Rendering rendering);

        BaseContentModel<VisionMissionCardItem> GetVisionMissionCard(Rendering rendering);

        BaseHeadingModel<CoreValuesGallery> GetCoreValues(Rendering rendering);

        DescriptionModel GetLegal(Rendering rendering);
        MandatoryPublicDisclosure GetMandatoryPublicDisclosure(Rendering rendering);
        OverviewMethod GetOverviewMethod(Rendering rendering);
        BaseContentModel GetOverview(Rendering rendering);

        MainBanner GetMainBanner(Rendering rendering);
        SubNav GetSubNav(Rendering rendering);
        VideoBanner GetVideoBanner(Rendering rendering);
        SearchData GetFaqsSearchData(Rendering rendering);
        BaseCards<FounderCardDataModel> GetFounderCard(Rendering rendering);
        PoliciesPageLinkSection GetPoliciesPageLinkSection(Rendering rendering);
        BaseHeadingModel<ProgramGalleryList> GetProgram(Rendering rendering);
        BaseHeadingModel<List<HolisticSubListItem>> GetHolistic(Rendering rendering);

       
        TransportModel GetTransportRouteDetails(Rendering rendering);
        BaseContentModel<AffiliationItem> GetAffiliation(Rendering rendering);
        LatestStories GetLatestStories(Rendering rendering);
        InfrastructureCategory GetInfrastructureCategory(Rendering rendering);
        InfrastructureDetailPage GetInfrastructureDetail(Rendering rendering);
        Cookies GetCookies(Rendering rendering);
       


        WhyUsImageBanner GetWhyUsImageBanner(Sitecore.Mvc.Presentation.Rendering rendering);


        //CareerFormRoot GetCareerForm(Sitecore.Mvc.Presentation.Rendering rendering);
        CareerFormData GetCareerForm(Sitecore.Mvc.Presentation.Rendering rendering);
        List<SiteMapXML> GetSiteMapXML(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}
