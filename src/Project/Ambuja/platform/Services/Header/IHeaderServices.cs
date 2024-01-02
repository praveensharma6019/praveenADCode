using Project.AmbujaCement.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.Header
{
    public interface IHeaderServices
    {
        HeaderModel GetHeader(Rendering rendering);
    }
}
