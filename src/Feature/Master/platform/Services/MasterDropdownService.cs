using Adani.SuperApp.Airport.Feature.Master.Platform.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Services
{
    public class MasterDropdownService: IMasterDropdownService
    {
        public List<MasterDropdownModel> GetMasterDropdownData(Item datasource)
        {
            List<MasterDropdownModel> dropdownResult = new List<MasterDropdownModel>();
            List<Item> children = datasource.GetChildren().ToList();
            foreach(Item child in children)
            {
                dropdownResult.Add(new MasterDropdownModel
                {
                    Key = child["name"],
                    Value = child["value"]
                });
            }
            return dropdownResult;
        }
    }
}