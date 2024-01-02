using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class SimilarProjectsDataModel
    {
        public SimilarProjectsData similarProjectsData { get; set; }
    }

    public class SimilarProject
    {
        public string src { get; set; }
        public string srcAlt { get; set; }
        public string logosrc { get; set; }
        public string logoAlt { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string link { get; set; }
        public string linkTarget { get; set; }
    }
    
    public class SimilarProjectsData
    {
        public string heading { get; set; }
        public List<SimilarProject> data { get; set; }
    }


}