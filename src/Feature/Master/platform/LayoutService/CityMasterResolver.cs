using Adani.SuperApp.Airport.Feature.Master.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.LayoutService
{
    public class CityMasterResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICityMasterService cityMasterService;

        public CityMasterResolver(ICityMasterService cityMasterService)
        {
            this.cityMasterService = cityMasterService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
           
            string StateCode = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["statecode"])? Sitecore.Context.Request.QueryString["statecode"] :"";
            string CountryCode = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["countrycode"]) ? Sitecore.Context.Request.QueryString["countrycode"] : "";
            string contextdb = Sitecore.Context.Database.ToString();
            return this.cityMasterService.GetCityMasterData(StateCode, CountryCode, contextdb);
        }
    }
}