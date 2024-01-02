using Project.AmbujaCement.Website.Models;
using Project.AmbujaCement.Website.Models.CostCalculator;
using Project.AmbujaCement.Website.Models.CostCalculatorClientAPI;
using Project.AmbujaCement.Website.Models.Home;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.CostCalculatorClientAPI
{
    public interface ICostCalculatorClientAPIServices
    {
        CostCalculatorClientAPIModel GetCostCalculatorClientAPIData(Rendering rendering);
        ClientAPIDropdownModel GetCostCalculatorClientAPIDropDownData(Rendering rendering);
    }
}
