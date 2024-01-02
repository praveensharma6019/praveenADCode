using Adani.SuperApp.Airport.Feature.Master.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.LayoutService
{
    public class MasterListResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IMasterList masterlistservice;

        public MasterListResolver(IMasterList nationalityService)
        {
            this.masterlistservice = nationalityService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string queryString = Sitecore.Context.Request.QueryString["lists"];
            
            var datasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException();
            }

            return this.masterlistservice.GetMasterList(datasource, queryString);
        }
    }
}
