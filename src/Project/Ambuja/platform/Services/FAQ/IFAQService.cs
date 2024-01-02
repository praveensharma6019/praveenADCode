using Project.AmbujaCement.Website.Models.AboutUsPage;
using Project.AmbujaCement.Website.Models.DealerResult;
using Project.AmbujaCement.Website.Models.FAQ;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.FAQ
{
    public interface IFAQService
    {
        FAQModel GetFAQModel(Rendering rendering);
    }
}
