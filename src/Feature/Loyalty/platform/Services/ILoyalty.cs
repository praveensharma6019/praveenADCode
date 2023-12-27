using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;
using Sitecore.Data.Fields;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
   public interface ILoyalty
    {
        LoyaltyWidgets getLoyaltyData(Sitecore.Mvc.Presentation.Rendering rendering);
        List<Rewards> getRewardsList(MultilistField multilistField);
    }
}
