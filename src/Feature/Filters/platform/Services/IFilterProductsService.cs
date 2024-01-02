using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Filters.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Filters.Platform.Services
{
    public interface IFilterProductsService
    {
        FilterProductsWidgets GetProductFilters(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}