using Adani.SuperApp.Airport.Feature.FNB.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.LayoutServices
{
    public class FnBSapAPIResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly ISapAPI _sapApi;

        public FnBSapAPIResolver(ISapAPI sapApi)
        {
            this._sapApi = sapApi;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
                string storeType = "";
                string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].Trim().ToLower() : "";
                if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["sc_storetype"]))
                {
                    storeType = HttpContext.Current.Request.QueryString["sc_storetype"].Trim().ToLower();
                }
                return _sapApi.GetSapAPIData(rendering, queryString, storeType);
        }
    }
}