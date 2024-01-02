using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public interface IFAQ
    {
        WidgetModel GetFAQList(Sitecore.Mvc.Presentation.Rendering rendering,  string location);
    }
}
