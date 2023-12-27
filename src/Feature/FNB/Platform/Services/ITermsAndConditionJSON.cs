using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public interface ITermsAndConditionJSON
    {
        WidgetModel GetTermsAndConditionData(Rendering rendering, string queryString, string storeType);
    }
}