using Adani.SuperApp.Airport.Feature.SiteMap.Platform.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.SiteMap.Platform.Services
{
    public interface ISiteMap
    {
        SiteMapModel DomesticFlightsSitemapData(Item datasource);

        SiteMapModel InternationalFlightsSitemapData(Item datasource);

        SiteMapModel DomesticAirlinesSitemapData(Item datasource);

        SiteMapModel InternationalAirlinesSitemapData(Item datasource);
    }
}
