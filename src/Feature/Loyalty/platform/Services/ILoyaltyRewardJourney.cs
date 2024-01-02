using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
	public interface ILoyaltyRewardJourney
	{
		LoyaltyRewardsJourneyWidget getLoyaltyRewardJourneyData(Sitecore.Mvc.Presentation.Rendering rendering);
	}
}