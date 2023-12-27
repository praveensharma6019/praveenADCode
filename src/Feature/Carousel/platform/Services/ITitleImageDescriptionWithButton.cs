using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface ITitleImageDescriptionWithButton
    {
        WidgetModel GetData(Rendering rendering);
    }
}