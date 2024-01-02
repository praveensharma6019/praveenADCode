using Project.Mining.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services.Banner
{
    public interface IBannerService
    {
        BannerModel GetBannerModel(Rendering rendering);
        PageBannerModel GetPageBannerModel(Rendering rendering);
    }
}
