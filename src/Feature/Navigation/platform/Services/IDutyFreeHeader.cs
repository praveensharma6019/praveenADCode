using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Feature.Navigation.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.Services
{
    public interface IDutyFreeHeader
    {
        DFHeaderlwidgets GetDutyFreeHeader(Sitecore.Mvc.Presentation.Rendering rendering, string airport, string storeType, bool restricted = false, string querystring ="");
    }
}
