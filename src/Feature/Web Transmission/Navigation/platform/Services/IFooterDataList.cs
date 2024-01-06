using Adani.BAU.Transmission.Feature.Navigation.Platform.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Services
{
   public interface IFooterDataList
    {
        FooterData GetFooterData(Item datasource);
    }
}
