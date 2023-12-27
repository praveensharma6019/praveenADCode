using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Services
{
    public interface IFAQ
    {
        WidgetModel GetFAQList(Sitecore.Mvc.Presentation.Rendering rendering,  string location);
    }
}
