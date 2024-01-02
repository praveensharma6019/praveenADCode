using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class OurProjectModel
    {
        public OurProjects ourProjects { get; set; }
    }

    public class OurProjectData
    {
        public string link { get; set; }
        public string src { get; set; }
        public string imgalt { get; set; }
        public string imgtitle { get; set; }
        public string projectcity { get; set; }
        public string projecttitle { get; set; }

        public List<Projectlist> projectlist { get; set; }
        public SeeAll seeAll { get; set; }
        public List<string> propertyType { get; set; }
       
    }

    public class OurProjects
    {
        public string heading { get; set; }
        public List<OurProjectData> data { get; set; }
    }

    public class Projectlist
    {
        public string projecttitle { get; set; }
        public string projectprice { get; set; }
        public string link { get; set; }
        public string propertyType { get; set; }
    }

    public class SeeAll
    {
        public string title { get; set; }
        public string link { get; set; }
    }
}