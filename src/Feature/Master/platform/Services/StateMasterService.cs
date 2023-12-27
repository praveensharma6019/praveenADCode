using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.Master.Platform.Constant;
using Adani.SuperApp.Airport.Feature.Master.Platform.Models;
using Adani.SuperApp.Airport.Feature.Master.Platform.Services;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Services
{
    public class StateMasterService : IStateMasterService
    {
        public List<StateMasterModel> GetStateMasterData(Item dataSource,string CountryId)
        {
            List<StateMasterModel> stateListData = new List<StateMasterModel>();
            List<Item> states = null;
            if (!string.IsNullOrEmpty(CountryId))
                states = dataSource.GetChildren().Where(x => x[Constants.CountryMaster].Equals(CountryId, StringComparison.OrdinalIgnoreCase)).ToList();
            else
                states = dataSource.GetChildren().ToList();
            foreach (Item state in states)
            {
                if (state != null)
                    stateListData.Add(GetStateData(state));
            }
            return stateListData;
        }

        public StateMasterModel GetStateData(Item item)
        {
            StateMasterModel state = new StateMasterModel();
            state.Id = item[Constants.Id];
            state.Import = item[Constants.Import];
            state.Name = item[Constants.Name];
            state.CountryCode = item[Constants.CountryCode];
            state.CountryMaster = item[Constants.CountryMaster];
            state.StateCode = item[Constants.StateCode];
            state.Latitude = item[Constants.Latitude];
            state.Longitude = item[Constants.Longitude];
            state.updated_State_Code = item[Constants.UpdatedStateCode];
            return state;
        }
    }
}