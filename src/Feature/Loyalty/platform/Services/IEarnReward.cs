using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
    public interface IEarnReward
    {
        LoyaltyWidgets getEarrewardData(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}
