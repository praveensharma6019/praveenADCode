using Project.AmbujaCement.Website.Models.AboutUsPage;
using Project.AmbujaCement.Website.Models.DealerResult;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.DealerResult
{
    public interface IDealerResultService
    {
        DealerResultModel GetDealerResultModel(Rendering rendering);
    }
}
