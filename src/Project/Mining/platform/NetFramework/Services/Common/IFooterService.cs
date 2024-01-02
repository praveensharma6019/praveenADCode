using Project.Mining.Website.Models.Common;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Services.Common
{
    public interface IFooterService
    {
        FooterServiceModel GetFooterServiceComponent(Rendering rendering);
    }
}