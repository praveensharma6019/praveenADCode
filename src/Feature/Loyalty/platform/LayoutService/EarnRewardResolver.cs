using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.LayoutService
{
    public class EarnRewardResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IEarnReward earnReward;
        public EarnRewardResolver(IEarnReward _earnRewards)
        {
            this.earnReward = _earnRewards;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return earnReward.getEarrewardData(rendering);

        }
    }
}