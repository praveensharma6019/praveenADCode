using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services
{
    public interface IFilterProductsService
    {
        FilterProductsWidgets GetProductFilters(Sitecore.Mvc.Presentation.Rendering rendering, string queryString, string language, string airport, string storeType, bool isAirportHome, bool restricted);
    }
}