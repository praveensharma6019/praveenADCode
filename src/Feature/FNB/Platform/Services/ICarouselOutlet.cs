using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public interface ICarouselOutlet
    {
        WidgetModel GetCarouselOutletData(Sitecore.Mvc.Presentation.Rendering rendering, string storeType, string location, string terminaltype, string isApp);
    }
}
