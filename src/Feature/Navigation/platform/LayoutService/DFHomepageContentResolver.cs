using System;
using Adani.SuperApp.Airport.Feature.Navigation.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;



namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.LayoutService
{
    public class DFHomepageContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IDutyFreeHeader dutyFreeHeader;
        private readonly ILogRepository _logRepository;

        
        public DFHomepageContentResolver(IDutyFreeHeader dutyFreeHeader, ILogRepository logRepository)
        {
            this.dutyFreeHeader = dutyFreeHeader;
            this._logRepository = logRepository;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            
            _logRepository.Info("Navigation resolver called");
            bool restricted = false;
            if(!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["sc_restricted"]))
            {
                restricted = Convert.ToBoolean(System.Web.HttpContext.Current.Request.QueryString["sc_restricted"]);
            }
            string querystring = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].Trim().ToLower() : "";
            string airport = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_airport"]) ? Sitecore.Context.Request.QueryString["sc_airport"].Trim().ToLower() : "BOM";
            string storeType = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_storetype"]) ? Sitecore.Context.Request.QueryString["sc_storetype"].Trim().ToLower() : "departure";
            return this.dutyFreeHeader.GetDutyFreeHeader(rendering, airport, storeType, restricted, querystring);           
        }

    }
}