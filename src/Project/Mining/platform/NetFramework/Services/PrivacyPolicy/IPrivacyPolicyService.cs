using Project.Mining.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services
{
    public interface IPrivacyPolicyService
    {
        PrivacyPolicy GetPrivacyDetail(Rendering rendering);
        TermsandConditions GetTermsDetail(Rendering rendering);
    }
}
