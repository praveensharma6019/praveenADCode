using Adani.SuperApp.Airport.Feature.Avatar.Controlles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Feature.Avatar
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(ProfileAvatarController),
                typeof(ProfileAvatarController)));
            serviceCollection.AddTransient<Services.IProfileAvatar, Services.ProfileAvatar>();
        }
    }
}