using Adani.SuperApp.Airport.Feature.Covid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Covid.Interfaces
{
    public interface ICovidUpdates
    {
        Covidwidgets GetCovidInformation(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}