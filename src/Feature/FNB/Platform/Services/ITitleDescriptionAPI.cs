using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public interface ITitleDescriptionAPI
    {
        WidgetModel GetTitleDescriptionAPIData(Sitecore.Mvc.Presentation.Rendering rendering, string storeType, string location, string terminaltype);
    }
}
