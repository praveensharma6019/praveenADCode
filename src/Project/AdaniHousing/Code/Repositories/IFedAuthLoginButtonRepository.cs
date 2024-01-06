using Sitecore.AdaniHousing.Website.Models;
using System.Collections.Generic;

namespace Sitecore.AdaniHousing.Website.Repositories
{
    public interface IFedAuthLoginButtonRepository
    {
        IEnumerable<FedAuthLoginButton> GetAll();
    }
}