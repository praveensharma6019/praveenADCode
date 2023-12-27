using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Master.Platform.Models;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Services
{
    public interface IStateMasterService
    {
        List<StateMasterModel> GetStateMasterData(Item dataSource, string CountryId);
    }
}