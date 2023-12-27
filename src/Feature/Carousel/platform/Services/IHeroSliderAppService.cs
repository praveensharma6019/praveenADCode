using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface IHeroSliderAppService
    {
        HeroCarouselwidgets GetHeroSliderData(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}