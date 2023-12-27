using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
    public interface ITerminal
    {
        WidgetModel GetTerminal(Sitecore.Mvc.Presentation.Rendering rendering, string Location);
    }
}
