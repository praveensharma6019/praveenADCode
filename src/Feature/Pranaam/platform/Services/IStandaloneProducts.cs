using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public interface IStandaloneProducts
    {
        StandaloneProductTerminal GetPorterDetails(Item item, string airportcode);
    }
}