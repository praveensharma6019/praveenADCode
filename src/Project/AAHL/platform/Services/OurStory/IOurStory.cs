using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Models.OurStory;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.Common
{
    public interface IOurStory
    {
        Highlights GetHighlights(Rendering rendering);
        OurTimelineCarousel GetOurTimelineCarousel(Rendering rendering);
        FutureGrowthThroughExcellenace GetFutureGrowthThroughExcellenace(Rendering rendering);
    }
}
