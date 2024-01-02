using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public interface IPackages
    {
        Packageswidgets GetPackagesData(Rendering rendering);
        AppPackage GetPackageData(Rendering rendering, string isapp);
    }
}