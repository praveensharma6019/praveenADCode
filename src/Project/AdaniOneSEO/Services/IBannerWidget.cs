using Project.AdaniOneSEO.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.Services
{
    public interface IBannerWidget
    {
        BannerWidgetModel GetBannerWidget(Rendering rendering);
    }
}
