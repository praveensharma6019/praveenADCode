using Project.AmbujaCement.Website.Models.HomeBuilder;
using Project.AmbujaCement.Website.Models.SiteMap;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.HomeBuilder
{
    public interface ISubNavService
    {
        SubNavModel GetSubNav(Rendering rendering);
        OverviewModel GetOverview(Rendering rendering);
        VectorCardModel GetVectorCard(Rendering rendering);
        TwoColumnListModel GetColumnList(Rendering rendering);
        CardListModel GetCardList(Rendering rendering);
        SelectingTechModel GetSelectingTech(Rendering rendering);
        BisStandardsModel GetBisstandard(Rendering rendering);
        HomeBuilderModel GetHomeBuilderCard(Rendering rendering); 
    }
}
