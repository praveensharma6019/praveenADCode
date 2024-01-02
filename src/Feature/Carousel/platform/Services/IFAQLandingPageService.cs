using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface IFAQLandingPageService
    {
        WidgetModel GetFAQData(Rendering rendering);
    }
}