using Project.AAHL.Website.Models.Awards;
using Project.AAHL.Website.Models.Home;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.Common
{
    public interface IAwardsService
    {
        AwardsAccolades GetAwardsAccolades(Rendering rendering);
        SearchModel GetSearch(Rendering rendering);
    }
}
