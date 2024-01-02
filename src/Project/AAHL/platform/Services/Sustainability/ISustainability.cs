using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Models.Our_Expertise;
using Project.AAHL.Website.Models.OurAirports;
using Project.AAHL.Website.Models.OurExpertise;
using Project.AAHL.Website.Models.OurStory;
using Project.AAHL.Website.Models.Sustainability;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.Common
{
    public interface ISustainability
    {
        SustainabilityStoriesModel GetSustainabilityStories(Rendering rendering);
        SustainabilityModel GetSustainability(Rendering rendering);
    }
}
