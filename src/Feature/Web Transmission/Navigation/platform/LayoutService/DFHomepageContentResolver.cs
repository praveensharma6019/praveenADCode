using System;
using Adani.BAU.Transmission.Feature.Navigation.Platform.Services;
using Adani.BAU.Transmission.Foundation.Logging.Platform.Repositories;
//using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;



namespace Adani.BAU.Transmission.Feature.Navigation.Platform.LayoutService
{
    //public class DFHomepageContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    //{
    //    private readonly IDutyFreeHeader dutyFreeHeader;
    //    private readonly ILogRepository _logRepository;

        
    //    public DFHomepageContentResolver(IDutyFreeHeader dutyFreeHeader, ILogRepository logRepository)
    //    {
    //        this.dutyFreeHeader = dutyFreeHeader;
    //        this._logRepository = logRepository;
    //    }
    //    public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
    //    {
            
    //        _logRepository.Info("Navigation resolver called");
    //        bool restricted = false;
    //        if(!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["sc_restricted"]))
    //        {
    //            restricted = Convert.ToBoolean(System.Web.HttpContext.Current.Request.QueryString["sc_restricted"]);
    //        }
    //        return this.dutyFreeHeader.GetDutyFreeHeader(rendering, restricted);           
    //    }

    //}
}