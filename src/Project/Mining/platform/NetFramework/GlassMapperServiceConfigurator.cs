using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web;
using Glass.Mapper.Sc.Web.Mvc;
using Glass.Mapper.Sc.Web.WebForms;
using Microsoft.Extensions.DependencyInjection;
using Sitecore;
using Sitecore.Data;
using Sitecore.DependencyInjection;

namespace Project.Mining.Website
{
    public class GlassMapperServiceConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISitecoreService>(sp => new SitecoreService(Context.Database));
            serviceCollection.AddTransient<IMvcContext>(sp => new MvcContext(sp.GetService<ISitecoreService>()));
            serviceCollection.AddTransient<IRequestContext>(sp => new RequestContext(sp.GetService<ISitecoreService>()));
            serviceCollection.AddTransient<IWebFormsContext>(sp => new WebFormsContext(sp.GetService<ISitecoreService>()));
        }
    }
}