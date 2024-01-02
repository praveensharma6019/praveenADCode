using Project.Mining.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services.Header
{
    public interface IHeaderService
    {
        HeaderModel GetHeader(Rendering rendering);
    }
}
