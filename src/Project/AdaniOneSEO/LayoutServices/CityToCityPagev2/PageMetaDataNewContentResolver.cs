using Project.AdaniOneSEO.Website.Services.CityToCityPage;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System.Web;

namespace Project.AdaniOneSEO.Website.LayoutServices.CityToCityPagev2
{
    public class PageMetaDataNewContentResolver : RenderingContentsResolver
    {
        private readonly IPageMetaDataNew _cityToCityPageService;

        public PageMetaDataNewContentResolver(IPageMetaDataNew cityToCityPageService)
        {
            _cityToCityPageService = cityToCityPageService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string location = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("location");
            location = !string.IsNullOrEmpty(location) ? location.Replace("property-in-", "") : "";

            return _cityToCityPageService.GetPageMetaDataNew(rendering, location);
        }
    }
}