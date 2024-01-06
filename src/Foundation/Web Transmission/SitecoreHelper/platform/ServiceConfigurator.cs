using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform
{
    public class ServiceConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Helper.IHelper, Helper.Helper>();
        
        }

    }

}