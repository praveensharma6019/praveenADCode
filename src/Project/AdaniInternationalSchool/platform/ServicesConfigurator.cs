using Project.AdaniInternationalSchool.Website.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Project.AdaniInternationalSchool.Website.AdmissionPage;
using Project.AdaniInternationalSchool.Website.Services.AdmissionPage;
using Project.AdaniInternationalSchool.Website.LearningPage;
using Project.AdaniInternationalSchool.Website.Services.LearningPage;
using Project.AdaniInternationalSchool.Website.Services.LifeAtSchoolPage;
using Project.AdaniInternationalSchool.Website.Services.EnrollNow;
using Project.AdaniInternationalSchool.Website.Services.AboutUs;
using Project.AdaniInternationalSchool.Website.Services.Academics;
using Project.AdaniInternationalSchool.Website.Services.ContentResolver;
using Project.AdaniInternationalSchool.Website.Services.Footer;
using Project.AdaniInternationalSchool.Website.Services.Header;
using Project.AdaniInternationalSchool.Website.Services.Text;

namespace Project.AdaniInternationalSchool.Website
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAdaniInternationalSchoolServices, AdaniInternationalSchoolServices>();
            serviceCollection.AddTransient<IAISLearningPageServices, AISLearningPageServices>();
            serviceCollection.AddTransient<IAISAdmissionPageServices, AISAdmissionPageServices>();
            serviceCollection.AddTransient<IAISLifeAtSchoolPageServices, AISLifeAtSchoolPageServices>();
            serviceCollection.AddTransient<IHeaderService, HeaderService>();
            serviceCollection.AddTransient<IFooterService, FooterService>();
            serviceCollection.AddTransient<IMainCardService, MainCardService>();
            serviceCollection.AddTransient<IMainCardV2Service, MainCardV2Service>();
            serviceCollection.AddTransient<ICardListService, CardListService>();
            serviceCollection.AddTransient<ITextService, TextService>();
            serviceCollection.AddTransient<IAcademicsService, AcademicsService>();
            serviceCollection.AddTransient<IContentResolverService, ContentResolverService>();
            serviceCollection.AddTransient<IAboutUsService, AboutUsService>();
            serviceCollection.AddTransient<IEnrollNowService, EnrollNowService>();
            serviceCollection.AddTransient<IUpcomingEventsService, UpcomingEventsService>();
            serviceCollection.AddTransient<IAllActivitiesService, AllActivitiesService>();
        }
    }
}