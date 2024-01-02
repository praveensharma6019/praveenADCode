using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.LayoutService
{
    public class LoyaltyRewardsBannerResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
	{
		public readonly ILoyalty loyalty;
		public LoyaltyRewardsBannerResolver(ILoyalty _loyalty)
		{
			this.loyalty = _loyalty;
		}
		public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
		{

			return loyalty.getLoyaltyData(rendering);

		}
	}
}