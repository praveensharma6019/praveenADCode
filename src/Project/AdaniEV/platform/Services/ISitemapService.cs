using Adani.EV.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.EV.Project.Services
{
    public interface ISitemapService
    {
        SitemapModel GetSitemapModel(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}
