using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class SteamCard:BaseContentModel
    {
        public List<SteamCardDataModel> LearningStoryListData { get; set; }
    }

    public class SteamCardDataModel : ImageModel
    {
        public string Label { get; set; }
        public string Theme { get; set; }
    }

}