using Project.AdaniOneSEO.Website.Models.FlightsToDestination.Banner;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniOneSEO.Website.Services.FlightsToDestination
{
    public interface IBannerServices
    {
        BannerModel GetBanner(Rendering rendering);
    }
}