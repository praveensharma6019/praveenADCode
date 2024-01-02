using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class CompletedProjectsModel
    {
        public string projectID { get; set; }
        public List<CompletedPropertyType> data { get; set; }
    }
    public class CompletedPropertyType
    {
        public string heading { get; set; }
        public List<CompletedProjectDetails> data { get; set; }
    }
    public class CompletedProjectDetails
    {
        public string imageSource { get; set; }
        public string imageAlt { get; set; }
        public string projectName { get; set; }
        public string link { get; set; }
        public string target { get; set; }
        public string projectArea { get; set; }
        public string areaTitle { get; set; }
        public string areaDesc { get; set; }

    }
}