using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.LayoutService
{
    public class LoyaltyRewardsJourneyResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
	{
		public readonly ILoyaltyRewardJourney loyaltyRewardJourney;
		public LoyaltyRewardsJourneyResolver(ILoyaltyRewardJourney _loyaltyRewardJourney)
		{
			this.loyaltyRewardJourney = _loyaltyRewardJourney;
		}
		public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
		{

			return loyaltyRewardJourney.getLoyaltyRewardJourneyData(rendering);

		}
	}
}