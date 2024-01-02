using Project.AdaniOneSEO.Website.Models.SitemapXml;
using Project.AdaniOneSEO.Website.Models.VideoGallery;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.Services.SitemapXml
{
    public interface ISitemapXmlService
    {
        SitemapXmlModel GetSiteMapXML(Rendering rendering);
        
    }
}
