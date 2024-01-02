using Project.AmbujaCement.Website.Models.AboutUsPage;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.AboutUs
{
    public interface IAboutUsService
    {
        AchievementsModel GetAchievementsModel(Rendering rendering);
        MessageCardModel GetMessageCardModel(Rendering rendering);
        VisionMissionCardModel GetVisionMissionCardModel(Rendering rendering);
        ICanCardsModel GetICanCardsModel(Rendering rendering);
    }
}
