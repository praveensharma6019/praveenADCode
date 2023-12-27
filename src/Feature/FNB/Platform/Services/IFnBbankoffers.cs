using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public interface IFnBbankoffers
    {
        WidgetModel GetFnBbankoffersData(Sitecore.Mvc.Presentation.Rendering rendering, string storeType, string location, string terminaltype, string outletcode);
    }
}
