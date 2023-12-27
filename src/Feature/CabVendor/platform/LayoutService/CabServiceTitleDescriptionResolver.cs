using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.LayoutServices
{
    public class CabServiceTitleDescriptionResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICabServiceTitleDescription _serviceDetail;

        public CabServiceTitleDescriptionResolver(ICabServiceTitleDescription serviceDetail)
        {
            this._serviceDetail = serviceDetail;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _serviceDetail.GetServiceDetails(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["airportCode"]));
            }
            catch (Exception ex)
            {
                throw new Exception("CabServiceTitleDescriptionResolver throws Exception -> " + ex.Message);
            }
        }
    }
}