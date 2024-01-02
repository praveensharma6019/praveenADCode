using Project.Mining.Website.Models.Sitemap;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services.Sitemap
{
    public interface ISitemapService
    {
        SitemapModel GetSitemapData(Rendering rendering);
    }
}
