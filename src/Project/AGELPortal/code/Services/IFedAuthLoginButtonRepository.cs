using Sitecore.AGELPortal.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AGELPortal.Website.Services
{
    public interface IFedAuthLoginButtonRepository
    {
        IEnumerable<FedAuthLoginButton> GetAll();
        string GetAllAAD();
    }
}