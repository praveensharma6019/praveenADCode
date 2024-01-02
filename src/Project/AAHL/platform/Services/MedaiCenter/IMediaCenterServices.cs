using Project.AAHL.Website.Models.Awards;
using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Models.MediaCenter;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.MediaCenter
{
    public interface IMediaCenterServices
    {
        CenterRelease GetCenterRelease(Rendering rendering);
        CenterResources GetCenterResources(Rendering rendering);
    }
}
