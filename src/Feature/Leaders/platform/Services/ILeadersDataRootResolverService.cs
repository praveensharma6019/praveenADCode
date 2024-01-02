using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.SuperApp.Realty.Feature.Leaders.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Realty.Feature.Leaders.Platform.Services
{
    public interface ILeadersDataRootResolverService
    {
        LeadersData GetLeadersDataList(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}
