using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.Services
{
    public interface IHotelsHeroCarouselWeb
    {
        WidgetModel GetHeroCarouselData(Rendering rendering);
    }
}
