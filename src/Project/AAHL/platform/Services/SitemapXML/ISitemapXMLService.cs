using Project.AAHL.Website.Models.SitemapXML;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.SitemapXML
{
    public interface ISitemapXMLService
    {
        SiteMapXML GetSiteMapXML(Rendering rendering);
    }
}
