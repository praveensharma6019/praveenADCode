using System;
using Adani.SuperApp.Airport.Feature.BrandListing.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.BrandListing.LayoutService
{
    public class BrandListingServices : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IBrandListing brandListing;

        public BrandListingServices(IBrandListing brandListing)
        {
            this.brandListing = brandListing;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            
            return brandListing.GetBrandListing(rendering);

        }
    }
}