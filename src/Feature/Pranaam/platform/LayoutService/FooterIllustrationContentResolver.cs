using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class FooterIllustrationContentResolver : RenderingContentsResolver
    {
        private readonly ILogRepository _logRepository;
        private readonly IFooterIllustration footerIllustration;
        public FooterIllustrationContentResolver(IFooterIllustration footerIllustration, ILogRepository logRepository)
        {
            this.footerIllustration = footerIllustration;
            this._logRepository = logRepository;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return footerIllustration.GetFooterIllustartionWidget(rendering);
            }
            catch(Exception ex)
            {
                throw new Exception("CancellationResolver Resolver throws Exception -> " + ex.Message);
            }
        }
    }
}