using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Retail.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform.LayoutServices
{
    public class PopularCategoryResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly IPopularCategory _popularCategory;

        public PopularCategoryResolver(IPopularCategory popularCategory)
        {
            this._popularCategory = popularCategory;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _popularCategory.GetPopularCategories(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_storetype"]), 
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_terminaltype"]),
                                                             Convert.ToString(Sitecore.Context.Request.QueryString["sc_outletcode"]));
        }
    }
}