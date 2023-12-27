using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class TitleImageDescriptionWithButtonResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ITitleImageDescriptionWithButton _titleImageDescriptionWithButton;
        public TitleImageDescriptionWithButtonResolver(ITitleImageDescriptionWithButton titleImageDescriptionWithButton)
        {
            this._titleImageDescriptionWithButton = titleImageDescriptionWithButton;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _titleImageDescriptionWithButton.GetData(rendering);
        }
    }
}