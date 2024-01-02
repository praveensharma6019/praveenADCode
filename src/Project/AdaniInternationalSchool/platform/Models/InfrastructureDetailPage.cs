using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class InfrastructureDetailPage
    {
        public string BackLabel { get; set; }

        public GtmDataModel GtmData { get; set; }

        public InfraOverview StoryOverview { get; set; }
        public InfrastructureDetailStory StoryCarousel { get; set; }

    }

    public class InfrastructureDetailStory
    {
        public List<InfrastructureDetailCarousel> CarouselData { get; set; }
    }

    public class InfraOverview
    {
        public string Heading { get; set; }
        public string Description { get; set; }
    }
}