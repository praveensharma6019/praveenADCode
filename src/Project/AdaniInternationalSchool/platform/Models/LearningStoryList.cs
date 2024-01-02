using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class LearningStoryList
    {
        public string Heading { get; set; }
        public string LinkText { get; set; }
        public string Link { get; set; }
        public GtmDataModel GtmData { get; set; }

        public List<LearningStoryListDataModel> StoryList { get; set; }
    }

    public class LearningStoryListDataModel
    {
        public string EventTxt { get; set; }
        public bool UpcomingEvent { get; set; }
        public string StoryHeading { get; set; }
        public string EventDate { get; set; }
        public string ImageSource { get; set; }
        public string ImageAlt { get; set; }
        public GtmDataModel GtmData { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
    }

}