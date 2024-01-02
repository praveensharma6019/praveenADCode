using Project.AmbujaCement.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.SitemapXML
{
    public interface ISitemapXMLService
    {
        SiteMapXML GetSiteMapXML(Rendering rendering);
    }
}
