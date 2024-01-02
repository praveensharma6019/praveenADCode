using Adani.SuperApp.Realty.Feature.Leaders.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Leaders.Platform.LayoutService
{
    public class AchievementsContentsResolver : RenderingContentsResolver
    {
        protected readonly IAchievementsRootResolverService RootResolver;

        public AchievementsContentsResolver(IAchievementsRootResolverService achievementsService)
        {
            this.RootResolver = achievementsService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
           return RootResolver.GetAchievementsList(rendering);          
        }
    
    }
}