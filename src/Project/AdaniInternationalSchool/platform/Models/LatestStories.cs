using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class LatestStories
    {
        public string Heading { get; set; }
        public string NoData { get; set; }
        public int ShowDataDesktop { get; set; }
        public int ShowDataMobile { get; set; }
        public List<StoryItemModel> StoriesData { get; set; }
    }

    public class StoryItemModel: ImageModel
    {
        public string Id { get; set; }
        [JsonIgnore]
        public DateTime Date { get; set; }
        public string CardDate { get; set; }
        public string CardHeading { get; set; }
        public string CardDescription { get; set; }
        public string CardLink { get; set; }

        public GtmDataModel GtmData { get; set; }
        public List<Filter> Filter { get; set; }

    }

    public class Filter
    {
        public string Placeholder { get; set; }
        public string Label { get; set; }
        public string Id { get; set; }
    }
}