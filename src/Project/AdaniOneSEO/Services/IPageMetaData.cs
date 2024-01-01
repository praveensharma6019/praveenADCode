using Project.AdaniOneSEO.Website.Models.FlightsToChennai;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.AdaniOneSEO.Website.Services
{
    public interface IPageMetaData
    {
        PageMetaDataModel GetPageMetaData(Rendering rendering);
    }
}
