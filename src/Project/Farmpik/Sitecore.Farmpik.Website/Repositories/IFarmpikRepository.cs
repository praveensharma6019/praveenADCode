using Sitecore.Farmpik.Website.Models;

namespace Sitecore.Farmpik.Website.Repositories
{
    public interface IFarmpikRepository
    {
        ActivityModel GetRecentActivities(string ItemId);
        KnowledgeHubTabsModel GetKnowledgeHubTabsModel();
    }
}
