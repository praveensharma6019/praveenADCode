using Project.AAHL.Website.Models.Awards;
using Project.AAHL.Website.Models.Common;
using Project.AAHL.Website.Models.Home;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.Common
{
    public interface IHeaderService
    {
        Header GetHeader(Rendering rendering);
        
    }
}
