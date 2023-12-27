using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public interface IOutletStatus
    {
        WidgetModel GetOutletStatusData(Sitecore.Mvc.Presentation.Rendering rendering, string queryString, string storeType);
    }
}
