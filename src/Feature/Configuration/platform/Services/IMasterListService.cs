using Adani.SuperApp.Realty.Feature.Configuration.Platform.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Services
{
    public interface  IMasterListService
    {
        MasterListModel GetMasterList(Item dataSource, string queryString);
    }
}