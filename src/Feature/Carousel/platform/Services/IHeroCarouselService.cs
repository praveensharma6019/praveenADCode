using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface IHeroCarouselService
    {
        HeroCarouselwidgets GetHeroCarouseldata(Sitecore.Mvc.Presentation.Rendering rendering, string queryString, string storeType, string airport, bool restricted);
    }
}
