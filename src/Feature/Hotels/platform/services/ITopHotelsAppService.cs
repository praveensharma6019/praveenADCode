using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.Services
{
    public interface ITopHotelsAppService
    {
        WidgetModel GetTopHotelsData(Rendering rendering);
    }
}
