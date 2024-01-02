using Project.AdaniOneSEO.Website.Models.CityToCityBannerWidget;
using Project.AdaniOneSEO.Website.Models.CityToCityPagev2;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public interface ICityToCityBannerWidgetService
    {
        CityToCityBannerWidgetModel GetCityToCityBannerWidgetModel(Rendering rendering);
    }
}