using Adani.EV.Project.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.EV.Project
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IArticleServices, ArticleServices>();
            serviceCollection.AddTransient<ICostCalculatorService, CostCalulatorService>();
            serviceCollection.AddTransient<ISideNavBarService, SideNavBarService>();
            serviceCollection.AddTransient<ISitemapService, SitemapService>();
            serviceCollection.AddTransient<IAboutUsService, AboutUsService>();
            serviceCollection.AddTransient<IHomeService, HomeService>();
            serviceCollection.AddTransient<ICommanService, CommanService>();
            
        }
    }
}