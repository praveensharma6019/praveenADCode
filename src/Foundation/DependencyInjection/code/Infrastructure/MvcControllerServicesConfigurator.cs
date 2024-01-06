namespace Sitecore.Foundation.DependencyInjection.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public class MvcControllerServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("*.Feature.*");
            serviceCollection.AddClassesWithServiceAttribute("*.Feature.*");
            serviceCollection.AddMvcControllers("*.AdaniHousing.*");
            serviceCollection.AddClassesWithServiceAttribute("*.AdaniHousing.*");
            serviceCollection.AddMvcControllers("*.AdaniCapital.*");
            serviceCollection.AddClassesWithServiceAttribute("*.AdaniCapital.*");
            serviceCollection.AddMvcControllers("*.AGELPortal.*");
            serviceCollection.AddClassesWithServiceAttribute("*.AGELPortal.*");
            serviceCollection.AddClassesWithServiceAttribute("*.Foundation.*");
        }
    }
}