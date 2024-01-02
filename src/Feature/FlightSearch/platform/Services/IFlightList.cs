using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services
{
   public interface IFlightList
    {
        SearchResults GetFlightListData(string queryString,string fields, string airlineType, string contextDB);
    }
}
