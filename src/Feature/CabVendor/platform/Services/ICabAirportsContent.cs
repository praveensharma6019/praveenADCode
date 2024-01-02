using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Services
{
    public interface ICabAirportsContent
    {
        AirportList GetCabAirports(Item datasource);
    }
}
