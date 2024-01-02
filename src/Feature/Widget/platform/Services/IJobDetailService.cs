using Adani.SuperApp.Realty.Feature.Widget.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.Services
{
    public interface IJobDetailService
    {
        JobDetails GetJobDetails(Item jobdetail);

        ProjectAction GetProjectAction(Item projectAction);

    }
}
