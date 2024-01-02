using Adani.SuperApp.Realty.Feature.Configuration.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.LayoutService
{
    public class MasterListResolver : RenderingContentsResolver
    {
        private readonly IMasterListService masterlistservice;

        public MasterListResolver(IMasterListService nationalityService)
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
