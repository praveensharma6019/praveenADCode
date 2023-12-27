using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
    public interface IEarn2XProduct
    {
        LoyaltyWidgets get2xLoyaltyProductData(Sitecore.Mvc.Presentation.Rendering rendering,string type);
    }
}
