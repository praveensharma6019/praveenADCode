using Project.AdaniOneSEO.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public interface IFAQWidget
    {
        BannerWidgetModel GetFAQWidget(Rendering rendering);
    }
}
