using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.FNB.Platform.Services;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.LayoutServices
{
    public class TermsAndConditionResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ITermsAndConditionJSON termsAndConditionJSON;

        public TermsAndConditionResolver(ITermsAndConditionJSON _termsAndConditionJSON)
        {
            this.termsAndConditionJSON = _termsAndConditionJSON;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string storeType = "";
            string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].Trim().ToLower() : "";
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["sc_outletcode"]))
            {
                storeType = HttpContext.Current.Request.QueryString["sc_outletcode"].Trim().ToLower();
            }
            return termsAndConditionJSON.GetTermsAndConditionData(rendering, queryString, storeType);
        }
    }
}