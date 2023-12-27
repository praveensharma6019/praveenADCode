using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;


namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface IThemeService
    {
        WidgetModel GetThemeData(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}
