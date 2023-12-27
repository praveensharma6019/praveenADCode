using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services
{
    public interface IFlightSearch
    {
        BookFlight GetBookFlightWidgetData(Item datasource);
    }
}
