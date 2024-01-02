using Adani.SuperApp.Realty.Feature.Sitemap.Platform.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Realty.Feature.Sitemap.Platform.Services
{
    public interface ISitemapRootResolverService
    {
        SitemapData GetSitemapDataItemList(Rendering rendering);
        List<DynamicSitemap> GetDynamicSitemapData(Rendering rendering);

        ConfigurationData GetConfigurationItemList(Rendering rendering);
    }
}
