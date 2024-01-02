using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public interface IAirportPackagesContent
    {
        Terminal GetDotNetPackages(Item item, string airportcode);
    }
}