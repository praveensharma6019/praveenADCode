using Adani.EV.Project.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.EV.Project.Services
{
    public interface ISideNavBarService
    {
        SidebarModel GetSideNavbarModel(Rendering rendering);
    }
}
