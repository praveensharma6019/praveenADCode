using Adani.SuperApp.Realty.Feature.Navigation.Platform.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Services
{
    public interface IFooterService
    {
        Footer GetFooterData(Rendering rendering , string location);
        List<FooterHeading> GetConfigurationData(Rendering rendering);
    }
}