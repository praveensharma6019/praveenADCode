using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class LifeAtSchoolFindOutMoreDataModel:ImageContentModel
    {
        public string Url { get; set; }
        public string Label { get; set; }
        public string Target { get; set; }
        public GtmDataModel GtmData { get; set; }
    }
}