using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Covid.Interfaces;
using Adani.SuperApp.Airport.Feature.Covid.Models;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Covid.Resolvers
{
    public class CovidContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICovidUpdates _covidInfo;
        public CovidContentResolver(ICovidUpdates covidInfo)
        {
            this._covidInfo = covidInfo;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _covidInfo.GetCovidInformation(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("CovidContentResolver throws Exception -> " + ex.Message);
            }
        }
    }
}