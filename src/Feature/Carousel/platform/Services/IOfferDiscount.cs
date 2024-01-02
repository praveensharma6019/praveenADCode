using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
   public interface IOfferDiscount
    {
        //New Offer Journey Code roll back
        //WidgetModel GetOfferList(Sitecore.Mvc.Presentation.Rendering rendering,bool flag, string location,string storeType,string isExclusive,string moduleType,string isOfferAndDiscount ,string isBannerOffer,string apptype="");
        WidgetModel GetOfferList(Sitecore.Mvc.Presentation.Rendering rendering, bool flag, string location, string storeType, string isExclusive, string moduleType, string isOfferAndDiscount, string apptype = "");
    }
}
