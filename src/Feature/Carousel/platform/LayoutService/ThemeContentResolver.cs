using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class ThemeContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IThemeService theme;
        public ThemeContentResolver(IThemeService _theme)
        {
            this.theme = _theme;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return theme.GetThemeData(rendering);
        }
    }
}