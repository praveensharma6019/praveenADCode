using Adani.SuperApp.Airport.Feature.Avatar.Models;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.Avatar.Services
{
    public interface IProfileAvatar
    {
        List<AvatarData> GetAvatarList();
    }
}
