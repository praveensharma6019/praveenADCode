using System;
using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class PackageServicesContentResolver : RenderingContentsResolver
    {
        private readonly IPackageServices _service;
        public PackageServicesContentResolver(IPackageServices service)
        {
            this._service = service;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                string isapp = Sitecore.Context.Request?.QueryString["isApp"];
                if (!string.IsNullOrEmpty(isapp) && isapp == "false")
                {
                    return _service.GetPackageServiceData(rendering);
                }
                else return null;
                //return _service.GetPackageServiceData(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("PackageServicesContentResolver throws Exception -> " + ex.Message);
            }
        }
    }
}