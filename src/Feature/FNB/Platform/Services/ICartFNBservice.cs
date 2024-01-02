using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Services
{
    public interface ICartFNBservice
    {
        WidgetModel GetCartFNBAPIData(Sitecore.Mvc.Presentation.Rendering rendering, string isApp);
    }
}
