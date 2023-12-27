using Project.AdaniOneSEO.Website.Models.FlightsToDestination;
using Sitecore.Mvc.Presentation;
using System.Threading.Tasks;

namespace Project.AdaniOneSEO.Website.Services.FlightsToDestination.Filter_Options
{
    public interface IFilterOptions
    {
        FilterOptionsModel GetFilterOptions(Rendering rendering);
        //Task<FilterOptionsModel> GetFilterOptions(Rendering rendering);
    }
}