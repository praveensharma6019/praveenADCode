using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Retail.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.LayoutServices
{
    public class RetailBrandsDetailsResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly IRetailBrandsDetails _brandsDetails;

        public RetailBrandsDetailsResolver(IRetailBrandsDetails brandsDetails)
        {
            this._brandsDetails = brandsDetails;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _brandsDetails.GetBrandsData(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_storetype"]), 
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_terminaltype"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["isApp"]));
        }
    }
}