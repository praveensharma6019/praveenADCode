using Project.AmbujaCement.Website.Models;
using Project.AmbujaCement.Website.Models.CostCalculator;
using Project.AmbujaCement.Website.Models.Home;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.CostCalculator
{
    public interface ICostCalculatorServices
    {
        CostCalculatorPageData GetCostCalculatorPageData(Rendering rendering);
    }
}
