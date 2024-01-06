using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Feature.ValidateGoogleCaptcha.Services;

namespace Sitecore.Feature.ValidateGoogleCaptcha
{
    public class ServiceConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IValidateCaptchaService, ValidateCaptchaService>();
        }
    }
}