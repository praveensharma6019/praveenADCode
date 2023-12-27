using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Adani.SuperApp.Airport.Feature.FNB.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
   public interface IImageMobileImageInterface
    {
        WidgetModel Illustration(Sitecore.Mvc.Presentation.Rendering rendering, string isApp);
    }
}