using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Realty.Website.Controllers;

namespace Sitecore.Realty.Website
{
    public class ServiceRegistration : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(RealtyController));
        }
    }
}