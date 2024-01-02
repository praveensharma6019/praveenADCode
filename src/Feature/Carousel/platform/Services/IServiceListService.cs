using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface IServiceListService
    {
        WidgetModel GetServiceListData(Rendering rendering);
    }
}