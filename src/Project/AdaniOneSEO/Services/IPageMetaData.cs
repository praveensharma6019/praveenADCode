using Project.AdaniOneSEO.Website.Models.CityToCityPagev2;
using Project.AdaniOneSEO.Website.Models.FlightsToChennai;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniOneSEO.Website.Services
{
    public interface IPageMetaData
    {
        PageMetaDataModel GetPageMetaData(Rendering rendering);
    }
}
