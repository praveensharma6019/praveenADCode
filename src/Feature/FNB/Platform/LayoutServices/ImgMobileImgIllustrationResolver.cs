using Adani.SuperApp.Airport.Feature.FNB.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.LayoutServices
{
    public class ImgMobileImgIllustrationResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly IImageMobileImageInterface _illustration;

        public ImgMobileImgIllustrationResolver(IImageMobileImageInterface footerillustration)
        {
            this._illustration = footerillustration;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            //Get the datasource for the item
            var datasource = RenderingContext.Current.Rendering.Item;
            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException();
            }
            return _illustration.Illustration(rendering,Convert.ToString(Sitecore.Context.Request.QueryString["isApp"]));
        }
    }
}