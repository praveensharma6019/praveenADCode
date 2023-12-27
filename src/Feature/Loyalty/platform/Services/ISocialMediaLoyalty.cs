using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
    public interface ISocialMediaLoyalty
    {
        LoyaltyWidgets getMediaItems(Rendering rendering);
    }
}
