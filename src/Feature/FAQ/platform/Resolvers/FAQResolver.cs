using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.FAQ.Interfaces;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.FAQ.Resolvers
{
    public class FAQResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IFAQ _faq;
        public FAQResolver(IFAQ faq)
        {
            this._faq = faq;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _faq.GetFaqData(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("FAQResolver throws Exception -> " + ex.Message);
            }
        }
    }
}