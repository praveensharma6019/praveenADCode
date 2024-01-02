using Project.AAHL.Website.Models.AboutUs;
using Project.AAHL.Website.Models.Common;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.AboutUs
{
    public interface IAboutUsServices
    {
        BannerModel GetBanner(Rendering rendering);
        DetailModel GetDetails(Rendering rendering);
        AboutAAHLModel GetaboutAAHL(Rendering rendering);
        StoryModel GetStory(Rendering rendering);
        OurLeadershipModel GetOurLeadership(Rendering rendering);
        OurStrategyModel GetStrategy(Rendering rendering);
    }
}