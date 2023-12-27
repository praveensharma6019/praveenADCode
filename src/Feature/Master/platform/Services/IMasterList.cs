using Adani.SuperApp.Airport.Feature.Master.Platform.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Services
{
    public interface IMasterList
    {
        /// <summary>
        /// this method has been declare and will inheritted  in MasterList class
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        MasterListModel GetMasterList(Item dataSource, string queryString);
    }
}
