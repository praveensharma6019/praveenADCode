using Project.AmbujaCement.Website.Models.SiteMap;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.Sitemap
{
    public interface ISitemapService
    {
        SitemapDataModel GetSitemapDataModel(Rendering rendering);
    }
}
