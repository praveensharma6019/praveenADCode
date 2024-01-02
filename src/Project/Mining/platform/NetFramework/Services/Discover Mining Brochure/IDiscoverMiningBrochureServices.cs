using Project.Mining.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services.OurProjects
{
    public interface IDiscoverMiningBrochureServices
    {
        DiscoverMiningBrochureModel GetDiscoverMiningBrochure(Rendering rendering);
    }
}