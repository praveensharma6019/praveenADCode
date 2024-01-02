using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Controllers;
using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.DependencyInjection;
using PNRFormController = Adani.SuperApp.Airport.Feature.StaticPages.Platform.Controllers.PNRFormController;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {


            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(FeebackFormController),
                typeof(FeebackFormController)));

            serviceCollection.AddTransient<IFeedbackRepository, FeedbackRepository>();

            serviceCollection.AddTransient<Services.IContactUsFormService, Services.ContactUsFormService>();            

            serviceCollection.AddScoped<IAPIResponse, APIResponse>();

            serviceCollection.AddScoped<ILogRepository, LogRepository>();

            serviceCollection.AddScoped<IHelper, Helper>();
            serviceCollection.AddScoped<Services.IPNRRepository, Services.PNRFormRepository>();
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(PNRFormController),
    typeof(PNRFormController)));
            serviceCollection.AddScoped<Services.ICallBackRepository, Services.CallBackRepository>();
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(CallBackFormController),
                typeof(CallBackFormController)));
        }
    }
}