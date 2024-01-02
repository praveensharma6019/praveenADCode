using Project.AdaniOneSEO.Website.Models.CityToCityPagev2;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public interface IPageMetaDataNew
    {
        PageMetaDataModelNew GetPageMetaDataNew(Rendering rendering, string location = null);
    }
}
