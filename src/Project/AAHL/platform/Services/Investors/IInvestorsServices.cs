using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Models.InvestorDownloads;
using Project.AAHL.Website.Models.Investors;
using Project.AAHL.Website.Models.OurAirports;
using Project.AAHL.Website.Models.OurStory;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.Investors
{
    public interface IInvestorsServices
    {
        InvestorModel GetInvestors(Rendering rendering);
        InvestorDownloadsModel GetInvestorDownload(Rendering rendering);
        PerformanceModel GetPerformance(Rendering rendering);
    }
}
