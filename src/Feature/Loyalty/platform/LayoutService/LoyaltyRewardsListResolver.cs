using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;
using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.LayoutService
{
    public class LoyaltyRewardsListResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
		public readonly IRewardsList rewardsList;
		public LoyaltyRewardsListResolver(IRewardsList _rewardsList)
		{
			this.rewardsList = _rewardsList;
		}
		public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
		{
			return rewardsList.GetRewards(rendering);
		}
	}
}