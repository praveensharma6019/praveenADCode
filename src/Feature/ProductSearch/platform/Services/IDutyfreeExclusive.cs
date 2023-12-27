using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services
{
    public interface IDutyfreeExclusive
    {
        ExclusiveOffersWidgets GetExclusiveOffers(Rendering rendering, string queryString, string language, string airport, string storeType);
    }
}