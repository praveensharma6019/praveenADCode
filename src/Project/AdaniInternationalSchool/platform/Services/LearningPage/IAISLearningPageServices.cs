using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.LearningPage
{
    public interface IAISLearningPageServices
    {
        LearningStoryList GetLearningStoryList(Rendering rendering);
        BaseContentModel<FindOutMoreDataModel> GetFindOutMore(Rendering rendering);
        SteamCard GetSteamCard(Rendering rendering);
        FacilitiesModel GetFacilities(Rendering rendering);
        SectionCards<CommunicationCardDataModel> GetCommunicationCard(Rendering rendering);
        SectionCards<CarouselDataModel> GetTechnologyInnovation(Rendering rendering);
    }
}
