using Adani.SuperApp.Airport.Feature.Master.Platform.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Services
{
    public interface ICityMasterService
    {
        SearchResults GetCityMasterData(string StateCode, string CountryCode, string contextdb);
    }
}