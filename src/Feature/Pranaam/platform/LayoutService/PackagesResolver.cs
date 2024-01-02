using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class PackagesResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IPackages _package;
        public PackagesResolver(IPackages package)
        {
            this._package = package;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                string isapp = Sitecore.Context.Request?.QueryString["isApp"];
                if (!string.IsNullOrEmpty(isapp))
                {
                    if (isapp == "false")
                    {
                        return _package.GetPackagesData(rendering);
                    }
                    else if (isapp == "true")
                    {
                        return _package.GetPackageData(rendering, isapp);
                    }
                    return null;
                }
                else return null;
            }
            catch (Exception ex)
            {
                throw new Exception("PackagesResolver throws Exception -> " + ex.Message);
            }
        }

    }
}