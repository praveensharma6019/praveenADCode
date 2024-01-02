using Project.AAHL.Website.Models.AboutAHHL;
using Project.AAHL.Website.Models.AboutUs;
using Project.AAHL.Website.Models.Common;
using Project.AAHL.Website.Models.GeneralAviation;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.GeneralAviation
{
    public interface IGeneralAviationServices
    {
        ImageDetailModel GetGeneralAviation(Rendering rendering);
        DetailsChildModel GetEfficiencyRedefined(Rendering rendering);
    }
}