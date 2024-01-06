using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.BAU.Transmission.Feature.Navigation.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Services
{
    public interface IDutyFreeHeader
    {
        DFHeaderlwidgets GetDutyFreeHeader(Sitecore.Mvc.Presentation.Rendering rendering, bool restricted = false);
    }
}
