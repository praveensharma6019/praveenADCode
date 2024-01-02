using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
    public interface IRewardsList
    {    
        WidgetModel GetRewards(Rendering rendering);
    }
}
