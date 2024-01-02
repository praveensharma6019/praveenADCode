using Project.AmbujaCement.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services
{
    public interface IBannerService
    {
        BannerModel GetBanner(Rendering rendering);
    }
}
