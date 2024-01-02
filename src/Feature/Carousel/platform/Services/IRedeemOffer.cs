using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface IRedeemOffer
    {
        WidgetModel RedeemOfferSteps(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}
