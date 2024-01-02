using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.Services
{
    public interface IRetailOutlet
    {
        WidgetModel GetRetailOutletData(Sitecore.Mvc.Presentation.Rendering rendering, string storeType, string location, string terminaltype, string outletcode, string isApp);
    }
}
