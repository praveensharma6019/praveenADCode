using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.Services
{
    public interface ITopHotelsWebService
    {
        WidgetModel GetTopHotelsData(Rendering rendering);
    }
}
